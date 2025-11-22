# Tutorial 9: Memory Architecture & The Dictionary

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Understand Forth's memory model: dictionary, data stack, and return stack
- 🎯 Use `HERE`, `ALLOT`, and `,` to manually allocate and compile data
- 🎯 Explore the dictionary structure and how words are stored
- 🎯 Create defining words with `CREATE...DOES>`
- 🎯 Understand the difference between compile-time and run-time behavior
- 🎯 Recognize how Forth's dictionary relates to symbol tables in compilers
- 🎯 Compare Forth's memory model to the heap, stack, and code segments in C

**Connection to other languages:** Understanding the dictionary reveals how compilers work. This knowledge transfers directly to understanding linkers, loaders, symbol tables, and how executable files are structured in any language.

## Introduction

So far, we've used Forth's memory without understanding what's happening under the hood. In this tutorial, we'll explore Forth's memory architecture - particularly the **dictionary**, where all code and data definitions are stored.

This is where Forth becomes a systems programming language. You'll see how Forth blurs the line between compile-time and run-time, and how you can manipulate the language itself.

## Forth's Memory Model

Forth uses several separate memory areas:

```
┌─────────────────────────────────┐
│        Dictionary               │  ← Code & data definitions
│  (grows upward →)              │
├─────────────────────────────────┤
│        Data Stack               │  ← Your working stack
│  (grows downward ←)            │
├─────────────────────────────────┤
│        Return Stack             │  ← Function call returns
│  (grows downward ←)            │
└─────────────────────────────────┘
```

### The Data Stack
This is what you've been using: temporary values during computation.

### The Return Stack
Stores return addresses for word calls (like the call stack in C). You can access it with `>R` (to return stack) and `R>` (from return stack), but use carefully!

### The Dictionary
This is where **everything** lives:
- Word definitions (your code)
- Variables and their storage
- Compiled literals
- Array allocations

The dictionary is like the combination of a compiler's symbol table, the code segment, and the initialized data segment in a C program.

## The Dictionary Pointer: HERE

`HERE` returns the address of the next available dictionary location.

```forth
here .              \ e.g., 140735828523008 (some address)
```

Think of `HERE` as the "allocation pointer" for the dictionary. Everything you define moves `HERE` forward.

### Watching HERE Move

```forth
: show-here  ( -- )
    ." Dictionary pointer: " here . cr
;

show-here           \ e.g., Dictionary pointer: 140735828523008
variable test       \ Allocates one cell
show-here           \ e.g., Dictionary pointer: 140735828523016 (moved by 8 bytes)
```

## Allocating Dictionary Space: ALLOT

`ALLOT` reserves space in the dictionary.

**Stack effect:** `( n -- )`

```forth
here .              \ Note the address
100 allot           \ Reserve 100 bytes
here .              \ Address is now 100 bytes higher
```

This is how `VARIABLE` and `CREATE` work internally!

### Example: Manual Variable Creation

Instead of:
```forth
variable counter
```

You could do:
```forth
create counter 1 cells allot
```

This:
1. Creates a dictionary entry named "counter"
2. Reserves space for one cell (8 bytes on 64-bit systems)

## Compiling Values: The Comma `,`

The `,` (comma) operator compiles a cell-sized value into the dictionary at `HERE`, then advances `HERE`.

**Stack effect:** `( n -- )`

```forth
create primes
2 , 3 , 5 , 7 , 11 , 13 ,

\ Access the primes
primes @ .          \ 2
primes cell+ @ .    \ 3
primes 2 cells + @ .  \ 5
```

### What Just Happened?

1. `create primes` - Creates a dictionary entry and returns its address when called
2. `2 ,` - Compiles the value 2 at the current dictionary position
3. `3 ,` - Compiles 3 right after 2
4. And so on...

Result: A contiguous array in the dictionary!

### Byte-Level Compilation: C,

`C,` compiles a single byte (character):

