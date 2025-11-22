# Forth Debugging Guide

## Introduction

Debugging Forth code can be challenging because of its stack-based nature and concatenative style. This guide provides strategies, tools, and techniques to help you find and fix bugs quickly.

## The Golden Rules of Forth Debugging

1. **Use `.s` constantly** - Check the stack after every operation
2. **Test incrementally** - Test each word immediately after defining it
3. **Start simple** - Build complex words from tested simple ones
4. **Read error messages carefully** - They often tell you exactly what's wrong
5. **Know your stack effects** - Document every word with `( -- )` notation

## Common Errors and Solutions

### 1. Stack Underflow

**Error message:**
```
Stack underflow
```

**Cause:** Trying to pop from an empty stack.

**Example:**
```forth
5 + .       \ Error! + needs TWO numbers
```

**Solutions:**
```forth
\ WRONG:
: double + ;

\ RIGHT:
: double  ( n -- 2n )
    dup + ;

\ Check with .s before operations:
.s          \ Make sure you have enough items
```

**Debugging technique:**
```forth
: debug-word
    ." Before operation: " .s cr
    +                          \ Your operation
    ." After operation: " .s cr
;
```

### 2. Stack Overflow

**Error message:**
```
Stack overflow
```

**Cause:** Too many items pushed without being consumed.

**Example:**
```forth
: bad-loop  ( -- )
    100 0 do
        i           \ Pushes index but never consumes it!
    loop
;                    \ Stack now has 100 items
```

**Solutions:**
```forth
\ WRONG:
: bad-loop
    100 0 do i loop     \ Leaves 100 items on stack
;

\ RIGHT:
: good-loop
    100 0 do i . loop   \ Print and consume each item
;
```

### 3. Undefined Word

**Error message:**
```
Undefined word
>>>foo<<<
```

**Cause:** Word doesn't exist or was misspelled.

**Common causes:**
```forth
\ Typo
: square dup * ;
5 sqare .           \ Error! Misspelled

\ Wrong case (case-sensitive)
: Double dup + ;
5 double .          \ Error! Should be 'Double'

\ Word not defined yet
: uses-helper helper-word ;
: helper-word ." I help!" ;
\ Error! helper-word used before defined
```

**Solutions:**
- Check spelling
- Check case (Forth is case-sensitive in standard implementations)
- Define words before using them
- Use `words` to list all defined words
- Use `words` with grep: `words` then search for your word

### 4. Division by Zero

**Error message:**
```
Division by zero
```

**Example:**
```forth
10 0 / .        \ Error!
```

**Solution:**
```forth
: safe-divide  ( n1 n2 -- result )
    dup 0= if
        ." Error: division by zero" cr
        drop drop 0
    else
        /
    then
;

10 0 safe-divide .  \ Prints error, returns 0
```

### 5. Incorrect Stack Effects

**Problem:** Word doesn't leave stack as expected.

**Example:**
```forth
: broken-average  ( a b -- avg )
    + 2 /           \ Looks right...
;

\ But using it:
10 20 broken-average
.s                  \ Expected: <1> 15
                    \ But might have leftovers from somewhere else
```

**Solution: Clear and test in isolation:**
```forth
clearstack
10 20 broken-average
.s                  \ Should show: <1> 15
```

### 6. Wrong Number of Items on Stack

**Problem:** Operations get wrong data.

**Example:**
```forth
: confusing
    10 20 30        \ Stack: 10 20 30
    + .             \ Pops 30 and 20, prints 50
    .               \ Pops 10, prints 10
;                    \ But you might have expected different behavior
```

**Debugging with .s:**
```forth
: better-version
    10 20 30
    ." After pushing: " .s cr
    +
    ." After +: " .s cr
    .
    ." After first .: " .s cr
    .
    ." After second .: " .s cr
;
```

## Debugging Tools and Techniques

### 1. The `.s` Command (Stack Display)

Your most important debugging tool!

```forth
: mystery
    5 10
    .s              \ <2> 5 10
    +
    .s              \ <1> 15
    3
    .s              \ <2> 15 3
    *
    .s              \ <1> 45
;
```

**Tip:** Add `.s` at every step when debugging.

### 2. The `SEE` Command

Displays the definition of a word:

```forth
: square dup * ;

see square
\ Shows the compiled definition
```

