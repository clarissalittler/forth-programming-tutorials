# Tutorial 3: Defining Words

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Define custom words (functions) using `:` and `;`
- 🎯 Write clear stack effect diagrams to document your words
- 🎯 Build complex programs incrementally from simple words (bottom-up design)
- 🎯 Use constants for readable, maintainable code
- 🎯 Apply proper Forth naming conventions
- 🎯 Understand how word definitions relate to functions in other languages
- 🎯 Factor code appropriately: knowing when to break down complex words

**Connection to other languages:** Defining words in Forth is like writing functions in C/Python, but compilation happens interactively. Understanding Forth's compilation model helps you grasp how compilers and interpreters work.

## Introduction

So far, we've been using Forth's built-in words like `+`, `dup`, and `swap`. Now you'll learn how to create your own words (functions) to extend the language.

This is where Forth's power becomes evident - you're not just using a programming language, you're building one that fits your problem domain.

## Your First Word Definition

In Forth, you define new words using `:` (colon) to start and `;` (semicolon) to end.

### Basic Syntax

```forth
: word-name
    \ ... code ...
;
```

### Example: A Simple Greeting

```forth
: hello
    ." Hello, World!" cr
;
```

Now you can use it:
```forth
hello           \ Prints: Hello, World!
hello           \ Prints: Hello, World!
```

You've just created a new word called `hello` that's now part of your Forth vocabulary!

## Defining Words That Use the Stack

Most words operate on the stack. Let's define a word that squares a number.

### Example: Square

```forth
: square  ( n -- n� )
    dup *
;
```

Test it:
```forth
5 square .      \ 25
10 square .     \ 100
```

The comment `( n -- n� )` is the **stack effect diagram**:
- `( n -- n� )` means: takes one number, returns its square
- This is documentation, not executable code
- It helps readers understand what the word does

### Example: Cube

```forth
: cube  ( n -- n� )
    dup dup * *
;
```

Test it:
```forth
3 cube .        \ 27
5 cube .        \ 125
```

## Stack Effect Notation

Always document your words with stack effects:

```forth
: word-name  ( inputs -- outputs )
    \ code
;
```

Examples:
```forth
: double  ( n -- 2n )
    2 *
;

: average  ( n1 n2 -- avg )
    + 2 /
;

: add-three  ( a b c -- sum )
    + +
;
```

Multiple outputs:
```forth
: sum-and-product  ( a b -- sum product )
    2dup + -rot *
;
```

Test it:
```forth
3 4 sum-and-product .s  \ <2> 7 12
```

## Naming Conventions

Forth has some common naming conventions:

### Descriptive Names
- Use lowercase with hyphens: `calculate-area`, `print-header`
- Be descriptive: `square` not `sq`, `average` not `avg`

### Special Suffixes
- `!` (store): Words that store values (e.g., `variable!`)
- `@` (fetch): Words that retrieve values (e.g., `variable@`)
- `?` (query): Words that print values (e.g., `depth?`)

### Common Patterns
- Words that print often end in `.`: `print-number.`, `show-result.`
- Boolean predicates: `is-positive`, `within-range`

## Building Vocabulary Incrementally

One of Forth's philosophies is building programs bottom-up. Define simple words, then combine them into more complex ones.

### Example: Geometry Functions

```forth
\ Basic definitions
: square  ( n -- n� )
    dup *
;

: pi  ( -- �-approx )
    314 100 /
;

\ Build on basics
: circle-area  ( radius -- area )
    square pi *
;

: circle-circumference  ( radius -- circumference )
    2 * pi *
;
```

Test them:
```forth
10 circle-area .                \ 314
10 circle-circumference .       \ 62
```

### Example: Temperature Conversion

```forth
: celsius>fahrenheit  ( C -- F )
    9 * 5 / 32 +
;

: fahrenheit>celsius  ( F -- C )
    32 - 5 * 9 /
;
```

Test them:
```forth
0 celsius>fahrenheit .      \ 32
100 celsius>fahrenheit .    \ 212
32 fahrenheit>celsius .     \ 0
212 fahrenheit>celsius .    \ 100
```

