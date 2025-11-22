# Tutorial 1: The Stack and Basic Arithmetic

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Visualize and trace stack operations step-by-step
- 🎯 Perform integer arithmetic and understand integer division behavior
- 🎯 Convert between number bases (decimal, hexadecimal, binary)
- 🎯 Translate mathematical expressions into stack-based operations
- 🎯 Debug stack-related errors using `depth` and `.s`
- 🎯 Understand how stack discipline prevents bugs

**Connection to other languages:** The stack model underlies function calls in all languages (the "call stack"), Java/Python bytecode interpreters, and assembly language operations.

## Understanding the Stack

The stack is the heart of Forth. Every operation in Forth revolves around the stack. Think of it as a stack of plates or a stack of pancakes - you can only add to the top (push) or remove from the top (pop).

### Stack Visualization

When we write stack operations, we use "stack notation" to show what happens:

```
( before -- after )
```

For example:
- `+` has the stack effect `( n1 n2 -- sum )`
- This means: takes two numbers, leaves their sum

Let's visualize what happens when we execute `2 3 + .`:

```
      Stack
      -----
2     [2]         \ Push 2
3     [2 3]       \ Push 3
+     [5]         \ Pop 2 and 3, push 5
.     []          \ Pop and print 5
```

The top of the stack is on the right.

### Seeing the Stack with `.s`

The `.s` word is your best friend when learning Forth. It shows the stack without modifying it.

Try this in gforth:
```forth
clearstack    \ Make sure stack is empty
5 .s          \ <1> 5
10 .s         \ <2> 5 10
15 .s         \ <3> 5 10 15
```

The output `<3> 5 10 15` means:
- `<3>` - there are 3 items on the stack
- `5 10 15` - the values from bottom to top

### Stack Depth

Use `depth` to get the number of items on the stack:

```forth
clearstack
depth .       \ Prints 0
5 10 15
depth .       \ Prints 3
```

## Arithmetic Operations

### Basic Operations

Forth provides all the standard arithmetic operations:

| Word | Stack Effect | Description | Example |
|------|--------------|-------------|---------|
| `+` | `( n1 n2 -- sum )` | Addition | `5 3 + .` � 8 |
| `-` | `( n1 n2 -- diff )` | Subtraction | `10 3 - .` � 7 |
| `*` | `( n1 n2 -- prod )` | Multiplication | `6 7 * .` � 42 |
| `/` | `( n1 n2 -- quot )` | Integer division | `15 4 / .` � 3 |
| `mod` | `( n1 n2 -- rem )` | Modulo (remainder) | `15 4 mod .` � 3 |
| `/mod` | `( n1 n2 -- rem quot )` | Both remainder and quotient | `15 4 /mod . .` � 3 3 |

Let's try these:

```forth
\ Addition
100 23 + .              \ 123

\ Subtraction
50 8 - .                \ 42

\ Multiplication
12 12 * .               \ 144

\ Division (integer)
22 7 / .                \ 3 (not 3.142...)

\ Modulo
22 7 mod .              \ 1

\ Both at once
22 7 /mod .s            \ <2> 1 3
.                       \ 3 (quotient)
.                       \ 1 (remainder)
```

### Important Note on Division

Forth uses **integer division** by default. `22 7 /` gives `3`, not `3.142...`. The fractional part is discarded.

If you need both the quotient and remainder, use `/mod` which is more efficient than calling `/` and `mod` separately.

### More Arithmetic Words

