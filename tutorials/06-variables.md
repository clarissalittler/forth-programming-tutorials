# Tutorial 6: Variables and Memory

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Create and use variables with `VARIABLE`, `!` (store), and `@` (fetch)
- 🎯 Understand the difference between values and addresses in memory
- 🎯 Use `VALUE` and `TO` for more convenient variable access
- 🎯 Perform direct memory operations with `C@` and `C!` for byte access
- 🎯 Recognize how Forth's memory model relates to pointers in C
- 🎯 Understand when to use the stack vs. when to use variables
- 🎯 Compare Forth's memory operations to variables in high-level languages

**Connection to other languages:** Forth's `!` and `@` are exactly like C's pointer dereference (`*ptr` and `*ptr = value`). Understanding this prepares you for systems programming and helps demystify pointers.

## Introduction

So far, we've worked primarily with the stack. But sometimes you need to store values in memory for later use. Forth provides variables, constants, and direct memory access.

## Constants

We've already seen constants. They're values that never change.

### Creating Constants

```forth
100 constant MAX-SIZE
60 constant SECONDS-PER-MINUTE
3.14159 constant PI
```

### Using Constants

Constants push their value onto the stack when invoked:

```forth
MAX-SIZE .          \ 100
SECONDS-PER-MINUTE 60 * .   \ 3600 (seconds per hour)
```

## Variables

Variables are memory locations that can be changed.

### Creating Variables

```forth
variable counter
variable total
variable user-age
```

This allocates memory for each variable.

### Storing Values: `!` (Store)

**Stack effect:** `( n addr -- )`

Store a value at a memory address:

```forth
42 counter !        \ Store 42 in counter
100 total !         \ Store 100 in total
25 user-age !       \ Store 25 in user-age
```

### Fetching Values: `@` (Fetch)

**Stack effect:** `( addr -- n )`

Retrieve a value from a memory address:

```forth
counter @           \ Push value of counter onto stack
.                   \ Print it
```

### Complete Example

```forth
variable score

: set-score  ( n -- )
    score !
;

: show-score  ( -- )
    ." Score: " score @ . cr
;

: add-points  ( n -- )
    score @ + score !
;
```

Test it:
```forth
100 set-score
show-score          \ Score: 100
50 add-points
show-score          \ Score: 150
```

## The Difference: Stack vs Variables

### Stack Approach (Functional)

```forth
: double  ( n -- 2n )
    2 *
;

5 double .          \ 10
```

### Variable Approach (Stateful)

```forth
variable number

: set-number  ( n -- )
    number !
;

: double-number  ( -- )
    number @ 2 * number !
;

: show-number  ( -- )
    number @ .
;

5 set-number
double-number
show-number         \ 10
```

**Generally prefer the stack approach** - it's cleaner and more "Forth-like". Use variables when you need persistent state.

## Incrementing Variables

Common pattern for counters:

```forth
variable counter

: reset-counter  ( -- )
    0 counter !
;

: increment-counter  ( -- )
    counter @ 1 + counter !
;

: show-counter  ( -- )
    ." Counter: " counter @ . cr
;
```

Test it:
```forth
reset-counter
increment-counter
increment-counter
show-counter        \ Counter: 2
```

### Better Way: Using `+!`

**Stack effect:** `( n addr -- )`

Add a value to what's stored at an address:

```forth
: increment-counter  ( -- )
    1 counter +!
;

: add-to-counter  ( n -- )
    counter +!
;
```

Test it:
```forth
0 counter !
5 add-to-counter
10 add-to-counter
counter @ .         \ 15
```

## VALUE: A Convenient Alternative

`VALUE` creates a named value that's easier to read and modify than variables.

### Creating Values

```forth
100 value health
50 value mana
```

### Reading Values

Just use the name (like constants):

```forth
health .            \ 100
mana .              \ 50
```

### Modifying Values: `TO`

```forth
200 to health
health .            \ 200
```

### Complete Example

```forth
100 value player-health

: take-damage  ( amount -- )
    player-health swap - to player-health
;

: heal  ( amount -- )
    player-health + to player-health
;

: show-health  ( -- )
    ." Health: " player-health . cr
;
```

Test it:
```forth
show-health         \ Health: 100
30 take-damage
show-health         \ Health: 70
20 heal
show-health         \ Health: 90
```

## Multiple Variables

You can create and use multiple variables:

```forth
variable x
variable y
variable z

: set-point  ( x y z -- )
    z ! y ! x !
;

: show-point  ( -- )
    ." (" x @ . ." , " y @ . ." , " z @ . ." )" cr
;

: distance-from-origin  ( -- distance )
    x @ dup * y @ dup * + z @ dup * +
    \ This gives us x�+y�+z� (not perfect, but demonstrates concept)
;
```

Test it:
```forth
3 4 5 set-point
show-point          \ (3, 4, 5)
```

