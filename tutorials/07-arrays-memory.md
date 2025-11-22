# Tutorial 7: Arrays and Advanced Memory

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Create and manipulate arrays using `CREATE`, `ALLOT`, and `CELLS`
- 🎯 Implement multi-dimensional arrays with index calculations
- 🎯 Use `ALLOCATE` and `FREE` for dynamic memory management
- 🎯 Build custom data structures (records/structs)
- 🎯 Understand memory layout and byte vs. cell addressing
- 🎯 Compare Forth arrays to arrays in C and pointer arithmetic
- 🎯 Recognize how memory allocation relates to malloc/free in C

**Connection to other languages:** Forth's array indexing (`cells + @`) is identical to C's pointer arithmetic (`*(array + index)`). Understanding this reveals how high-level array syntax works under the hood in all languages.

## Introduction

Now that you understand variables, let's explore more advanced memory operations: arrays, buffers, memory allocation, and custom data structures.

## Arrays in Forth

Forth doesn't have built-in array syntax like other languages, but creating arrays is straightforward.

### Creating an Array with CREATE and ALLOT

```forth
create numbers 10 cells allot
```

This allocates space for 10 cells (numbers). A "cell" is one machine word (typically 4 or 8 bytes).

### Accessing Array Elements

```forth
: array!  ( value index array-addr -- )
    swap cells + !
;

: array@  ( index array-addr -- value )
    swap cells + @
;
```

### Complete Example

```forth
create numbers 5 cells allot

: set-number  ( value index -- )
    numbers array!
;

: get-number  ( index -- value )
    numbers array@
;

\ Initialize array
42 0 set-number
17 1 set-number
99 2 set-number

\ Read values
0 get-number .      \ 42
1 get-number .      \ 17
2 get-number .      \ 99
```

### Why `cells`?

The word `cells` converts an index to bytes:
- If cells are 8 bytes: `3 cells` � 24
- This gives the byte offset for the 3rd element

```forth
1 cells .           \ 8 (on 64-bit system)
2 cells .           \ 16
```

## Initializing Arrays

### Method 1: One by One

```forth
create numbers 5 cells allot

: init-numbers
    10 0 numbers array!
    20 1 numbers array!
    30 2 numbers array!
    40 3 numbers array!
    50 4 numbers array!
;
```

### Method 2: Using a Loop

```forth
: init-array  ( -- )
    5 0 DO
        I 10 * I numbers array!
    LOOP
;
```

### Method 3: Inline Data with `,`

```forth
create primes 2 , 3 , 5 , 7 , 11 , 13 , 17 , 19 ,
```

The `,` (comma) compiles a value into the dictionary at the current position.

Access with:
```forth
: get-prime  ( index -- value )
    cells primes + @
;

0 get-prime .       \ 2
4 get-prime .       \ 11
```

## Creating Array Abstractions

Make arrays easier to use by wrapping them:

```forth
\ Define an array type
: array  ( size -- )
    create cells allot
    does> ( index -- addr )
        swap cells +
;

\ Create arrays
10 array scores
5 array temperatures
```

Now use them:
```forth
100 0 scores !      \ Set scores[0] = 100
0 scores @ .        \ Get scores[0]

25 2 temperatures !
2 temperatures @ .
```

## Multidimensional Arrays

For 2D arrays, calculate the offset manually:

```forth
\ 3x3 matrix
create matrix 9 cells allot

\ Convert (row, col) to index
: matrix-index  ( row col -- index )
    swap 3 * +
;

\ Store in matrix
: matrix!  ( value row col -- )
    matrix-index cells matrix + !
;

\ Fetch from matrix
: matrix@  ( row col -- value )
    matrix-index cells matrix + @
;
```

Test it:
```forth
42 1 2 matrix!      \ matrix[1][2] = 42
1 2 matrix@ .       \ 42
```

## Character Arrays (Buffers)

For character data, use byte-sized storage:

```forth
create buffer 256 allot     \ 256 bytes

: buffer!  ( char index -- )
    buffer + c!
;

: buffer@  ( index -- char )
    buffer + c@
;
```

Note: `c!` and `c@` are character store and fetch (1 byte instead of 1 cell).

## HERE and ALLOT

`HERE` returns the address of the next free space in the dictionary:

```forth
here .              \ Shows current dictionary pointer
create space 100 allot
here .              \ Shows new pointer (100 bytes later)
```

### Dynamic Memory Allocation

```forth
variable my-buffer

: allocate-buffer  ( size -- addr )
    here swap allot
;

100 allocate-buffer my-buffer !
```

## Using BUFFER:

`BUFFER:` is like `CREATE...ALLOT` but specifically for buffers:

```forth
256 buffer: input-buffer
80 buffer: line-buffer
```

## Counted Strings

