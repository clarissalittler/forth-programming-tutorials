# Tutorial 4: Conditionals and Logic

## Introduction

Real programs need to make decisions. In this tutorial, you'll learn how to use conditional statements and logical operations in Forth.

## Boolean Values in Forth

Forth uses a simple convention for boolean values:

- **True**: `-1` (all bits set to 1)
- **False**: `0` (all bits zero)

Any non-zero value is considered "truthy", but true flag operations return `-1`.

```forth
0 .             \ 0 (false)
-1 .            \ -1 (true)
```

## Comparison Operators

Forth provides standard comparison operators that return boolean flags.

### Basic Comparisons

| Word | Stack Effect | Description | Example |
|------|--------------|-------------|---------|
| `=` | `( n1 n2 -- flag )` | Equal | `5 5 = .` ’ -1 |
| `<>` | `( n1 n2 -- flag )` | Not equal | `5 3 <> .` ’ -1 |
| `<` | `( n1 n2 -- flag )` | Less than | `3 5 < .` ’ -1 |
| `>` | `( n1 n2 -- flag )` | Greater than | `5 3 > .` ’ -1 |
| `<=` | `( n1 n2 -- flag )` | Less or equal | `3 5 <= .` ’ -1 |
| `>=` | `( n1 n2 -- flag )` | Greater or equal | `5 5 >= .` ’ -1 |

### Examples:

```forth
\ Equality
5 5 = .         \ -1 (true)
5 3 = .         \ 0 (false)

\ Inequality
5 3 <> .        \ -1 (true)
5 5 <> .        \ 0 (false)

\ Less than
3 5 < .         \ -1 (true)
5 3 < .         \ 0 (false)

\ Greater than
5 3 > .         \ -1 (true)
3 5 > .         \ 0 (false)
```

### Zero Comparisons

Special shortcuts for comparing with zero:

| Word | Stack Effect | Description |
|------|--------------|-------------|
| `0=` | `( n -- flag )` | Equal to zero |
| `0<>` | `( n -- flag )` | Not equal to zero |
| `0<` | `( n -- flag )` | Less than zero (negative) |
| `0>` | `( n -- flag )` | Greater than zero (positive) |

```forth
0 0= .          \ -1 (true)
5 0= .          \ 0 (false)

-5 0< .         \ -1 (true, negative)
5 0> .          \ -1 (true, positive)
```

## Logical Operators

Combine boolean values with logical operators.

| Word | Stack Effect | Description |
|------|--------------|-------------|
| `and` | `( flag1 flag2 -- flag )` | Logical AND |
| `or` | `( flag1 flag2 -- flag )` | Logical OR |
| `xor` | `( flag1 flag2 -- flag )` | Logical XOR |
| `invert` | `( flag -- !flag )` | Logical NOT |

### AND - Both Must Be True

```forth
-1 -1 and .     \ -1 (true AND true = true)
-1 0 and .      \ 0 (true AND false = false)
0 -1 and .      \ 0 (false AND true = false)
0 0 and .       \ 0 (false AND false = false)
```

### OR - At Least One Must Be True

```forth
-1 -1 or .      \ -1 (true OR true = true)
-1 0 or .       \ -1 (true OR false = true)
0 -1 or .       \ -1 (false OR true = true)
0 0 or .        \ 0 (false OR false = false)
```

### XOR - Exactly One Must Be True

```forth
-1 -1 xor .     \ 0 (true XOR true = false)
-1 0 xor .      \ -1 (true XOR false = true)
0 -1 xor .      \ -1 (false XOR true = true)
0 0 xor .       \ 0 (false XOR false = false)
```

### INVERT - Logical NOT

```forth
-1 invert .     \ 0 (NOT true = false)
0 invert .      \ -1 (NOT false = true)
```

## The IF-THEN Structure

Basic conditional execution: do something if a condition is true.

### Syntax

```forth
condition IF
    \ code to execute if true
THEN
```

### Example: Positive Number Check