```forth
create message
  char H c, char e c, char l c, char l c, char o c, 0 c,

\ Print as null-terminated string (C-style)
message dup
begin
    dup c@ dup          \ ( addr char char )
while
    emit                \ Print character
    1+                  \ Move to next byte
repeat
2drop cr                \ Clean up stack
```

This creates a C-style null-terminated string!

## Understanding Word Structure

When you define a word, Forth creates a dictionary entry:

```forth
: square  ( n -- n² )
    dup *
;
```

The dictionary entry contains:
1. **Link field** - Pointer to previous word (creating a linked list)
2. **Name field** - The word's name ("square")
3. **Code field** - Execution behavior (native code or pointer to interpreter)
4. **Parameter field** - The compiled definition (`dup *`)

You can inspect compiled words with `SEE`:

```forth
see square
```

This shows you the internal structure!

## CREATE...DOES> - Defining Defining Words

This is where Forth gets meta: you can create words that create words!

### Example: A Better Array Definition

```forth
: array  ( n "name" -- )
    create cells allot
    does> ( index -- addr )
        swap cells +
;

\ Now use it to create arrays:
10 array scores
5 array temperatures

\ Use them:
100 0 scores !      \ scores[0] = 100
95 1 scores !       \ scores[1] = 95
0 scores @ .        \ 100
1 scores @ .        \ 95
```

**How it works:**

1. **Compile time** (when you run `10 array scores`):
   - `create` makes a new word "scores"
   - `cells allot` allocates 10 cells of space

2. **Run time** (when you use `0 scores`):
   - `does>` specifies what happens: takes index, converts to byte offset, adds to base address

### Example: Constant-like Definitions

```forth
: constant  ( n "name" -- )
    create ,
    does> ( -- n )
        @
;

42 constant answer
answer .            \ 42
```

This is how Forth's own `CONSTANT` works!

## Compile-Time vs Run-Time

Understanding when code executes is crucial:

```forth
: test1
    ." Compiling test1" cr      \ Prints when DEFINING test1
;

: test2
    ['] test1                    \ Compile reference to test1
;
```

Some words are **immediate** - they execute during compilation, not at runtime.

### Example: Understanding [

When you use `[` in a definition, it switches from compilation mode to interpretation mode:

```forth
: test3
    [ 2 3 + ] literal           \ Calculate 2+3 at compile time
    .                           \ Print result at run time
;

test3                           \ 5
```

The `[ 2 3 + ]` runs when you **define** test3, not when you call it!

## Memory Inspection

View memory contents directly:

```forth
create data  10 , 20 , 30 , 40 ,

\ Dump memory
: dump-cell  ( addr -- )
    dup . ." : " @ . cr
;

data dump-cell          \ address : 10
data cell+ dump-cell    \ address : 20
```

### Hex Dump Example

```forth
: hdump  ( addr len -- )
    hex
    0 do
        dup i + c@
        dup 16 < if ." 0" then
        .
    loop
    drop
    decimal
;

\ Dump 16 bytes of memory
create bytes  1 c, 2 c, 3 c, 4 c, 5 c, 6 c, 7 c, 8 c,
bytes 8 hdump           \ 01 02 03 04 05 06 07 08
```

## Practical Applications

### Custom Data Structures

```forth
\ Define a point structure
: point  ( x y "name" -- )
    create swap , ,    \ Swap so x is compiled first
    does> ( -- addr )
;

10 20 point p1

\ Access components
: point-x  ( point -- x )  @ ;
: point-y  ( point -- y )  cell+ @ ;

p1 point-x .        \ 10
p1 point-y .        \ 20
```

### Building Your Own Control Structures

Advanced: You can even create your own IF/THEN!

```forth
\ Simplified IF (actual implementation is more complex)
: my-if  ( flag -- )
    0= if postpone else then
; immediate
```

## Comparison to C

