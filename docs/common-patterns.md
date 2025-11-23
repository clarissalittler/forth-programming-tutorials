# Forth Common Patterns & Idioms

## Introduction

This guide presents common patterns, idioms, and best practices in Forth programming. Learning these patterns will help you write clean, idiomatic Forth code and solve problems the "Forth way."

## Stack Manipulation Patterns

### Pattern: Duplicate and Use

**Problem:** Need to use a value twice

**Idiom:**
```forth
: square  ( n -- n² )
    dup * ;
```

**When to use:** Any time you need the same value multiple times

### Pattern: Keep One, Drop One

**Problem:** Have two values, only need one

**Idiom:**
```forth
\ Keep first, drop second
: keep-first  ( a b -- a )
    drop ;

\ Keep second, drop first
: keep-second  ( a b -- b )
    nip ;        \ or: swap drop
```

### Pattern: Compare and Branch

**Problem:** Compare two values and execute different code

**Idiom:**
```forth
: max  ( n1 n2 -- max )
    2dup < if swap then drop ;

\ Alternative with over
: max  ( n1 n2 -- max )
    over over < if nip else drop then ;
```

### Pattern: Rotate to Access Buried Item

**Problem:** Need to access third item on stack

**Idiom:**
```forth
: use-third  ( a b c --  ...)
    rot     \ Now c is on top: b c a
    \ ... use c ...
;
```

### Pattern: Preserve While Operating

**Problem:** Need to keep a value while operating on another

**Idiom:**
```forth
\ Using over
: do-something  ( a b -- a result )
    over +      \ a b → a a+b
;

\ Using 2dup for two values
: do-something2  ( a b -- a b result )
    2dup +      \ a b → a b a+b
;
```

## Conditional Patterns

### Pattern: Guard Clause

**Problem:** Early exit from word if condition not met

**Idiom:**
```forth
: process-positive  ( n -- )
    dup 0<= if drop exit then
    \ ... process positive number ...
;
```

### Pattern: Default Value

**Problem:** Use default if value is zero or invalid

**Idiom:**
```forth
: use-or-default  ( n -- n' )
    dup 0= if drop 42 then ;

\ More general
: default-if-zero  ( n default -- n' )
    swap dup 0= if drop else nip then ;
```

### Pattern: Clamp Value

**Problem:** Keep value within range

**Idiom:**
```forth
: clamp  ( n min max -- n' )
    rot     \ min max n
    max     \ min n'    (n' = max(n, min))
    min ;   \ n''       (n'' = min(n', max))

10 0 100 clamp .    \ 10
-5 0 100 clamp .    \ 0
150 0 100 clamp .   \ 100
```

### Pattern: Boolean to Index

**Problem:** Use boolean result to index into array or select value

**Idiom:**
```forth
\ Convert boolean to 0 or 1
: bool>index  ( flag -- 0|1 )
    0<> if 1 else 0 then ;

\ More concise
: bool>index  ( flag -- 0|1 )
    0<> 1 and ;  \ or: abs 1 min
```

## Loop Patterns

### Pattern: Iterate Array

**Problem:** Process each element of an array

**Idiom:**
```forth
\ Method 1: DO...LOOP with index
: process-array  ( array len -- )
    0 do
        dup i cells + @
        \ ... process value ...
    loop
    drop ;

\ Method 2: Using bounds
: process-array2  ( array len -- )
    cells bounds do
        i @
        \ ... process value ...
    cell +loop ;
```

### Pattern: Accumulate Result

**Problem:** Build up result while looping

**Idiom:**
```forth
\ Sum array
: sum-array  ( array len -- sum )
    0 -rot          \ 0 array len
    0 do
        over i cells + @ +
    loop
    nip ;

\ Or using a variable
variable acc

: sum-array2  ( array len -- sum )
    0 acc !
    0 do
        dup i cells + @ acc +!
    loop
    drop acc @ ;
```

### Pattern: Find First Match