Forth uses "counted strings" - the first byte is the length:

```forth
create message 20 allot

: set-message  ( addr len -- )
    message c!          \ Store length
    message 1+ swap     \ Address after length byte
    move                \ Copy string
;
```

We'll cover strings more in Tutorial 8.

## Memory Operations

### MOVE: Copy Memory

**Stack effect:** `( src-addr dest-addr count -- )`

Copy `count` bytes from source to destination:

```forth
create source 10 allot
create dest 10 allot

\ Put some data in source
42 0 source c!
17 1 source c!

\ Copy to dest
source dest 2 move

\ Check dest
0 dest c@ .         \ 42
1 dest c@ .         \ 17
```

### FILL: Fill Memory

**Stack effect:** `( addr count char -- )`

Fill memory with a character:

```forth
create buffer 10 allot

\ Fill with zeros
buffer 10 0 fill

\ Fill with 'X'
buffer 10 char X fill
```

### ERASE: Zero Memory

**Stack effect:** `( addr count -- )`

Fill with zeros (equivalent to `0 fill`):

```forth
buffer 10 erase
```

## CREATE...DOES> for Custom Data Types

`CREATE...DOES>` is powerful for defining custom data structures.

### Example: Record Type

```forth
\ Define a 'person' type with age and score
: person  ( -- )
    create 2 cells allot
    does> ( -- addr )
;

\ Create persons
person alice
person bob

\ Accessors
: age!  ( n person-addr -- )
    !
;

: age@  ( person-addr -- n )
    @
;

: score!  ( n person-addr -- )
    cell+ !
;

: score@  ( person-addr -- n )
    cell+ @
;
```

Test it:
```forth
25 alice age!
100 alice score!

alice age@ .        \ 25
alice score@ .      \ 100
```

### Example: Array Factory

```forth
: array  ( size -- )
    create cells allot
    does> ( index -- addr )
        swap cells +
;

10 array my-numbers
5 array my-scores

42 0 my-numbers !
0 my-numbers @ .    \ 42
```

## Practical Examples

### Example 1: Statistics on an Array

```forth
10 array data

: fill-data
    10 0 DO
        I 10 * I data !
    LOOP
;

: sum-data  ( -- sum )
    0
    10 0 DO
        I data @ +
    LOOP
;

: average-data  ( -- avg )
    sum-data 10 /
;

: max-data  ( -- max )
    0 data @
    10 1 DO
        I data @ max
    LOOP
;
```

Test it:
```forth
fill-data
sum-data .          \ 450
average-data .      \ 45
max-data .          \ 90
```

### Example 2: Dynamic Stack

```forth
100 array stack-data
variable stack-top

: init-stack  ( -- )
    0 stack-top !
;

: push  ( n -- )
    stack-top @ stack-data !
    1 stack-top +!
;

: pop  ( -- n )
    -1 stack-top +!
    stack-top @ stack-data @
;

: stack-empty?  ( -- flag )
    stack-top @ 0=
;
```

### Example 3: Histogram

```forth
10 array histogram

: clear-histogram
    10 0 DO
        0 I histogram !
    LOOP
;

: record-value  ( n -- )
    dup 0 >= over 10 < and IF
        dup histogram @ 1 + swap histogram !
    ELSE
        drop
    THEN
;

: show-histogram
    10 0 DO
        I . ." : "
        I histogram @ 0 DO
            ." *"
        LOOP
        cr
    LOOP
;
```

Test it:
```forth
clear-histogram
3 record-value
3 record-value
5 record-value
3 record-value
show-histogram
```

### Example 4: Circular Buffer

```forth
10 constant BUFFER-SIZE
BUFFER-SIZE array circ-buffer
variable write-ptr
variable read-ptr
variable buffer-count

: init-buffer
    0 write-ptr !
    0 read-ptr !
    0 buffer-count !
;

: buffer-full?  ( -- flag )
    buffer-count @ BUFFER-SIZE =
;

: buffer-empty?  ( -- flag )
    buffer-count @ 0=
;

: write-buffer  ( n -- )
    buffer-full? IF
        drop ." Buffer full!" cr
    ELSE
        write-ptr @ circ-buffer !
        write-ptr @ 1 + BUFFER-SIZE mod write-ptr !
        1 buffer-count +!
    THEN
;

: read-buffer  ( -- n )
    buffer-empty? IF
        0 ." Buffer empty!" cr
    ELSE
        read-ptr @ circ-buffer @
        read-ptr @ 1 + BUFFER-SIZE mod read-ptr !
        -1 buffer-count +!
    THEN
;
```

## Memory Inspection

### DUMP: Display Memory

Show memory contents (gforth specific):

```forth
create test-data 1 , 2 , 3 , 4 , 5 ,
test-data 5 cells dump
```

This displays the memory in hexadecimal.

