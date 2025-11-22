# Tutorial 2: Stack Manipulation

## Introduction

Stack manipulation words are essential tools in Forth programming. They let you rearrange, copy, and remove values on the stack. Mastering these words is key to writing clean, efficient Forth code.

In this tutorial, you'll learn the most important stack manipulation words and when to use them.

## The Basic Stack Words

### `dup` - Duplicate

**Stack effect:** `( n -- n n )`

Duplicates the top item on the stack.

```forth
5 .s            \ <1> 5
dup .s          \ <2> 5 5
```

**When to use:** When you need to use a value twice.

**Example:** Square a number (n * n)
```forth
7 dup * .       \ 49
```

Step by step:
```
7       [7]
dup     [7 7]
*       [49]
.       [] prints 49
```

### `drop` - Remove Top Item

**Stack effect:** `( n -- )`

Removes the top item from the stack.

```forth
1 2 3 .s        \ <3> 1 2 3
drop .s         \ <2> 1 2
```

**When to use:** When you have an unwanted value on the stack.

**Example:**
```forth
5 3 + dup .     \ Print 8
drop            \ Remove the duplicate
```

### `swap` - Swap Top Two Items

**Stack effect:** `( n1 n2 -- n2 n1 )`

Swaps the top two items on the stack.

```forth
10 20 .s        \ <2> 10 20
swap .s         \ <2> 20 10
```

**When to use:** When operands are in the wrong order.

**Example:** Reverse subtraction
```forth
\ We want 3 - 10 = -7
10 3 swap - .   \ -7
```

Or more directly:
```forth
3 10 - .        \ -7
```

### `over` - Copy Second Item

**Stack effect:** `( n1 n2 -- n1 n2 n1 )`

Copies the second item to the top of the stack.

```forth
10 20 .s        \ <2> 10 20
over .s         \ <3> 10 20 10
```

**When to use:** When you need to use the second item again.

**Example:** Calculate (a + b) and (a - b)
```forth
5 3             \ [5 3]
over over       \ [5 3 5 3]
+ .             \ 8
- .             \ 2
```

### `rot` - Rotate Top Three Items

**Stack effect:** `( n1 n2 n3 -- n2 n3 n1 )`

Rotates the top three items. The third item comes to the top.

```forth
1 2 3 .s        \ <3> 1 2 3
rot .s          \ <3> 2 3 1
```

Visualization:
```
Before:  n1 n2 n3  (n3 on top)
After:   n2 n3 n1  (n1 on top)
```

**Example:**
```forth
10 20 30 .s     \ <3> 10 20 30
rot .s          \ <3> 20 30 10
```

## Advanced Stack Words

### `nip` - Remove Second Item

**Stack effect:** `( n1 n2 -- n2 )`

Removes the second item, keeping the top.

```forth
10 20 .s        \ <2> 10 20
nip .s          \ <1> 20
```

Equivalent to: `swap drop`

### `tuck` - Copy Top Below Second

**Stack effect:** `( n1 n2 -- n2 n1 n2 )`

Copies the top item below the second item.

```forth
10 20 .s        \ <2> 10 20
tuck .s         \ <3> 20 10 20
```

Equivalent to: `swap over`

### `2dup` - Duplicate Top Two Items

**Stack effect:** `( n1 n2 -- n1 n2 n1 n2 )`

Duplicates the top two items.

```forth
10 20 .s        \ <2> 10 20
2dup .s         \ <4> 10 20 10 20
```

**Example:** Calculate both sum and product of two numbers
```forth
6 7             \ [6 7]
2dup + .        \ 13
* .             \ 42
```

### `2drop` - Drop Top Two Items

**Stack effect:** `( n1 n2 -- )`

Removes the top two items.

```forth
1 2 3 4 .s      \ <4> 1 2 3 4
2drop .s        \ <2> 1 2
```

### `2swap` - Swap Top Two Pairs

**Stack effect:** `( n1 n2 n3 n4 -- n3 n4 n1 n2 )`

Swaps the top two pairs of numbers.

```forth
1 2 3 4 .s      \ <4> 1 2 3 4
2swap .s        \ <4> 3 4 1 2
```