## Seeing Word Definitions

Use `see` to view a word's definition:

```forth
: double  ( n -- 2n )
    2 *
;

see double
```

This shows you the compiled definition - very useful for learning and debugging!

## Words That Print

You can define words that print formatted output.

### Example: Print Square

```forth
: print-square  ( n -- )
    dup square
    swap ." The square of " . ." is " . cr
;
```

Test it:
```forth
7 print-square      \ The square of 7 is 49
```

### Example: Print Table Header

```forth
: print-header  ( -- )
    ." Number | Square | Cube" cr
    ." -------|--------|------" cr
;

: print-row  ( n -- )
    dup dup square dup .
    ." | " .
    ." | " cube . cr
;
```

Test it:
```forth
print-header
5 print-row
6 print-row
```

## Factoring: Breaking Down Complex Words

Good Forth style involves breaking complex operations into smaller, reusable words.

### Example: Poor Factoring

```forth
\ Not good: does too much in one word
: print-stats  ( a b -- )
    2dup + ." Sum: " . cr
    2dup swap - ." Difference: " . cr
    2dup * ." Product: " . cr
    swap / ." Quotient: " . cr
;
```

### Example: Good Factoring

```forth
\ Better: separate concerns
: sum  ( a b -- sum )
    +
;

: difference  ( a b -- diff )
    -
;

: product  ( a b -- prod )
    *
;

: quotient  ( a b -- quot )
    /
;

: print-stat  ( value label-addr label-len -- )
    type ." : " . cr
;

: print-stats  ( a b -- )
    2dup sum ." Sum: " . cr
    2dup difference ." Difference: " . cr
    2dup product ." Product: " . cr
    quotient ." Quotient: " . cr
;
```

## Recursive Definitions

Words can call themselves - this is recursion.

### Example: Factorial

```forth
: factorial  ( n -- n! )
    dup 1 >        \ Is n > 1?
    if
        dup 1 -    \ n-1
        factorial  \ (n-1)!
        *          \ n * (n-1)!
    then
;
```

Test it:
```forth
5 factorial .   \ 120
6 factorial .   \ 720
```

Note: We'll cover `if/then` in the next tutorial. For now, just know that recursion is possible.

## Constants

For values that don't change, use `constant`:

```forth
355 113 / constant pi
60 constant seconds-per-minute
24 constant hours-per-day
```

Use them:
```forth
pi .                            \ 3 (approximate)
seconds-per-minute hours-per-day * . \ 1440
```

## Practical Examples

### Example 1: Hypotenuse Calculation

Calculate the hypotenuse of a right triangle: `c = (a� + b�)`