**Problem:** Search for element meeting condition

**Idiom:**
```forth
\ Find first positive number
: find-positive  ( array len -- n | 0 )
    0 do
        dup i cells + @
        dup 0> if nip unloop exit then
        drop
    loop
    drop 0 ;
```

### Pattern: Count Matches

**Problem:** Count how many elements meet condition

**Idiom:**
```forth
: count-positive  ( array len -- count )
    0 -rot          \ counter array len
    0 do
        over i cells + @
        0> if swap 1+ swap then
    loop
    nip ;
```

## Memory Patterns

### Pattern: Swap Two Variables

**Problem:** Exchange values of two variables

**Idiom:**
```forth
: swap-vars  ( addr1 addr2 -- )
    over @  over @      \ a1 a2 val1 val2
    rot !   swap ! ;    \ Store val2 in a1, val1 in a2

\ More concise
: swap-vars  ( addr1 addr2 -- )
    2dup @ swap @ rot ! swap ! ;
```

### Pattern: Increment Variable

**Problem:** Add to variable value

**Idiom:**
```forth
\ Add n to variable
: add-to  ( n addr -- )
    +! ;

\ Increment by 1
: inc  ( addr -- )
    1 swap +! ;

\ Usage:
variable counter
counter inc
5 counter +!
```

### Pattern: Array Bounds Check

**Problem:** Ensure index is valid before accessing

**Idiom:**
```forth
: array-bounds-check  ( index len -- index )
    over 0< over rot >= or if
        ." Index out of bounds" cr abort
    then ;

: safe-array@  ( index array len -- value )
    >r over r> array-bounds-check drop
    cells + @ ;
```

### Pattern: Initialize Memory Block

**Problem:** Set memory region to specific value

**Idiom:**
```forth
\ Fill with zeros
: clear-memory  ( addr len -- )
    0 fill ;

\ Fill with specific value
: fill-memory  ( addr len value -- )
    rot rot fill ;

\ Initialize array with sequence
: init-sequence  ( array len -- )
    0 do
        i over i cells + !
    loop
    drop ;
```

## Defining Word Patterns

### Pattern: Computed DOES>

**Problem:** Runtime behavior depends on compile-time value

**Idiom:**
```forth
\ Create constants with units
: meters  ( n "name" -- )
    create ,
    does> @ ." meters: " . ;

: seconds  ( n "name" -- )
    create ,
    does> @ ." seconds: " . ;

100 meters distance
30 seconds duration

distance    \ meters: 100
duration    \ seconds: 30
```

### Pattern: Array with Size

**Problem:** Array that knows its own size

**Idiom:**
```forth
: sized-array  ( n "name" -- )
    create dup , cells allot
    does> ( index -- addr )
        tuck @ over >= if
            ." Index out of range" abort
        then
        cell+ cells + ;

10 sized-array my-array

42 5 my-array !
5 my-array @ .      \ 42
99 my-array !       \ Error: Index out of range
```

### Pattern: Structure Definition

**Problem:** Define record/struct type

**Idiom:**
```forth
\ Define structure
0
    cell +field point-x
    cell +field point-y
constant point-size

: point  ( x y -- point )
    point-size allocate throw >r
    r@ point-y !
    r@ point-x !
    r> ;

\ Usage:
10 20 point constant p1
p1 point-x @ .      \ 10
p1 point-y @ .      \ 20
```

## Factoring Patterns

### Pattern: Extract Common Code

**Problem:** Repeated code in multiple words

**Bad:**
```forth
: process-a
    get-data
    validate
    2 *
    store-result ;

: process-b
    get-data
    validate
    3 *
    store-result ;
```

**Good:**
```forth
: process  ( multiplier -- )
    get-data
    validate
    rot *
    store-result ;

: process-a  2 process ;
: process-b  3 process ;
```

### Pattern: Factor Out Constants

**Problem:** Magic numbers scattered in code

**Bad:**
```forth
: calculate
    15 * 32 + 9 / ;
```