```forth
: check-positive  ( n -- )
    0 > IF
        ." The number is positive!" cr
    THEN
;
```

Test it:
```forth
5 check-positive        \ Prints: The number is positive!
-3 check-positive       \ Prints nothing
```

### How It Works

1. A boolean flag must be on the stack
2. `IF` pops the flag
3. If true (-1 or any non-zero), execute code until `THEN`
4. If false (0), skip to `THEN`

### Example: Even Number Check

```forth
: even?  ( n -- )
    2 mod 0= IF
        ." Even!" cr
    THEN
;
```

Test it:
```forth
4 even?         \ Prints: Even!
5 even?         \ Prints nothing
```

## The IF-ELSE-THEN Structure

Execute different code based on a condition.

### Syntax

```forth
condition IF
    \ code if true
ELSE
    \ code if false
THEN
```

### Example: Even or Odd

```forth
: even-or-odd  ( n -- )
    2 mod 0= IF
        ." Even" cr
    ELSE
        ." Odd" cr
    THEN
;
```

Test it:
```forth
4 even-or-odd       \ Even
5 even-or-odd       \ Odd
```

### Example: Absolute Value

```forth
: my-abs  ( n -- |n| )
    dup 0< IF
        negate
    ELSE
        \ Do nothing, number is already positive
    THEN
;
```

Test it:
```forth
-5 my-abs .     \ 5
5 my-abs .      \ 5
```

### Example: Max of Two Numbers

```forth
: my-max  ( n1 n2 -- max )
    2dup > IF
        drop
    ELSE
        swap drop
    THEN
;
```

Test it:
```forth
10 20 my-max .      \ 20
20 10 my-max .      \ 20
```

## Nested Conditionals

You can nest IF statements inside each other.

### Example: Number Classification

```forth
: classify  ( n -- )
    dup 0= IF
        drop ." Zero" cr
    ELSE
        0< IF
            ." Negative" cr
        ELSE
            ." Positive" cr
        THEN
    THEN
;
```

Test it:
```forth
0 classify      \ Zero
-5 classify     \ Negative
5 classify      \ Positive
```

### Example: Grade Classification

```forth
: letter-grade  ( score -- )
    dup 90 >= IF
        drop ." A" cr
    ELSE dup 80 >= IF
        drop ." B" cr
    ELSE dup 70 >= IF
        drop ." C" cr
    ELSE dup 60 >= IF
        drop ." D" cr
    ELSE
        drop ." F" cr
    THEN THEN THEN THEN
;
```

Test it:
```forth
95 letter-grade     \ A
75 letter-grade     \ C
55 letter-grade     \ F
```

## Complex Conditions

Combine comparisons with logical operators.

### Example: Range Check

```forth
: in-range?  ( n low high -- flag )
    rot dup rot >= -rot <= and
;

: check-range  ( n -- )
    dup 1 10 in-range? IF
        ." In range [1,10]" cr
    ELSE
        ." Out of range" cr
    THEN
    drop
;
```

Test it:
```forth
5 check-range       \ In range [1,10]
15 check-range      \ Out of range
```

### Example: Leap Year

A year is a leap year if:
- Divisible by 4 AND (not divisible by 100 OR divisible by 400)

```forth
: divisible?  ( n divisor -- flag )
    mod 0=
;

: leap-year?  ( year -- flag )
    dup 4 divisible? IF
        dup 100 divisible? IF
            400 divisible?
        ELSE
            drop -1
        THEN
    ELSE
        drop 0
    THEN
;

: check-leap-year  ( year -- )
    dup leap-year? IF
        . ." is a leap year" cr
    ELSE
        . ." is not a leap year" cr
    THEN
;
```

Test it:
```forth
2000 check-leap-year    \ 2000 is a leap year
1900 check-leap-year    \ 1900 is not a leap year
2024 check-leap-year    \ 2024 is a leap year
2023 check-leap-year    \ 2023 is not a leap year
```

## The CASE Statement

For multiple conditions, `CASE` is cleaner than nested IFs.

### Syntax