Use this to:
- Verify a word's definition
- Check if a word exists
- Learn how built-in words work

### 3. The `WORDS` Command

Lists all defined words:

```forth
words
\ Shows all available words

\ To search for a specific word:
\ words | grep pattern    (in shell)
```

### 4. Temporary Print Statements

Add debugging output:

```forth
: calculate-something  ( a b -- result )
    ." Input a: " over . cr
    ." Input b: " dup . cr
    +
    ." After +: " dup . cr
    10 *
    ." After *: " dup . cr
;
```

### 5. Stack Comment Tracing

Document stack state as you go:

```forth
: complex-word  ( a b c -- result )
    +           \ ( a total )
    swap        \ ( total a )
    2 *         \ ( total 2a )
    +           \ ( final )
;
```

### 6. The `DEPTH` Command

Check stack depth:

```forth
: check-balance
    depth ." Stack depth: " . cr
    10 20 30
    depth ." Stack depth: " . cr
;
```

### 7. Isolate and Test

Test suspicious code in isolation:

```forth
\ Full program has bugs?
: big-program
    setup
    process-data
    calculate
    display-result
;

\ Test each part separately:
setup .s
clearstack

10 20 process-data .s
clearstack

5 calculate .s
```

## Common Logical Errors

### 1. Incorrect Stack Manipulation

```forth
\ WRONG: Want to compute (a + b) * a
: bad  ( a b -- result )
    + *             \ Error! Nothing left to multiply!
;

\ RIGHT:
: good  ( a b -- result )
    over            \ ( a b a )
    rot rot         \ ( a a b )
    +               \ ( a sum )
    *               \ ( result )
;

\ BETTER:
: better  ( a b -- result )
    over +          \ ( a sum )
    *               \ ( result )
;
```

### 2. Off-by-One in Loops

```forth
\ WRONG: Print 0-9 (10 numbers)
: wrong-count
    10 0 do i . loop    \ Prints 0-9 (correct!)
;

\ WRONG: Print 1-10 (10 numbers)
: wrong-count-2
    10 1 do i . loop    \ Prints 1-9 (only 9 numbers!)
;

\ RIGHT:
: right-count
    11 1 do i . loop    \ Prints 1-10 (10 numbers)
;
```

### 3. Forgetting Stack Order

```forth
\ Stack effect: ( top bottom )
10 20 .s            \ <2> 10 20  (20 is on top!)

\ WRONG: Subtract top from bottom
10 20 - .           \ -10 (computed 20 - 10)

\ RIGHT: Swap first
10 20 swap - .      \ 10 (computed 10 - 20)
```

## Step-by-Step Debugging Process

### Example: Debugging a Broken Function

```forth
\ Goal: Calculate (a² + b²) / 2
: broken-formula  ( a b -- result )
    dup * swap dup * + 2 /
;

5 3 broken-formula .    \ Expected: 17, Got: ???
```

**Step 1: Add .s everywhere**
```forth
: debug-formula  ( a b -- result )
    ." Start: " .s cr
    dup *
    ." After dup *: " .s cr
    swap
    ." After swap: " .s cr
    dup *
    ." After dup *: " .s cr
    +
    ." After +: " .s cr
    2 /
    ." After 2 /: " .s cr
;

5 3 debug-formula
```

**Step 2: Analyze output**
```
Start: <2> 5 3
After dup *: <2> 5 9     ← 3 * 3 = 9 ✓
After swap: <2> 9 5      ← Swapped ✓
After dup *: <2> 9 25    ← 5 * 5 = 25 ✓
After +: <1> 34          ← 9 + 25 = 34 ✓
After 2 /: <1> 17        ← 34 / 2 = 17 ✓
```

Actually it works! But let's verify our understanding:

**Step 3: Document what each step does**
```forth
: better-formula  ( a b -- result )
    dup *       \ ( a b² )
    swap        \ ( b² a )
    dup *       \ ( b² a² )
    +           \ ( total )
    2 /         \ ( result )
;
```

## Advanced Debugging Techniques

### 1. Create a Debug Mode

```forth
variable debug-mode

: ?debug  ( -- flag )
    debug-mode @ ;

: debug-print  ( -- )
    ?debug if
        ." Stack: " .s cr
    then
;

: my-word
    debug-print
    \ ... operations ...
    debug-print
;

\ Turn on debugging:
1 debug-mode !

\ Turn off:
0 debug-mode !
```