(Note: We'll approximate  using integer division for now)

```forth
: square  ( n -- n� )
    dup *
;

\ Integer approximation of square root
\ (This is simplified - real sqrt is more complex)
: approx-sqrt  ( n -- n )
    \ For this example, we'll use a simple estimate
    \ Real implementation would use Newton's method
    dup 2 /     \ Simple estimate: n H n/2 (very rough!)
;

: hypotenuse  ( a b -- c )
    square swap square + approx-sqrt
;
```

### Example 2: Even or Odd

```forth
: even?  ( n -- flag )
    2 mod 0 =
;

: odd?  ( n -- flag )
    2 mod 0 <>
;
```

Test:
```forth
4 even? .       \ -1 (true in Forth)
5 even? .       \ 0 (false in Forth)
5 odd? .        \ -1 (true)
```

Note: In Forth, `-1` (all bits set) is true, `0` is false.

### Example 3: Within Range

```forth
: within-range?  ( n low high -- flag )
    rot         \ ( low high n )
    dup rot >   \ ( high n n>low )
    -rot        \ ( n>low high n )
    swap <      \ ( n>low n<high )
    and         \ ( flag )
;
```

Test:
```forth
50 0 100 within-range? .    \ -1 (true)
150 0 100 within-range? .   \ 0 (false)
```

## Comments

Forth supports several comment styles:

### Line Comments with `\`

```forth
\ This is a comment
5 3 + .     \ Add 5 and 3
```

### Block Comments with `( ... )`

```forth
( This is a block comment
  It can span multiple lines )

: square  ( n -- n� )  \ Stack effect comment
    dup *
;
```

### Stack Effect Comments

Always use these:
```forth
: word-name  ( inputs -- outputs )
```

## Immediate Words and Compilation

When you define a word with `:`, Forth enters **compilation mode**. The words inside the definition are compiled, not executed immediately.

```forth
: test
    5 .         \ This doesn't execute now
;

test            \ NOW it executes and prints 5
```

Some words, like `\` and `."`, are **immediate** - they execute even during compilation. This is an advanced topic we'll explore later.

## Common Mistakes

### 1. Forgetting the Semicolon

```forth
\ WRONG:
: square
    dup *

\ Forth is still waiting for ;
```

### 2. Wrong Stack Effect

```forth
\ This says it takes one number
: broken  ( n -- )
    + .         \ But + needs TWO numbers! Error!
;
```

### 3. Name Collisions

```forth
\ Don't redefine built-in words unless you know what you're doing!
: +  ( a b -- result )
    * ;         \ This is confusing! Don't do this!
```

### 4. Not Testing Incrementally

```forth
\ Build and test incrementally, not all at once
: complex-function  \ Test each part as you build!
    ...
;
```

## Exercises

1. Define a word `double` that doubles a number
2. Define a word `triple` that triples a number
3. Define a word `abs` that returns the absolute value (use built-in `abs`)
4. Define a word `average` that averages two numbers
5. Define a word `print-square` that prints "The square of N is N�"
6. Define constants for common values (e.g., days in a week)
7. Define a word `max3` that finds the maximum of three numbers
8. Define a word `between?` that checks if a number is between two others
9. Define a word `greet` that takes no input and prints a personalized greeting
10. Build a word `rectangle-area` that calculates the area of a rectangle

## Solutions

1.
```forth
: double  ( n -- 2n )
    2 *
;
```

2.
```forth
: triple  ( n -- 3n )
    3 *
;
```

3.
```forth
: my-abs  ( n -- |n| )
    abs
;
\ Or implement it:
: my-abs  ( n -- |n| )
    dup 0 < if negate then
;
```

4.
```forth
: average  ( n1 n2 -- avg )
    + 2 /
;
```

5.
```forth
: print-square  ( n -- )
    dup dup square
    ." The square of " swap . ." is " . cr
;
```

6.
```forth
7 constant days-per-week
365 constant days-per-year
24 constant hours-per-day
60 constant minutes-per-hour
```

7.
```forth
: max3  ( a b c -- max )
    max max
;
\ Or more explicit:
: max3  ( a b c -- max )
    rot rot max max
;
```

8.
```forth
: between?  ( n low high -- flag )
    rot dup rot >= -rot <= and
;
```

9.
```forth
: greet  ( -- )
    ." Hello! Welcome to Forth programming!" cr
;
```

10.
```forth
: rectangle-area  ( width height -- area )
    *
;
```

## Best Practices

1. **Document with stack effects** - Always include `( inputs -- outputs )`
2. **Test incrementally** - Test each word as you write it
3. **Factor appropriately** - Break complex words into simpler ones
4. **Use descriptive names** - `calculate-average` not `ca`
5. **Keep words short** - If a word is too long, factor it
6. **Use constants** - For magic numbers and fixed values
7. **Comment when unclear** - But prefer clear code over comments

## Quick Reference

### Definition Syntax
```forth
: word-name  ( stack-effect )
    \ code
;
```

### Constants
```forth
value constant name
```

### Viewing Definitions
```forth
see word-name
```

### Comments
- `\` - Line comment
- `( ... )` - Block comment
- `( inputs -- outputs )` - Stack effect (by convention)

## What's Next?

Now that you can define your own words, you need to control program flow!

In [Tutorial 4: Conditionals and Logic](04-conditionals.md), you'll learn:
- Boolean values and logic operations
- `if/then/else` structures
- Comparison operators
- Complex conditional logic
- Case statements

This will let you write programs that make decisions!