```forth
n CASE
    value1 OF
        \ code for value1
    ENDOF
    value2 OF
        \ code for value2
    ENDOF
    \ default case (optional)
ENDCASE
```

### Example: Day of Week

```forth
: day-name  ( n -- )
    CASE
        1 OF ." Monday" ENDOF
        2 OF ." Tuesday" ENDOF
        3 OF ." Wednesday" ENDOF
        4 OF ." Thursday" ENDOF
        5 OF ." Friday" ENDOF
        6 OF ." Saturday" ENDOF
        7 OF ." Sunday" ENDOF
        ." Invalid day"
    ENDCASE
    cr
;
```

Test it:
```forth
1 day-name      \ Monday
5 day-name      \ Friday
9 day-name      \ Invalid day
```

### Example: Simple Calculator

```forth
: calculator  ( a b op -- result )
    CASE
        '+' OF + ENDOF
        '-' OF - ENDOF
        '*' OF * ENDOF
        '/' OF / ENDOF
        ." Unknown operator" drop drop 0 swap
    ENDCASE
;
```

Test it:
```forth
10 5 '+' calculator .   \ 15
10 5 '-' calculator .   \ 5
10 5 '*' calculator .   \ 50
10 5 '/' calculator .   \ 2
```

Note: `'+'` is the ASCII code for the character '+'.

## Conditional Stack Effects

Be careful with stack effects in conditionals!

### Wrong: Unbalanced Stack

```forth
\ WRONG: Unbalanced stack effects
: broken  ( n -- ? )
    dup 0> IF
        dup *       \ Leaves one value
    ELSE
        \ Leaves nothing!
    THEN
;
```

### Right: Balanced Stack

```forth
\ RIGHT: Both branches have same effect
: correct  ( n -- n² )
    dup 0> IF
        dup *       \ Leaves n²
    ELSE
        0           \ Leaves 0
    THEN
;
```

**Rule**: Both IF and ELSE branches must have the same stack effect.

## Short-Circuit Evaluation

Forth doesn't do short-circuit evaluation automatically. You need to structure it yourself.

### Example: Safe Division

```forth
: safe-divide  ( n divisor -- quotient )
    dup 0= IF
        ." Error: Division by zero!" cr
        drop drop 0
    ELSE
        /
    THEN
;
```

Test it:
```forth
10 5 safe-divide .      \ 2
10 0 safe-divide .      \ Error: Division by zero! 0
```

## Practical Examples

### Example 1: Sign Function

Returns -1, 0, or 1 based on sign.

```forth
: sign  ( n -- -1|0|1 )
    dup 0= IF
        \ It's zero
    ELSE dup 0< IF
        drop -1
    ELSE
        drop 1
    THEN THEN
;
```

Test it:
```forth
5 sign .        \ 1
-5 sign .       \ -1
0 sign .        \ 0
```

### Example 2: Clamp

Restrict a value to a range.

```forth
: clamp  ( n min max -- clamped )
    rot                 \ ( min max n )
    dup rot             \ ( min n n max )
    min swap            \ ( min n' max ) where n' = min(n,max)
    rot max             \ max(min, n')
;
```

Test it:
```forth
50 0 100 clamp .        \ 50 (in range)
150 0 100 clamp .       \ 100 (clamped to max)
-10 0 100 clamp .       \ 0 (clamped to min)
```

### Example 3: FizzBuzz (Single Number)

```forth
: fizzbuzz  ( n -- )
    dup 15 mod 0= IF
        drop ." FizzBuzz"
    ELSE dup 3 mod 0= IF
        drop ." Fizz"
    ELSE dup 5 mod 0= IF
        drop ." Buzz"
    ELSE
        .
    THEN THEN THEN
    cr
;
```

Test it:
```forth
3 fizzbuzz      \ Fizz
5 fizzbuzz      \ Buzz
15 fizzbuzz     \ FizzBuzz
7 fizzbuzz      \ 7
```

## Common Mistakes

### 1. Forgetting the Flag