## The `?` Operator

`?` is a convenience word that fetches and prints a variable's value:

**Stack effect:** `( addr -- )`

```forth
variable count
42 count !
count ?             \ 42
```

It's equivalent to:
```forth
count @ .
```

## Memory Addresses

Variables return their memory address when invoked:

```forth
variable test
test .              \ Prints an address (e.g., 140737488347104)
test @ .            \ Prints the value stored there
```

This lets you pass addresses to words:

```forth
: set-value  ( n addr -- )
    !
;

: get-value  ( addr -- n )
    @
;

variable storage
42 storage set-value
storage get-value .     \ 42
```

## 2VARIABLE for Pairs

`2VARIABLE` creates storage for two values (double-cell):

```forth
2variable coordinates

: set-xy  ( x y -- )
    coordinates 2!
;

: get-xy  ( -- x y )
    coordinates 2@
;

10 20 set-xy
get-xy .s           \ <2> 10 20
```

## ALLOT: Allocating Memory

`ALLOT` allocates raw memory space.

### Creating an Array with ALLOT

```forth
create my-array 10 cells allot

\ Store value in array
: array!  ( n index -- )
    cells my-array + !
;

\ Fetch value from array
: array@  ( index -- n )
    cells my-array + @
;
```

Test it:
```forth
42 0 array!         \ Store 42 at index 0
99 5 array!         \ Store 99 at index 5
0 array@ .          \ 42
5 array@ .          \ 99
```

**Note:** `cells` converts an index to bytes (1 cell = 1 machine word, usually 4 or 8 bytes).

## CREATE...DOES>: Advanced Memory

`CREATE...DOES>` lets you define custom data structures. This is advanced, but here's a taste:

### Example: Named Values Array

```forth
: array  ( size -- )
    create cells allot
    does> swap cells +
;

10 array my-numbers

: set-number  ( n index -- )
    my-numbers !
;

: get-number  ( index -- n )
    my-numbers @
;
```

Test it:
```forth
42 0 set-number
99 5 set-number
0 get-number .      \ 42
5 get-number .      \ 99
```

## Practical Examples

### Example 1: Bank Account

```forth
variable balance

: deposit  ( amount -- )
    balance +!
;

: withdraw  ( amount -- )
    negate balance +!
;

: show-balance  ( -- )
    ." Balance: $" balance @ . cr
;

: can-withdraw?  ( amount -- flag )
    balance @ >=
;
```

Test it:
```forth
0 balance !
100 deposit
show-balance        \ Balance: $100
30 withdraw
show-balance        \ Balance: $70
```

### Example 2: Statistics Accumulator

```forth
variable count
variable sum

: reset-stats  ( -- )
    0 count !
    0 sum !
;

: add-value  ( n -- )
    sum +!
    1 count +!
;

: average  ( -- avg )
    count @ 0= IF
        0
    ELSE
        sum @ count @ /
    THEN
;

: show-stats  ( -- )
    ." Count: " count @ . cr
    ." Sum: " sum @ . cr
    ." Average: " average . cr
;
```

Test it:
```forth
reset-stats
10 add-value
20 add-value
30 add-value
show-stats
\ Count: 3
\ Sum: 60
\ Average: 20
```

### Example 3: Game State

```forth
100 value player-health
0 value score
1 value level

: player-alive?  ( -- flag )
    player-health 0 >
;

: add-score  ( points -- )
    score + to score
;

: level-up  ( -- )
    level 1 + to level
    ." Level up! Now level " level . cr
;

: show-status  ( -- )
    ." === Player Status ===" cr
    ." Health: " player-health . cr
    ." Score: " score . cr
    ." Level: " level . cr
;
```

### Example 4: Simple Stack Implementation

```forth
create stack 100 cells allot
variable stack-ptr

: init-stack  ( -- )
    0 stack-ptr !
;

: push  ( n -- )
    stack-ptr @ cells stack + !
    1 stack-ptr +!
;

: pop  ( -- n )
    -1 stack-ptr +!
    stack-ptr @ cells stack + @
;

: stack-depth  ( -- n )
    stack-ptr @
;
```

Test it:
```forth
init-stack
10 push
20 push
30 push
stack-depth .       \ 3
pop .               \ 30
pop .               \ 20
stack-depth .       \ 1
```

## Common Patterns

### Toggle a Boolean

```forth
variable debug-mode

: toggle-debug  ( -- )
    debug-mode @ 0= debug-mode !
;
```

### Clamp a Variable to Range

```forth
variable temperature

: set-temp  ( n -- )
    0 max 100 min temperature !
;
```

### Swap Two Variables

```forth
variable a
variable b

: swap-vars  ( -- )
    a @ b @ a ! b !
;
```

## Common Mistakes

### 1. Forgetting @ or !