### 2. Assertion Testing

```forth
: assert  ( flag -- )
    0= if
        ." Assertion failed!" cr
        abort
    then
;

: test-square
    5 square 25 = assert
    10 square 100 = assert
    ." All tests passed!" cr
;
```

### 3. Trace Execution

```forth
: trace  ( -- )
    ." Executing: " >in @ source drop + swap source drop - type cr
;

\ In your code:
: my-word
    trace
    10 20 +
    trace
    30 *
    trace
;
```

## Performance Debugging

### Finding Slow Code

```forth
: benchmark  ( xt n -- )
    >r
    milliseconds >r
    r@ 0 do dup execute loop
    drop
    milliseconds r> - r> /
    ." Average time: " . ." ms" cr
;

\ Usage:
' my-word 1000 benchmark
```

### Memory Usage

```forth
: show-memory-use
    ." Dictionary pointer: " here . cr
;

show-memory-use
\ ... create some data structures ...
show-memory-use
\ Shows how much memory was allocated
```

## Debugging Checklist

When encountering a bug, check:

- [ ] Does the word compile without errors?
- [ ] What's on the stack before the word? (Use `.s`)
- [ ] What's on the stack after the word? (Use `.s`)
- [ ] Are stack effects documented correctly?
- [ ] Are there enough items on the stack?
- [ ] Is the stack order correct?
- [ ] Did you test with simple values (0, 1, 2)?
- [ ] Did you test edge cases (negative, zero, large)?
- [ ] Is the logic correct for the algorithm?
- [ ] Are loop bounds correct?
- [ ] Did you forget to `drop` or `dup` something?

## Getting Unstuck

If you're completely stuck:

1. **Simplify:** Remove complexity until it works
2. **Isolate:** Test each word separately
3. **Restart:** Sometimes starting over reveals the issue
4. **Explain:** Describe the problem out loud (rubber duck debugging)
5. **Research:** Check the tutorials or Forth documentation
6. **Take a break:** Fresh eyes often spot bugs immediately

## Examples of Real Debugging Sessions

### Example 1: FizzBuzz Bug

**Broken code:**
```forth
: fizzbuzz  ( n -- )
    dup 15 mod 0= if ." FizzBuzz" then
    dup 3 mod 0= if ." Fizz" then
    dup 5 mod 0= if ." Buzz" then
    drop cr
;

15 fizzbuzz     \ Prints: FizzBuzzFizzBuzz
```

**Problem:** All conditions execute, not just one!

**Fix:** Use ELSE to make them mutually exclusive:
```forth
: fizzbuzz  ( n -- )
    dup 15 mod 0= if
        ." FizzBuzz"
    else dup 3 mod 0= if
        ." Fizz"
    else dup 5 mod 0= if
        ." Buzz"
    else
        dup .
    then then then
    drop cr
;
```

### Example 2: Array Index Bug

**Broken code:**
```forth
create array 10 cells allot

: set-array  ( value index -- )
    array + !       \ WRONG!
;

42 0 set-array
```

**Problem:** Adding index directly to address!

**Debug:**
```forth
: debug-set-array  ( value index -- )
    ." Index: " dup . cr
    ." Array address: " array . cr
    ." Offset: " dup . cr
    array + ." Computed address: " dup . cr
    !
;

42 5 debug-set-array
\ Shows index is 5, not 40!
```

**Fix:**
```forth
: set-array  ( value index -- )
    cells array + !     \ Convert index to bytes!
;
```

## Quick Reference

| Command | Purpose | Example |
|---------|---------|---------|
| `.s` | Show stack contents | `.s` → `<3> 1 2 3` |
| `depth` | Stack depth | `depth .` → `3` |
| `see` | Show word definition | `see square` |
| `words` | List all words | `words` |
| `clearstack` | Empty the stack | `clearstack .s` → `<0>` |
| `bye` | Exit (useful when stuck) | `bye` |

## Conclusion

Debugging Forth requires understanding the stack deeply. The more you practice:
- Using `.s` to inspect state
- Testing incrementally
- Reading stack effects carefully
- Building from simple to complex

...the faster you'll find and fix bugs. Remember: every Forth programmer has debugged countless stack underflows. You're in good company!

Happy debugging! 🐛🔍