```forth
\ WRONG: No flag on stack!
: broken
    IF ." Hello" THEN
;

\ RIGHT: Need a flag
: correct
    -1 IF ." Hello" THEN
;
```

### 2. Unbalanced Branches

```forth
\ WRONG: Different stack effects
: broken  ( n -- ? )
    IF dup ELSE THEN
;

\ RIGHT: Same stack effect
: correct  ( n -- n )
    IF dup ELSE 0 THEN
;
```

### 3. Mixing Up Comparison Order

```forth
\ These are different!
10 5 > .        \ -1 (10 > 5, true)
5 10 > .        \ 0 (5 > 10, false)
```

## Exercises

1. Write a word `positive?` that checks if a number is positive
2. Write a word `max3` that returns the maximum of three numbers
3. Write a word `abs` that returns absolute value
4. Write a word `sign` as shown above
5. Write a word `between?` that checks if `n` is between `low` and `high` (inclusive)
6. Write a word that categorizes a number as small (<10), medium (10-99), or large (>=100)
7. Write a word `compare` that prints "greater", "less", or "equal" for two numbers
8. Implement a simple menu system using CASE
9. Write a word that checks if a number is divisible by both 2 and 3
10. Write a word that classifies triangles (equilateral, isosceles, scalene)

## Solutions

1.
```forth
: positive?  ( n -- flag )
    0 >
;
```

2.
```forth
: max3  ( a b c -- max )
    max max
;
```

3.
```forth
: abs  ( n -- |n| )
    dup 0< IF negate THEN
;
```

4.
```forth
: sign  ( n -- -1|0|1 )
    dup 0= IF
        drop 0
    ELSE 0< IF
        -1
    ELSE
        1
    THEN THEN
;
```

5.
```forth
: between?  ( n low high -- flag )
    rot dup rot >= -rot <= and
;
```

6.
```forth
: categorize  ( n -- )
    dup 10 < IF
        drop ." Small" cr
    ELSE dup 100 < IF
        drop ." Medium" cr
    ELSE
        drop ." Large" cr
    THEN THEN
;
```

7.
```forth
: compare  ( a b -- )
    2dup > IF
        2drop ." greater" cr
    ELSE 2dup < IF
        2drop ." less" cr
    ELSE
        2drop ." equal" cr
    THEN THEN
;
```

8.
```forth
: menu  ( choice -- )
    CASE
        1 OF ." You selected Option 1" cr ENDOF
        2 OF ." You selected Option 2" cr ENDOF
        3 OF ." You selected Option 3" cr ENDOF
        ." Invalid choice" cr
    ENDCASE
;
```

9.
```forth
: divisible-by-2-and-3?  ( n -- flag )
    dup 2 mod 0= swap 3 mod 0= and
;
```

10.
```forth
: triangle-type  ( a b c -- )
    rot rot             \ Arrange sides
    2dup = IF
        drop swap = IF
            ." Equilateral" cr
        ELSE
            drop ." Isosceles" cr
        THEN
    ELSE
        = IF
            ." Isosceles" cr
        ELSE
            ." Scalene" cr
        THEN
    THEN
;
```

## Quick Reference

### Comparisons
- `=` `<>` `<` `>` `<=` `>=` - Binary comparisons
- `0=` `0<>` `0<` `0>` - Compare with zero

### Logic
- `and` `or` `xor` - Logical operations
- `invert` - Logical NOT

### Conditionals
```forth
\ IF-THEN
flag IF code THEN

\ IF-ELSE-THEN
flag IF true-code ELSE false-code THEN

\ CASE
n CASE
    val1 OF code1 ENDOF
    val2 OF code2 ENDOF
    default-code
ENDCASE
```

## What's Next?

In [Tutorial 5: Loops and Iteration](05-loops.md), you'll learn:
- `DO` loops for counting
- `BEGIN-UNTIL` for condition-based loops
- `BEGIN-WHILE-REPEAT` for more complex loops
- Iterating over ranges
- Breaking out of loops early

Get ready to make your programs repeat tasks efficiently!