```forth
\ WRONG: Pushes address, not value
variable count
count .             \ Prints address

\ RIGHT:
count @ .           \ Prints value
```

### 2. Wrong Order for !

```forth
\ WRONG: Order matters!
counter 42 !        \ Error! Expects ( value addr -- )

\ RIGHT:
42 counter !        \ Correct order
```

### 3. Not Initializing

```forth
\ WRONG: Uninitialized variable has random value
variable total
total @ .           \ Unpredictable!

\ RIGHT: Initialize first
0 total !
total @ .           \ 0
```

## Quick Reference

### Variables
```forth
\ Create
variable name
2variable name

\ Store
n addr !            \ Store n at addr
n addr +!           \ Add n to value at addr
n1 n2 addr 2!       \ Store double

\ Fetch
addr @              \ Fetch value at addr
addr 2@             \ Fetch double
addr ?              \ Fetch and print
```

### Values
```forth
\ Create
n value name

\ Read
name                \ Pushes value

\ Modify
n to name           \ Set new value
```

### Constants
```forth
\ Create
n constant name

\ Read
name                \ Pushes value (cannot modify)
```

### Memory
```forth
create name n allot     \ Allocate n bytes
cells                   \ Convert index to bytes
```

## Best Practices

1. **Use constants for fixed values** - More efficient than variables
2. **Prefer VALUES over VARIABLE** - Cleaner syntax with `TO`
3. **Initialize variables** - Don't rely on initial values
4. **Prefer stack over variables** - More functional, easier to test
5. **Use meaningful names** - `player-health` not `ph`
6. **Document state** - Comment what each variable represents

## Exercises

1. Create a variable `counter` and words to increment, decrement, and reset it
2. Create two variables `x` and `y` and a word to swap their values
3. Create a `value` for temperature with words to increase/decrease by degrees
4. Implement a simple scoreboard with variables for player1 and player2 scores
5. Create an array of 5 elements and words to set/get values
6. Implement a min/max tracker that updates when new values are added
7. Create a word that counts how many times it's been called
8. Implement a simple average calculator using variables
9. Create a "health bar" system with max-health and current-health
10. Build a simple random number generator using a variable as a seed

## Solutions

1.
```forth
variable counter

: inc-counter  ( -- )
    1 counter +! ;

: dec-counter  ( -- )
    -1 counter +! ;

: reset-counter  ( -- )
    0 counter ! ;
```

2.
```forth
variable x
variable y

: swap-xy  ( -- )
    x @ y @ x ! y ! ;
```

3.
```forth
20 value temperature

: warmer  ( degrees -- )
    temperature + to temperature ;

: cooler  ( degrees -- )
    temperature swap - to temperature ;
```

4.
```forth
variable p1-score
variable p2-score

: reset-game  ( -- )
    0 p1-score ! 0 p2-score ! ;

: p1-scores  ( points -- )
    p1-score +! ;

: p2-scores  ( points -- )
    p2-score +! ;

: show-scores  ( -- )
    ." P1: " p1-score @ .
    ." P2: " p2-score @ . cr ;
```

5.
```forth
create arr 5 cells allot

: arr!  ( n index -- )
    cells arr + ! ;

: arr@  ( index -- n )
    cells arr + @ ;
```

6.
```forth
variable current-min
variable current-max
variable initialized

: reset-tracker  ( -- )
    0 initialized ! ;

: track-value  ( n -- )
    initialized @ 0= IF
        dup current-min !
        dup current-max !
        -1 initialized !
    ELSE
        dup current-min @ min current-min !
        dup current-max @ max current-max !
    THEN
    drop ;

: show-range  ( -- )
    ." Min: " current-min @ .
    ." Max: " current-max @ . cr ;
```

7.
```forth
variable call-count

: counted-word  ( -- )
    1 call-count +!
    ." This word has been called "
    call-count @ . ." times" cr ;
```

8.
```forth
variable total
variable count

: reset  ( -- )
    0 total ! 0 count ! ;

: add-number  ( n -- )
    total +! 1 count +! ;

: average  ( -- avg )
    count @ 0= IF 0 ELSE total @ count @ / THEN ;
```

9.
```forth
100 value max-health
100 value current-health

: take-damage  ( n -- )
    current-health swap - 0 max to current-health ;

: heal  ( n -- )
    current-health + max-health min to current-health ;

: show-health  ( -- )
    ." HP: " current-health . ." / " max-health . cr ;
```

10.
```forth
12345 value seed

: random  ( -- n )
    seed 1103515245 * 12345 + to seed
    seed abs ;
```

## What's Next?

In [Tutorial 7: Arrays and Advanced Memory](07-arrays-memory.md), you'll learn:
- Creating and manipulating arrays
- Memory management
- Buffers and strings
- The dictionary
- Creating custom data structures

Get ready for more advanced memory operations!