### `2over` - Copy Second Pair

**Stack effect:** `( n1 n2 n3 n4 -- n1 n2 n3 n4 n1 n2 )`

Copies the second pair to the top.

```forth
1 2 3 4 .s      \ <4> 1 2 3 4
2over .s        \ <6> 1 2 3 4 1 2
```

## The Return Stack

Forth has a second stack called the **return stack** used primarily for return addresses during function calls. However, you can temporarily store values there.

### `>r` - To Return Stack

**Stack effect:** `( n -- ) ( R: -- n )`

Moves a value from the data stack to the return stack.

### `r>` - From Return Stack

**Stack effect:** `( -- n ) ( R: n -- )`

Moves a value from the return stack to the data stack.

### `r@` - Copy from Return Stack

**Stack effect:** `( -- n ) ( R: n -- n )`

Copies the top of the return stack without removing it.

**Example:**
```forth
5 >r            \ Move 5 to return stack
3 4 +           \ Calculate 3 + 4 = 7
r> * .          \ Get 5 back, multiply: 7 * 5 = 35
```

**WARNING:** The return stack is used by Forth's control structures. Be very careful:
- Always `r>` what you `>r` in the same word definition
- Don't leave values on the return stack when exiting a word
- Don't use `>r` and `r>` across control structures

## Practical Examples

### Example 1: Calculate `a² + b²`

```forth
\ For a=3, b=4, we want 3² + 4² = 9 + 16 = 25
3 4             \ [3 4]
dup * swap      \ [9 4]  (calculated 3², swapped)
dup * +         \ [25]   (calculated 4², added)
.               \ 25
```

### Example 2: Average of Two Numbers

```forth
\ Average of 10 and 20 = (10 + 20) / 2 = 15
10 20           \ [10 20]
2dup + .        \ 30 (sum)
swap drop       \ [10 20] clean up
+ 2 / .         \ 15 (average)

\ Better way:
10 20
+ 2 / .         \ 15
```

### Example 3: Swap Without `swap`

You can implement swap using `rot`:
```forth
\ Put a dummy value, then rotate
5 10 0 rot drop     \ [10 5]
.s                  \ <2> 10 5

\ But just use swap!
5 10 swap .s        \ <2> 10 5
```

## Stack Visualization Practice

Let's trace through a complex sequence. Start with an empty stack:

```forth
3 4 5           \ [3 4 5]
over            \ [3 4 5 4]
*               \ [3 4 20]      (5 * 4)
swap            \ [3 20 4]
dup             \ [3 20 4 4]
rot             \ [20 4 4 3]
+ * +           \ Result?
```

Let's trace it:
```
3 4 5           [3 4 5]
over            [3 4 5 4]
*               [3 4 20]
swap            [3 20 4]
dup             [3 20 4 4]
rot             [20 4 4 3]
+               [20 4 7]        (4 + 3)
*               [20 28]         (4 * 7)
+               [48]            (20 + 28)
```

## Common Patterns

### Square a Number
```forth
dup *
```

### Cube a Number
```forth
dup dup * *
```

### Keep a Copy While Operating
```forth
\ Calculate (n * 2) but keep original n
dup 2 * .       \ Print n * 2
.               \ Print original n
```

### Drop an Unwanted Intermediate Result
```forth
5 3 + dup .     \ Print 8
drop            \ Remove the 8 from stack
```

## Debugging with `.s`

When learning stack manipulation, use `.s` after every step:

```forth
5 .s            \ <1> 5
3 .s            \ <2> 5 3
dup .s          \ <3> 5 3 3
* .s            \ <2> 5 9
+ .s            \ <1> 14
. .s            \ <0>  (prints 14)
```

## Common Mistakes

### 1. Stack Underflow

```forth
\ WRONG: Not enough values
drop            \ Error if stack is empty
```

### 2. Wrong Number of Values

```forth
\ WRONG: over needs at least 2 items
5 over          \ Error! Only one item on stack
```

### 3. Misunderstanding Stack Order

```forth
\ Which is which?
1 2 .s          \ <2> 1 2
\ 1 is the second item, 2 is the top
```

### 4. Return Stack Mismatch