### Viewing Cell Contents

```forth
create data 10 , 20 , 30 ,

data @ .            \ 10
data cell+ @ .      \ 20
data 2 cells + @ .  \ 30
```

## Alignment

Sometimes you need to ensure data is aligned to cell boundaries:

```forth
here .              \ Maybe 12345
align               \ Round up to cell boundary
here .              \ Now 12352 (or similar)
```

Use `aligned` to align an address:

```forth
: aligned-allot  ( n -- addr )
    here swap allot
    here aligned
;
```

## Common Mistakes

### 1. Forgetting CELLS

```forth
\ WRONG: Using index directly
: broken  ( value index -- )
    numbers + ! ;       \ Only adds 1, 2, 3... bytes!

\ RIGHT: Convert to bytes
: correct  ( value index -- )
    cells numbers + ! ; \ Adds 8, 16, 24... bytes
```

### 2. Array Bounds

```forth
\ WRONG: No bounds checking
100 999 numbers array! \ Writes outside array!

\ BETTER: Add bounds checking
: safe-array!  ( value index -- )
    dup 0 10 within IF
        numbers array!
    ELSE
        drop drop ." Index out of bounds!" cr
    THEN
;
```

### 3. Uninitialized Memory

```forth
\ WRONG: Reading uninitialized memory
create data 10 cells allot
0 data @ .          \ Random garbage!

\ RIGHT: Initialize first
create data 10 cells allot
data 10 cells erase
0 data @ .          \ 0
```

## Quick Reference

### Creating Arrays
```forth
create name size cells allot    \ Allocate array
create name val1 , val2 , ... , \ Inline initialization
size buffer: name               \ Byte buffer
```

### Accessing Memory
```forth
cells           \ Convert index to bytes
@               \ Fetch cell
!               \ Store cell
c@              \ Fetch byte
c!              \ Store byte
2@              \ Fetch double
2!              \ Store double
```

### Memory Operations
```forth
move            \ Copy memory
fill            \ Fill with value
erase           \ Fill with zeros
here            \ Current dictionary pointer
allot           \ Allocate memory
align           \ Align to cell boundary
```

### CREATE...DOES>
```forth
: array-type
    create <initialization>
    does> <runtime behavior>
;
```

## Best Practices

1. **Always use `cells`** for cell-sized arrays
2. **Initialize arrays** before use
3. **Add bounds checking** for safety
4. **Use abstractions** - wrap arrays in convenient words
5. **Document sizes** - comment array dimensions
6. **Consider alignment** - use `align` for mixed data
7. **Zero memory** - use `erase` for clean state

## Exercises

1. Create a 5-element array and fill it with even numbers (2, 4, 6, 8, 10)
2. Write a word to find the minimum value in an array
3. Implement a word to reverse an array in place
4. Create a 3x3 matrix and implement matrix addition
5. Implement a FIFO queue using an array
6. Write a word to search for a value in an array
7. Create a word that sorts an array (bubble sort)
8. Implement a simple memory pool allocator
9. Create a bit array (storing multiple bits per cell)
10. Build a simple hash table

## Solutions

1.
```forth
5 array evens

: fill-evens
    5 0 DO
        I 2 + 2 * I evens !
    LOOP
;
```

2.
```forth
10 array nums

: array-min  ( array size -- min )
    over @ -rot          \ Start with first element
    1 DO
        over I cells + @
        min
    LOOP
    swap drop
;
```

3.
```forth
: reverse-array  ( array size -- )
    0 swap 1 - DO
        over I cells + @
        over J cells + @
        over I cells + !
        over J cells + !
    LOOP
    drop
;
```

4.
```forth
create mat-a 9 cells allot
create mat-b 9 cells allot
create mat-result 9 cells allot

: mat-add  ( -- )
    9 0 DO
        I cells mat-a + @
        I cells mat-b + @
        + I cells mat-result + !
    LOOP
;
```

5.
```forth
10 array queue
variable head
variable tail

: init-queue
    0 head ! 0 tail ! ;

: enqueue  ( n -- )
    tail @ queue !
    tail @ 1 + 10 mod tail ! ;

: dequeue  ( -- n )
    head @ queue @
    head @ 1 + 10 mod head ! ;
```

6.
```forth
: array-find  ( value array size -- index|-1 )
    0 DO
        2dup I cells + @ = IF
            2drop I LEAVE
        THEN
    LOOP
    \ If not found, return -1
    dup I = IF drop -1 THEN
;
```

## What's Next?

In [Tutorial 8: Strings and Character Operations](08-strings.md), you'll learn:
- String handling in Forth
- Counted strings vs address/length pairs
- String operations and manipulation
- Printing and formatting
- Character classification
- Parsing input

Strings bring together everything you've learned about memory and arrays!
