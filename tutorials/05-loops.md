# Tutorial 5: Loops and Iteration

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Write counting loops with `DO...LOOP` and custom increments with `+LOOP`
- 🎯 Use `BEGIN...UNTIL` for condition-based loops
- 🎯 Apply `BEGIN...WHILE...REPEAT` for complex loop logic
- 🎯 Access loop counters with `I` and `J` for nested loops
- 🎯 Exit loops early with `LEAVE`
- 🎯 Choose the right loop construct for different iteration patterns
- 🎯 Understand how Forth loops compare to for/while loops in other languages

**Connection to other languages:** Forth's loop constructs map directly to assembly-level branch instructions. Understanding these prepares you for optimizing loops in any language and recognizing compiler optimizations.

## Introduction

Loops let you repeat operations efficiently. Forth provides several looping constructs for different use cases:
- `DO...LOOP` for counting loops
- `BEGIN...UNTIL` for loops that run until a condition is true
- `BEGIN...WHILE...REPEAT` for more complex conditional loops

## The DO...LOOP Structure

The most common loop structure for iterating a fixed number of times.

### Basic DO...LOOP

**Syntax:**
```forth
limit start DO
    \ loop body
LOOP
```

### Example: Count from 0 to 4

```forth
: count-to-5
    5 0 DO
        I .
    LOOP
;
```

Test it:
```forth
count-to-5      \ 0 1 2 3 4
```

**How it works:**
- `5 0 DO` sets up a loop from 0 to 4 (5 is the limit, not included)
- `I` gets the current loop counter
- `LOOP` increments the counter and repeats if counter < limit

### Example: Print a Square

```forth
: squares
    10 0 DO
        I dup * .
    LOOP
;
```

Test it:
```forth
squares         \ 0 1 4 9 16 25 36 49 64 81
```

### The Loop Index: I

`I` returns the current loop counter:

```forth
: show-index
    5 0 DO
        ." Index: " I . cr
    LOOP
;
```

Output:
```
Index: 0
Index: 1
Index: 2
Index: 3
Index: 4
```

## DO...+LOOP with Custom Increment

Use `+LOOP` to increment by a custom amount instead of 1.

### Example: Count by Twos

```forth
: count-by-twos
    10 0 DO
        I .
    2 +LOOP
;
```

Test it:
```forth
count-by-twos   \ 0 2 4 6 8
```

### Example: Countdown

```forth
: countdown
    0 10 DO
        I .
    -1 +LOOP
;
```

Test it:
```forth
countdown       \ 10 9 8 7 6 5 4 3 2 1
```

### Example: Multiplication Table

```forth
: times-table  ( n -- )
    11 1 DO
        dup I * .
    LOOP
    drop
;
```

Test it:
```forth
7 times-table   \ 7 14 21 28 35 42 49 56 63 70
```

## Nested Loops

You can nest loops inside each other. For nested loops, use `J` for the outer loop index.

### Example: Multiplication Table Grid

```forth
: print-times-table
    11 1 DO
        11 1 DO
            I J * 4 .r
        LOOP
        cr
    LOOP
;
```