```forth
\ WRONG: Don't do this!
5 >r
\ ... forget to r>
\ This will cause problems!
```

## Exercises

1. Starting with `3 4` on the stack, duplicate both numbers: `3 4 3 4`
2. Starting with `10 20 30` on the stack, get it to: `20 30 10`
3. Square the number 9 (result: 81)
4. Calculate `2³ = 2 * 2 * 2` using `dup`
5. Starting with `5 10`, calculate both the sum (15) and product (50)
6. Remove the second item from the stack containing `10 20 30`
7. Starting with `7 8`, arrange them as `7 8 7 8 7`
8. Calculate the average of 20 and 30 (result: 25)
9. Calculate `(a + b) * c` where a=2, b=3, c=4 (result: 20)
10. Implement this formula: `(x + y) / (x - y)` where x=10, y=6

## Solutions

1.
```forth
3 4 2dup .s     \ <4> 3 4 3 4
```

2.
```forth
10 20 30 rot .s \ <3> 20 30 10
```

3.
```forth
9 dup * .       \ 81
```

4.
```forth
2 dup dup * * . \ 8
```

5.
```forth
5 10 2dup + .   \ 15 (sum)
* .             \ 50 (product)
```

6.
```forth
10 20 30        \ [10 20 30]
swap drop .s    \ <2> 10 30
```

7.
```forth
7 8             \ [7 8]
2dup            \ [7 8 7 8]
drop over .s    \ <5> 7 8 7 8 7
```

8.
```forth
20 30 + 2 / .   \ 25
```

9.
```forth
2 3 + 4 * .     \ 20
```

10.
```forth
10 6            \ [10 6]
2dup - >r       \ [10 6], R: [4]  (10-6=4 on return stack)
+ r>            \ [16 4]           (10+6=16, retrieve 4)
/ .             \ 4                (16/4=4)

\ Or without return stack:
10 6 2dup + -rot - / .  \ More complex but avoids return stack
```

## Stack Effect Comments

When writing Forth code, it's good practice to document the stack effects of your operations:

```forth
\ Calculate area of circle ( radius -- area )
dup             \ ( radius -- radius radius )
*               \ ( radius radius -- radius² )
314 * 100 /     \ ( radius² -- area )
```

This helps you and others understand what the code expects and produces.

## Stack Discipline

Good Forth code follows "stack discipline":

1. **Predictable effects** - Each word should have a clear, documented stack effect
2. **No surprises** - Don't leave unexpected values on the stack
3. **Clean stack** - Start with a clean stack when testing
4. **Use `.s` often** - Check the stack frequently while debugging

## Quick Reference

### Basic Stack Manipulation
- `dup` `( n -- n n )` - Duplicate top
- `drop` `( n -- )` - Remove top
- `swap` `( n1 n2 -- n2 n1 )` - Swap top two
- `over` `( n1 n2 -- n1 n2 n1 )` - Copy second
- `rot` `( n1 n2 n3 -- n2 n3 n1 )` - Rotate three

### Advanced Stack Words
- `nip` `( n1 n2 -- n2 )` - Remove second
- `tuck` `( n1 n2 -- n2 n1 n2 )` - Copy top below second
- `2dup` `( n1 n2 -- n1 n2 n1 n2 )` - Duplicate two
- `2drop` `( n1 n2 -- )` - Drop two
- `2swap` `( n1 n2 n3 n4 -- n3 n4 n1 n2 )` - Swap pairs
- `2over` `( n1 n2 n3 n4 -- n1 n2 n3 n4 n1 n2 )` - Copy second pair

### Return Stack
- `>r` `( n -- ) ( R: -- n )` - To return stack
- `r>` `( -- n ) ( R: n -- )` - From return stack
- `r@` `( -- n ) ( R: n -- n )` - Copy from return stack

## What's Next?

Now that you can manipulate the stack effectively, you're ready to create your own words!

In [Tutorial 3: Defining Words](03-defining-words.md), you'll learn:
- How to define your own words with `:` and `;`
- Stack effect notation
- Building vocabularies
- Code organization
- Creating reusable functions

This is where Forth really starts to shine!