| Word | Stack Effect | Description | Example |
|------|--------------|-------------|---------|
| `negate` | `( n -- -n )` | Negate a number | `5 negate .` � -5 |
| `abs` | `( n -- |n| )` | Absolute value | `-7 abs .` � 7 |
| `min` | `( n1 n2 -- min )` | Minimum of two numbers | `5 3 min .` � 3 |
| `max` | `( n1 n2 -- max )` | Maximum of two numbers | `5 3 max .` � 5 |
| `1+` | `( n -- n+1 )` | Add 1 | `5 1+ .` � 6 |
| `1-` | `( n -- n-1 )` | Subtract 1 | `5 1- .` � 4 |
| `2+` | `( n -- n+2 )` | Add 2 | `5 2+ .` � 7 |
| `2-` | `( n -- n-2 )` | Subtract 2 | `5 2- .` � 3 |
| `2*` | `( n -- n*2 )` | Multiply by 2 | `5 2* .` � 10 |
| `2/` | `( n -- n/2 )` | Divide by 2 | `10 2/ .` � 5 |

Examples:

```forth
\ Negate
42 negate .             \ -42

\ Absolute value
-15 abs .               \ 15

\ Min and max
100 250 min .           \ 100
100 250 max .           \ 250

\ Increment/decrement
10 1+ .                 \ 11
10 1- .                 \ 9

\ Quick multiplication/division by 2
16 2* .                 \ 32
16 2/ .                 \ 8
```

## Number Formats

### Decimal (Default)

By default, Forth interprets numbers as decimal:

```forth
42 .                    \ 42
100 .                   \ 100
```

### Hexadecimal

Use `hex` to switch to hexadecimal mode:

```forth
hex
10 .                    \ A
FF .                    \ FF
decimal                 \ Switch back to decimal
```

To print a number in hex without changing the mode:

```forth
255 hex . decimal       \ FF
```

Or use the `.` alternative for hex:

```forth
hex 255 . decimal       \ FF
```

### Binary

For binary, you can use the `%` prefix in some Forth systems, or use binary literals:

```forth
2 base !                \ Set base to binary
1010 .                  \ Shows as 1010 (which is 10 in decimal)
decimal                 \ Back to decimal
```

Or use the prefix notation:
```forth
%1010 .                 \ 10 (gforth supports % for binary)
```

### Octal

```forth
8 base !                \ Set base to octal
77 .                    \ Shows as 77
decimal                 \ Back to decimal
```

### Viewing the Current Base

```forth
base ? .                \ Shows current number base (usually 10)
```

## Complex Expressions

Remember, Forth evaluates left to right, and there's no operator precedence like in traditional languages.

### Example 1: `(2 + 3) * 4`

In traditional notation: `(2 + 3) * 4 = 20`

In Forth:
```forth
2 3 + 4 * .             \ 20
```

Step by step:
```
2       [2]
3       [2 3]
+       [5]
4       [5 4]
*       [20]
.       [] prints 20
```

### Example 2: `2 + 3 * 4`

In traditional notation with precedence: `2 + 3 * 4 = 14`

In Forth (no precedence, left to right):
```forth
2 3 + 4 * .             \ 20 (NOT 14!)
```

To get the traditional result, you need to rearrange:
```forth
3 4 * 2 + .             \ 14
```

### Example 3: `(10 - 2) / (3 + 1)`

```forth
10 2 - 3 1 + / .        \ 2
```

Step by step:
```
10      [10]
2       [10 2]
-       [8]
3       [8 3]
1       [8 3 1]
+       [8 4]
/       [2]
.       [] prints 2
```

## Practical Examples

### Temperature Conversion

Let's write a Forth script to convert Celsius to Fahrenheit.
Formula: F = (C * 9/5) + 32

Create `celsius-to-fahrenheit.fs`:

```forth
\ celsius-to-fahrenheit.fs
\ Convert Celsius to Fahrenheit

\ Let's convert 25�C
25 9 * 5 / 32 + .       \ 77

bye
```

Note: We do `25 * 9 / 5` rather than `25 * 9/5` because `9/5` would be integer division (1), giving the wrong result.

### Circle Area