**Good:**
```forth
15 constant SCALE-FACTOR
32 constant OFFSET
9 constant DIVISOR

: calculate
    SCALE-FACTOR * OFFSET + DIVISOR / ;
```

### Pattern: Separate I/O from Logic

**Problem:** Mixing computation and I/O

**Bad:**
```forth
: calculate-and-print
    + dup . ;
```

**Good:**
```forth
: calculate  ( a b -- result )
    + ;

: print-result  ( n -- )
    ." Result: " . cr ;

: calculate-and-print  ( a b -- )
    calculate print-result ;
```

## Error Handling Patterns

### Pattern: Validate and Abort

**Problem:** Check precondition and abort if invalid

**Idiom:**
```forth
: require-positive  ( n -- n )
    dup 0<= if
        ." Error: Expected positive number" cr abort
    then ;

: safe-divide  ( n1 n2 -- quot )
    dup 0= if
        ." Error: Division by zero" cr abort
    then
    / ;
```

### Pattern: Return Error Code

**Problem:** Signal error without aborting

**Idiom:**
```forth
\ Return -1 on error, 0 on success
: try-operation  ( ... -- ... ior )
    \ ... do something ...
    error-condition if -1 else 0 then ;

\ Check result
: use-operation
    try-operation if
        ." Operation failed" cr
    else
        ." Success!" cr
    then ;
```

### Pattern: Graceful Degradation

**Problem:** Provide fallback if operation fails

**Idiom:**
```forth
: try-with-fallback  ( ... -- result )
    try-operation if
        default-value
    else
        normal-result
    then ;
```

## String Patterns

### Pattern: Counted String

**Problem:** Store string with length

**Idiom:**
```forth
: create-string  ( addr len "name" -- )
    create
        dup c,          \ Store length
        here over allot \ Allocate space
        swap cmove      \ Copy string
    does> ( -- addr len )
        dup 1+ swap c@ ;

s" Hello" create-string greeting
greeting type cr    \ Hello
```

### Pattern: String Comparison

**Problem:** Check if strings are equal

**Idiom:**
```forth
: string=  ( addr1 len1 addr2 len2 -- flag )
    rot over <> if      \ Check lengths
        2drop drop false
    else
        compare 0=
    then ;

s" Hello" s" Hello" string= .   \ -1 (true)
s" Hello" s" World" string= .   \ 0 (false)
```

### Pattern: Parse Words

**Problem:** Process space-separated words

**Idiom:**
```forth
: process-words  ( -- )
    begin
        parse-name dup
    while
        \ ... process addr len ...
        2drop
    repeat
    2drop ;
```

## State Machine Pattern

**Problem:** Implement state machine

**Idiom:**
```forth
0 constant STATE-IDLE
1 constant STATE-RUNNING
2 constant STATE-PAUSED
3 constant STATE-STOPPED

variable current-state

: state-idle
    ." Idle state" cr
    STATE-RUNNING current-state ! ;

: state-running
    ." Running state" cr
    STATE-PAUSED current-state ! ;

: state-paused
    ." Paused state" cr
    STATE-STOPPED current-state ! ;

: state-stopped
    ." Stopped state" cr
    STATE-IDLE current-state ! ;

create state-table
    ' state-idle ,
    ' state-running ,
    ' state-paused ,
    ' state-stopped ,

: run-state-machine
    current-state @ cells state-table + @ execute ;

\ Run through states
STATE-IDLE current-state !
4 0 do run-state-machine loop
```

## Performance Patterns

### Pattern: Avoid Division

**Problem:** Division is slow

**Idiom:**
```forth
\ Instead of: n 2 /
: halve  ( n -- n/2 )
    1 rshift ;      \ Bit shift is faster

\ Instead of: n 4 /
: quarter  ( n -- n/4 )
    2 rshift ;
```

### Pattern: Lookup Table

**Problem:** Complex calculation repeated many times