- `I` is the inner loop index
- `J` is the outer loop index
- `.r` prints right-aligned (we'll cover this more later)

### Example: Triangle Pattern

```forth
: triangle
    10 1 DO
        I 1 DO
            ." *"
        LOOP
        cr
    LOOP
;
```

Output:
```
*
**
***
****
...
```

## BEGIN...UNTIL Loop

Repeats until a condition becomes true (like do-while in C).

### Syntax

```forth
BEGIN
    \ loop body
    condition
UNTIL
```

The loop body executes at least once, then repeats until the condition is true.

### Example: Countdown to Zero

```forth
: countdown-until
    10
    BEGIN
        dup .
        1 -
        dup 0=
    UNTIL
    drop
;
```

Test it:
```forth
countdown-until  \ 10 9 8 7 6 5 4 3 2 1
```

### Example: Find First Power of 2 Greater Than N

```forth
: next-power-of-2  ( n -- power )
    1
    BEGIN
        2 *
        2dup <
    UNTIL
    swap drop
;
```

Test it:
```forth
10 next-power-of-2 .    \ 16
100 next-power-of-2 .   \ 128
```

## BEGIN...WHILE...REPEAT Loop

More flexible than UNTIL - tests condition at the start.

### Syntax

```forth
BEGIN
    condition
WHILE
    \ loop body
REPEAT
```

The condition is tested before each iteration. Like while loops in other languages.

### Example: Print Numbers While Less Than 10

```forth
: print-while-small
    1
    BEGIN
        dup 10 <
    WHILE
        dup .
        1 +
    REPEAT
    drop
;
```

Test it:
```forth
print-while-small   \ 1 2 3 4 5 6 7 8 9
```

### Example: Divide Until Can't Divide Anymore

```forth
: divide-by-2-repeatedly  ( n -- result )
    BEGIN
        dup 2 mod 0=
    WHILE
        2 /
    REPEAT
;
```

Test it:
```forth
32 divide-by-2-repeatedly .     \ 1
40 divide-by-2-repeatedly .     \ 5
```

## LEAVE: Early Exit from DO Loops

`LEAVE` exits a DO loop immediately.

### Example: Find First Multiple of 7 Greater Than 50

```forth
: first-multiple-of-7-after-50
    100 50 DO
        I 7 mod 0= IF
            I . cr
            LEAVE
        THEN
    LOOP
;
```

Test it:
```forth
first-multiple-of-7-after-50    \ 56
```

### Example: Search for Value

```forth
: contains-zero?  ( start end -- flag )
    0 -rot          \ Flag starts as false
    DO
        I 0= IF
            drop -1     \ Found it, set flag to true
            LEAVE
        THEN
    LOOP
;
```

## Practical Examples

### Example 1: Sum of Numbers

```forth
: sum-to-n  ( n -- sum )
    0 swap          \ Accumulator
    1 + 1 DO
        I +
    LOOP
;
```

Test it:
```forth
10 sum-to-n .   \ 55 (1+2+3+...+10)
```

### Example 2: Factorial

```forth
: factorial  ( n -- n! )
    dup 1 <= IF
        drop 1
    ELSE
        1 swap
        1 + 1 DO
            I *
        LOOP
    THEN
;
```

Test it:
```forth
5 factorial .   \ 120
6 factorial .   \ 720
```

### Example 3: FizzBuzz (1 to N)

```forth
: fizzbuzz-to-n  ( n -- )
    1 + 1 DO
        I 15 mod 0= IF
            ." FizzBuzz"
        ELSE I 3 mod 0= IF
            ." Fizz"
        ELSE I 5 mod 0= IF
            ." Buzz"
        ELSE
            I .
        THEN THEN THEN
        cr
    LOOP
;
```

Test it:
```forth
15 fizzbuzz-to-n
```

### Example 4: GCD (Greatest Common Divisor)

Using Euclidean algorithm:

```forth
: gcd  ( a b -- gcd )
    BEGIN
        dup 0>
    WHILE
        2dup mod
        -rot drop
    REPEAT
    drop
;
```

Test it:
```forth
48 18 gcd .     \ 6
100 35 gcd .    \ 5
```

### Example 5: Prime Check

```forth
: is-prime?  ( n -- flag )
    dup 2 < IF
        drop 0      \ Numbers less than 2 are not prime
    ELSE
        dup 2 = IF
            drop -1     \ 2 is prime
        ELSE
            -1 swap     \ Assume prime
            dup 1 + 2 DO
                dup I mod 0= IF
                    drop 0 LEAVE    \ Found divisor, not prime
                THEN
            LOOP
            swap drop
        THEN
    THEN
;
```

Test it:
```forth
7 is-prime? .   \ -1 (true)
9 is-prime? .   \ 0 (false)
17 is-prime? .  \ -1 (true)
```

## Infinite Loops

You can create infinite loops (use Ctrl+C to break):

```forth
: infinite
    BEGIN
        ." Forever! " cr
    0 UNTIL         \ 0 is always false, so never exits
;
```

Or more explicitly:
```forth
: infinite2
    BEGIN
        ." Looping... " cr
    AGAIN           \ AGAIN always loops back
;
```

Note: `AGAIN` is like `UNTIL` but doesn't pop a condition - it always loops.

## Loop Variables on Return Stack

Loop counters are stored on the return stack. This means:
- Don't mess with the return stack inside DO loops
- Don't use `>R` and `R>` carelessly in loops

### Example: Safe Use

```forth
: safe-loop
    10 0 DO
        I dup *     \ Use I normally
        .
    LOOP
;
```

### Example: Unsafe (Don't Do This!)

```forth
\ WRONG: Messes with return stack
: unsafe-loop
    10 0 DO
        5 >r        \ BAD: Corrupts loop counter
        I .
        r> drop
    LOOP
;
```

## Common Patterns

### Pattern: Accumulate a Result

```forth
: sum-squares  ( n -- sum )
    0 swap              \ Accumulator
    1 + 1 DO
        I dup * +
    LOOP
;
```

### Pattern: Transform Each Element

```forth
: print-doubled  ( n -- )
    1 + 1 DO
        I 2 * .
    LOOP
;
```

### Pattern: Find First Match

```forth
: find-first-even  ( start end -- n )
    DO
        I 2 mod 0= IF
            I LEAVE
        THEN
    LOOP
;
```

## Common Mistakes

### 1. Off-by-One Errors

```forth
\ This loops 0,1,2,3,4 (5 times)
5 0 DO I . LOOP

\ This loops 1,2,3,4 (4 times)
5 1 DO I . LOOP

\ To include 5: use 6 as limit
6 1 DO I . LOOP     \ 1,2,3,4,5
```

### 2. Forgetting to Drop After Loops

```forth
\ WRONG: Leaves value on stack
: bad-loop
    5 0 DO
        I
    LOOP        \ Stack now has 0,1,2,3,4 !
;

\ RIGHT: Process or drop values
: good-loop
    5 0 DO
        I .     \ Print each value
    LOOP
;
```

### 3. Modifying Loop Counter

```forth
\ DON'T: Can't modify I directly
\ The loop counter is on the return stack
```

## Exercises

1. Print numbers 1 to 10
2. Print even numbers from 0 to 20
3. Calculate the sum of numbers from 1 to 100
4. Print a countdown from 10 to 1
5. Print the first 10 squares (1�, 2�, ..., 10�)
6. Implement a word that checks if a number is prime
7. Print a 5x5 grid of asterisks using nested loops
8. Calculate N! (factorial) using a loop
9. Find the sum of all multiples of 3 or 5 below 100
10. Implement FizzBuzz for numbers 1 to 30

## Solutions

1.
```forth
: print-1-to-10
    11 1 DO I . LOOP
;
```

2.
```forth
: print-evens
    21 0 DO
        I 2 mod 0= IF I . THEN
    LOOP
;
\ Or with +LOOP:
: print-evens
    21 0 DO I . 2 +LOOP
;
```

3.
```forth
: sum-1-to-100
    0 101 1 DO I + LOOP .
;
```

4.
```forth
: countdown
    0 11 DO I . -1 +LOOP
;
```

5.
```forth
: squares
    11 1 DO I dup * . LOOP
;
```

6.
```forth
: prime?  ( n -- flag )
    dup 2 < IF
        drop 0
    ELSE
        -1 swap
        dup 2 / 2 DO
            over I mod 0= IF
                swap drop 0 swap LEAVE
            THEN
        LOOP
        swap drop
    THEN
;
```

7.
```forth
: grid
    5 0 DO
        5 0 DO
            ." * "
        LOOP
        cr
    LOOP
;
```

8.
```forth
: factorial  ( n -- n! )
    1 swap 1 + 1 DO I * LOOP
;
```

9.
```forth
: sum-multiples
    0
    100 1 DO
        I 3 mod 0= I 5 mod 0= or IF
            I +
        THEN
    LOOP
    .
;
```

10.
```forth
: fizzbuzz-30
    31 1 DO
        I 15 mod 0= IF
            ." FizzBuzz"
        ELSE I 3 mod 0= IF
            ." Fizz"
        ELSE I 5 mod 0= IF
            ." Buzz"
        ELSE
            I .
        THEN THEN THEN
        cr
    LOOP
;
```

## Quick Reference

### DO Loops
```forth
\ Basic loop
limit start DO
    I               \ Current index
    \ body
LOOP

\ Custom increment
limit start DO
    I
    \ body
n +LOOP

\ Nested loops
limit1 start1 DO
    limit2 start2 DO
        J           \ Outer index
        I           \ Inner index
    LOOP
LOOP

\ Early exit
DO
    \ ...
    condition IF LEAVE THEN
LOOP
```

### Conditional Loops
```forth
\ Repeat until true
BEGIN
    \ body
    condition
UNTIL

\ While loop
BEGIN
    condition
WHILE
    \ body
REPEAT

\ Infinite loop
BEGIN
    \ body
AGAIN
```

## What's Next?

In [Tutorial 6: Variables and Memory](06-variables.md), you'll learn:
- Creating and using variables
- The `@` (fetch) and `!` (store) operations
- Constants vs variables
- Creating arrays
- Memory allocation with `ALLOT`

Variables let you store state between word executions!