Calculate area of a circle: A = �r�
(We'll approximate � as 314/100)

```forth
\ circle-area.fs
\ Calculate area of circle with radius 10

10              \ radius
dup *           \ square it (r�)
314 *           \ multiply by 314
100 /           \ divide by 100 (this gives us � H 3.14)
.               \ print result (314)

bye
```

We'll learn about `dup` in the next tutorial - it duplicates the top stack item.

## Common Mistakes

### 1. Wrong Order

```forth
\ WRONG: This doesn't make sense
+ 2 3               \ Error! + expects two numbers already on stack

\ RIGHT:
2 3 +
```

### 2. Not Enough Values on Stack

```forth
\ WRONG:
5 + .               \ Error! + needs two values

\ RIGHT:
5 3 + .
```

### 3. Forgetting Integer Division

```forth
\ This might surprise you:
10 3 / .            \ 3 (not 3.333...)
```

### 4. Order of Operands

```forth
\ Subtraction and division order matters:
10 3 - .            \ 7  (10 - 3)
3 10 - .            \ -7 (3 - 10)

20 4 / .            \ 5  (20 / 4)
4 20 / .            \ 0  (4 / 20 = 0 with integer division)
```

## Exercises

1. Calculate: `(7 + 8) * 3`
2. Calculate: `100 / (5 - 3)`
3. Calculate the remainder when 47 is divided by 5
4. Calculate both quotient and remainder of 47 � 5
5. Convert 100�F to Celsius: C = (F - 32) * 5/9
6. Find the absolute value of `-42`
7. Find the larger of 17 and 23
8. Calculate: `2� = 2 * 2 * 2`
9. What's the result of `15 3 /mod`? (Show both values)
10. Create a script that calculates the perimeter of a rectangle with width 5 and height 3

## Solutions

1.
```forth
7 8 + 3 * .         \ 45
```

2.
```forth
100 5 3 - / .       \ 50
```

3.
```forth
47 5 mod .          \ 2
```

4.
```forth
47 5 /mod .s        \ <2> 2 9
\ Or print both:
47 5 /mod . .       \ 9 2 (quotient then remainder)
```

5.
```forth
\ (100 - 32) * 5 / 9
100 32 - 5 * 9 / .  \ 37
```

6.
```forth
-42 abs .           \ 42
```

7.
```forth
17 23 max .         \ 23
```

8.
```forth
2 2 * 2 * .         \ 8
```

9.
```forth
15 3 /mod .s        \ <2> 0 5
\ remainder: 0, quotient: 5
```

10. Create `rectangle.fs`:
```forth
\ rectangle.fs
\ Calculate perimeter of rectangle: 2 * (width + height)

5 3 +               \ width + height = 8
2 *                 \ 2 * 8 = 16
." Perimeter: " . cr

bye
```

## Stack Discipline

In Forth, maintaining "stack discipline" is crucial. This means:

1. **Know what's on the stack** - Use `.s` frequently while learning
2. **Clean up after yourself** - Don't leave unexpected values on the stack
3. **Document stack effects** - When you define words (next tutorial), document what they expect and leave on the stack
4. **Use `.s` to debug** - When something goes wrong, check the stack!

## Quick Reference

### Arithmetic Operations
- `+` `-` `*` `/` `mod` `/mod` - Basic arithmetic
- `negate` `abs` - Unary operations
- `min` `max` - Comparison
- `1+` `1-` `2+` `2-` `2*` `2/` - Quick increment/decrement

### Stack Inspection
- `.s` - Show stack contents (non-destructive)
- `depth` - Get number of items on stack
- `.` - Print and remove top item

### Number Bases
- `decimal` - Switch to base 10
- `hex` - Switch to base 16
- `base !` - Set base to any value (e.g., `2 base !` for binary)

## What's Next?

In [Tutorial 2: Stack Manipulation](02-stack-manipulation.md), you'll learn the essential stack manipulation words:
- `dup` - Duplicate the top item
- `drop` - Remove the top item
- `swap` - Swap the top two items
- `over` - Copy the second item to the top
- `rot` - Rotate the top three items
- And more!

These are fundamental to writing Forth programs effectively.