| Forth Concept | C Equivalent | Description |
|---------------|--------------|-------------|
| Dictionary | Symbol table + code segment | Where definitions live |
| `HERE` | `malloc` / `sbrk` | Allocation pointer |
| `ALLOT` | `malloc` (static) | Reserve memory |
| `,` | Store to array | Compile value |
| Data Stack | CPU registers | Fast temporary storage |
| Return Stack | Call stack | Function returns |
| `CREATE...DOES>` | Macros + function pointers | Meta-programming |

## Common Mistakes

### 1. Forgetting Dictionary is Permanent

```forth
\ DON'T DO THIS:
: waste-memory
    100 allot       \ This permanently reserves space!
;
```

Each call allocates more space. You'll run out eventually!

### 2. Mixing Cells and Bytes

```forth
create array 10 allot       \ 10 bytes
5 0 array !                 \ Might work, might not!

create array2 10 cells allot  \ 10 cells (80 bytes on 64-bit)
5 0 array2 !                  \ Safe!
```

### 3. Using , Outside Definitions

```forth
\ This works but is dangerous:
42 ,                \ Compiles into dictionary randomly
```

Always use `,` within `CREATE` or controlled contexts.

## Exercises

1. **Memory Explorer**: Write a word that shows how much the dictionary grows when you define a variable vs. a constant.

2. **Custom Array**: Create a defining word `matrix` that creates 2D arrays:
   ```forth
   3 4 matrix board    \ 3 rows, 4 columns
   ```

3. **String Pool**: Create a string storage system that allocates strings in the dictionary and returns handles.

4. **Memory Usage**: Write a word that reports total dictionary space used.

5. **Linked List**: Implement a simple linked list using dictionary allocation.

## Solutions

### 1. Memory Explorer

```forth
: measure-growth  ( -- )
    here
    variable test
    here swap -
    ." Variable allocates: " . ." bytes" cr

    here
    42 constant answer
    here swap -
    ." Constant allocates: " . ." bytes" cr
;
```

### 2. Custom 2D Array

```forth
: matrix  ( rows cols "name" -- )
    create
        over , dup ,        \ Store dimensions
        * cells allot       \ Allocate rows*cols cells
    does> ( row col -- addr )
        dup >r              \ Save base address
        2 cells + @         \ Get columns
        rot * +             \ row*cols + col
        cells r> 2 cells + +  \ Convert to address
;

3 4 matrix board
100 0 1 board !     \ board[0][1] = 100
```

### 3. String Pool

```forth
variable string-count

: $create  ( addr len "name" -- )
    create
        here over 1+ allot  \ Reserve len+1 bytes
        dup c!              \ Store length as first byte
        1+ swap cmove       \ Copy string
    does> ( -- addr len )
        dup 1+ swap c@      \ Return addr+1 and length
;

s" Hello" $create greeting
greeting type cr    \ Hello
```

## Quick Reference

| Word | Stack Effect | Description |
|------|--------------|-------------|
| `here` | `( -- addr )` | Current dictionary pointer |
| `allot` | `( n -- )` | Reserve n bytes |
| `,` | `( n -- )` | Compile cell value |
| `c,` | `( c -- )` | Compile byte value |
| `create` | `( "name" -- )` | Create dictionary entry |
| `does>` | Runtime: `( -- addr )` | Define runtime behavior |
| `>r` | `( n -- ) ( R: -- n )` | Move to return stack |
| `r>` | `( -- n ) ( R: n -- )` | Move from return stack |

## What's Next?

Now that you understand how Forth manages memory at a low level, you're ready for advanced topics!

In the next tutorials (to be created), you'll learn:
- **Tutorial 10: Bit Manipulation** - Low-level binary operations
- **Tutorial 11: File I/O** - Reading and writing files
- **Tutorial 12: Advanced Topics** - Compilation, optimization, and more

Understanding the dictionary is the key to mastering Forth and thinking like a systems programmer. You now know what happens "under the hood" - knowledge that applies to any programming language!