**Idiom:**
```forth
\ Instead of calculating each time
: slow-calc  ( n -- result )
    dup * 3 + 7 mod ;

\ Precompute in table
create calc-table
    0 slow-calc ,
    1 slow-calc ,
    2 slow-calc ,
    \ ... etc
    99 slow-calc ,

: fast-calc  ( n -- result )
    cells calc-table + @ ;
```

### Pattern: Inline Constants

**Problem:** Repeated calculation of same value

**Idiom:**
```forth
\ Bad: Calculating each time
: area  ( radius -- area )
    dup * 314 100 / * ;

\ Good: Precompute constant
314 100 / constant PI

: area  ( radius -- area )
    dup * PI * ;
```

## Testing Patterns

### Pattern: Simple Assert

**Problem:** Verify result is correct

**Idiom:**
```forth
: assert=  ( actual expected -- )
    = if
        ." PASS" cr
    else
        ." FAIL" cr
    then ;

: test-square
    5 square 25 assert=
    0 square 0 assert=
    -3 square 9 assert= ;
```

### Pattern: Test Suite

**Problem:** Run multiple tests

**Idiom:**
```forth
variable test-count
variable pass-count

: reset-tests
    0 test-count !
    0 pass-count ! ;

: assert=  ( actual expected -- )
    1 test-count +!
    = if
        1 pass-count +!
        ." ✓ "
    else
        ." ✗ "
    then ;

: report-results
    cr
    ." Tests: " test-count ? cr
    ." Passed: " pass-count ? cr
    ." Failed: " test-count @ pass-count @ - . cr ;

: run-tests
    reset-tests
    \ ... tests ...
    report-results ;
```

## Anti-Patterns (What to Avoid)

### Anti-Pattern: Deep Stack Depth

**Problem:** Too many items on stack

**Bad:**
```forth
: bad-word  ( a b c d e f g h -- result )
    \ Managing 8 items is error-prone
    ...
```

**Good:**
```forth
\ Use variables or factor into smaller words
: good-word-part1  ( a b c -- result1 )
    ... ;

: good-word-part2  ( d e f -- result2 )
    ... ;

: good-word  ( a b c d e f g h -- result )
    good-word-part1
    good-word-part2
    ... ;
```

### Anti-Pattern: Not Using Stack Effects

**Problem:** Undocumented stack usage

**Bad:**
```forth
: mystery
    dup * swap 3 + / ;
```

**Good:**
```forth
: calculate  ( a b -- result )
    dup * swap 3 + / ;
```

### Anti-Pattern: Side Effects in Utility Words

**Problem:** Unexpected behavior

**Bad:**
```forth
: double  ( n -- 2n )
    dup * ." Doubled!" cr ;  \ Unexpected I/O
```

**Good:**
```forth
: double  ( n -- 2n )
    dup * ;  \ Pure calculation

: double-and-report  ( n -- 2n )
    double dup ." Doubled to " . cr ;
```

## Quick Reference: Common Idioms

| Task | Idiom | Example |
|------|-------|---------|
| Square | `dup *` | `5 dup *` → `25` |
| Cube | `dup dup * *` | `3 dup dup * *` → `27` |
| Swap vars | `over @ over @ rot ! swap !` | `a b swap-vars` |
| Clamp | `max min` | `n 0 100 clamp` |
| Inc var | `1 swap +!` | `1 counter +!` |
| Bool→0/1 | `0<> 1 and` | `flag 0<> 1 and` |
| Array sum | `0 -rot 0 do over i cells + @ + loop nip` | |
| Guard | `dup 0<= if drop exit then` | |
| Default | `dup 0= if drop 42 then` | |

## Conclusion

These patterns represent idiomatic Forth. As you gain experience, you'll recognize when to apply each pattern and develop your own variations. The key is to:

1. Keep stack depth manageable (2-3 items)
2. Factor words appropriately
3. Use descriptive names
4. Document stack effects
5. Test incrementally

Happy Forth programming!
