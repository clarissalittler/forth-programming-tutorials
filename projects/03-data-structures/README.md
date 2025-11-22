# Project 3: Data Structures Library

## Difficulty: 🔴 Advanced

## Prerequisites
- Tutorials 0-9 (All tutorials including Memory & Dictionary)
- Strong understanding of memory management
- Experience with CREATE...DOES>
- Algorithm design knowledge

## Overview

Implement fundamental data structures from scratch in Forth. This project deepens your understanding of memory management, pointers, and algorithmic thinking while building reusable components.

## Learning Goals

- Master dynamic memory allocation
- Understand pointer-based data structures
- Implement classic algorithms (insertion, deletion, search)
- Design clean, reusable APIs
- Analyze time and space complexity
- Think about memory safety and error handling

## Specification

### Required Data Structures

Implement at least THREE of the following:

1. **Dynamic Array** (growable)
2. **Linked List** (singly-linked)
3. **Stack** (LIFO)
4. **Queue** (FIFO)
5. **Hash Table** (bonus)
6. **Binary Search Tree** (bonus)

### Common Requirements

Each data structure must support:

- **Creation**: Initialize a new instance
- **Insertion**: Add elements
- **Deletion**: Remove elements
- **Search**: Find elements (where applicable)
- **Iteration**: Visit all elements
- **Destruction**: Free all memory
- **Size**: Return element count
- **Clear**: Remove all elements (without freeing structure)

### Testing Requirements

For each structure, test:
- Empty structure behavior
- Single element operations
- Large dataset (100+ elements)
- Edge cases (full, empty, not found)
- Memory cleanup (no leaks)

## Data Structure Specifications

### 1. Dynamic Array

A growable array that automatically resizes.

**Operations:**
```forth
da-create     ( initial-size -- da )  \ Create array
da-append     ( value da -- )         \ Add to end
da-get        ( index da -- value )   \ Get by index
da-set        ( value index da -- )   \ Set by index
da-size       ( da -- size )          \ Get length
da-capacity   ( da -- cap )           \ Get capacity
da-clear      ( da -- )               \ Remove all
da-free       ( da -- )               \ Free memory
```

**Example:**
```forth
10 da-create constant my-array

42 my-array da-append
17 my-array da-append
99 my-array da-append

my-array da-size .      \ 3

1 my-array da-get .     \ 17

100 1 my-array da-set
1 my-array da-get .     \ 100

my-array da-free
```

**Hints:**
- Start with fixed capacity
- When full, allocate 2x space and copy
- Track both size (used) and capacity (allocated)

### 2. Linked List

A singly-linked list with head pointer.

**Operations:**
```forth
list-create   ( -- list )           \ Create empty list
list-append   ( value list -- )     \ Add to end
list-prepend  ( value list -- )     \ Add to front
list-insert   ( value index list -- ) \ Insert at position
list-remove   ( index list -- value ) \ Remove at position
list-get      ( index list -- value ) \ Get at position
list-find     ( value list -- index | -1 ) \ Search
list-size     ( list -- size )      \ Get length
list-free     ( list -- )           \ Free all nodes
```

**Node structure:**
```forth
: node  ( -- addr )
    create
        1 cells ,   \ data
        1 cells ,   \ next pointer
;
```

### 3. Stack (LIFO)

Last-In-First-Out data structure.

**Operations:**
```forth
stack-create  ( -- stack )        \ Create stack
stack-push    ( value stack -- )  \ Push value
stack-pop     ( stack -- value )  \ Pop value (error if empty)
stack-peek    ( stack -- value )  \ Look at top (non-destructive)
stack-empty?  ( stack -- flag )   \ Check if empty
stack-size    ( stack -- size )   \ Get size
stack-free    ( stack -- )        \ Free memory
```

**Example:**
```forth
stack-create constant s

10 s stack-push
20 s stack-push
30 s stack-push

s stack-peek .      \ 30 (still on stack)
s stack-pop .       \ 30
s stack-pop .       \ 20
s stack-size .      \ 1
s stack-pop .       \ 10
s stack-empty? .    \ -1 (true)
```

### 4. Queue (FIFO)

First-In-First-Out data structure.

**Operations:**
```forth
queue-create  ( -- queue )          \ Create queue
queue-enqueue ( value queue -- )    \ Add to back
queue-dequeue ( queue -- value )    \ Remove from front
queue-peek    ( queue -- value )    \ Look at front
queue-empty?  ( queue -- flag )     \ Check if empty
queue-size    ( queue -- size )     \ Get size
queue-free    ( queue -- )          \ Free memory
```

### 5. Hash Table (Bonus)

Key-value store with O(1) average access.

**Operations:**
```forth
ht-create     ( size -- ht )        \ Create with capacity
ht-insert     ( value key ht -- )   \ Insert key-value pair
ht-get        ( key ht -- value | -1 ) \ Get value (or -1)
ht-remove     ( key ht -- flag )    \ Remove entry
ht-contains?  ( key ht -- flag )    \ Check if key exists
ht-size       ( ht -- size )        \ Number of entries
ht-free       ( ht -- )             \ Free memory
```

**Hints:**
- Use modulo for hash function
- Handle collisions with chaining (linked lists)
- Start with simple hash: key MOD table-size

### 6. Binary Search Tree (Bonus)

Ordered tree structure for fast lookup.

**Operations:**
```forth
bst-create    ( -- bst )            \ Create empty tree
bst-insert    ( value bst -- )      \ Insert value
bst-search    ( value bst -- flag ) \ Search for value
bst-remove    ( value bst -- flag ) \ Remove value
bst-min       ( bst -- value )      \ Find minimum
bst-max       ( bst -- value )      \ Find maximum
bst-inorder   ( bst xt -- )         \ Traverse inorder
bst-free      ( bst -- )            \ Free all nodes
```

## Implementation Guidelines

### Memory Management

**Use ALLOCATE and FREE:**
```forth
: make-node  ( value -- addr )
    2 cells allocate throw
    tuck !              \ Store value
    0 over cell+ !      \ NULL next pointer
;

: free-node  ( addr -- )
    free throw
;
```

### Error Handling

```forth
: ?index  ( index size -- index )
    2dup >= if
        ." Index out of bounds" cr abort
    then
    drop
;
```

### Iterator Pattern

```forth
\ Execute xt for each element
: list-each  ( list xt -- )
    >r
    @ begin dup while   \ Start at head
        dup @ r@ execute  \ Execute xt on value
        cell+ @         \ Move to next
    repeat
    drop r> drop
;

\ Example use:
: print-element  ( value -- )  . ;
my-list ' print-element list-each
```

## Test Suite

Create comprehensive tests:

```forth
\ Test suite for dynamic array
: test-da  ( -- )
    cr ." Testing Dynamic Array..." cr

    \ Test creation
    5 da-create constant da
    da da-size 0 = assert" Initial size should be 0"

    \ Test append
    42 da da-append
    da da-size 1 = assert" Size after append"
    0 da da-get 42 = assert" Value stored correctly"

    \ Test growth
    100 0 do i da da-append loop
    da da-size 101 = assert" Size after many appends"

    \ Test access
    50 da da-get 50 = assert" Value retrieval"

    \ Cleanup
    da da-free

    ." All tests passed!" cr
;
```

## Performance Analysis

For each structure, analyze:

| Operation | Time Complexity | Space Complexity |
|-----------|----------------|------------------|
| Insert | ? | ? |
| Search | ? | ? |
| Delete | ? | ? |

Example:
```forth
\ Benchmark insertion
: benchmark-insert  ( n -- )
    list-create >r
    milliseconds >r
    0 do i r@ list-append loop
    milliseconds r> - .
    ." ms for " dup . ." insertions" cr
    r> list-free
;

1000 benchmark-insert
10000 benchmark-insert
100000 benchmark-insert
```

## Hints

<details>
<summary>Hint 1: Dynamic Array Growth</summary>

```forth
: da-grow  ( da -- )
    dup da-capacity 2 *   \ New capacity = old * 2
    cells allocate throw  \ Allocate new space
    over da-data @        \ Old data
    over
    3 pick da-size cells  \ Copy size
    cmove                 \ Copy data
    over da-data @ free throw  \ Free old
    swap da-data !        \ Store new
;
```
</details>

<details>
<summary>Hint 2: List Node Creation</summary>

```forth
: list-make-node  ( value next -- node )
    2 cells allocate throw >r
    r@ cell+ !    \ Store next
    r@ !          \ Store value
    r>
;
```
</details>

<details>
<summary>Hint 3: Stack with Array</summary>

```forth
\ Use dynamic array as backing store
: stack-create  ( -- stack )
    10 da-create
;

: stack-push  ( value stack -- )
    da-append
;

: stack-pop  ( stack -- value )
    dup da-size 1-
    swap da-get
;
```
</details>

## Extensions

1. **Generic Types**: Make structures work with any data type
2. **Iterators**: Implement iterator protocol for all structures
3. **Persistent Structures**: Add serialization to save/load
4. **Thread-Safe**: Add locking mechanisms (advanced)
5. **Smart Pointers**: Implement reference counting
6. **Sorting**: Add sort methods to appropriate structures

## Example: Complete Linked List

```forth
\ Node structure: [value|next]
: make-node  ( value next -- node-addr )
    2 cells allocate throw >r
    r@ cell+ !   \ next
    r@ !         \ value
    r>
;

\ List structure: [head|tail|size]
: list-create  ( -- list )
    3 cells allocate throw
    0 over !                \ head = null
    0 over cell+ !          \ tail = null
    0 over 2 cells + !      \ size = 0
;

: list-append  ( value list -- )
    2dup @ 0= if           \ Empty list?
        0 rot make-node    \ Create first node
        tuck over !        \ Set head
        swap cell+ !       \ Set tail
    else
        over cell+ @       \ Get tail
        0 rot make-node    \ Create new node
        tuck swap cell+ !  \ Link old tail to new
        swap cell+ !       \ Update tail pointer
    then
    2 cells + dup @ 1+ swap !  \ Increment size
;

\ More operations...
```

## Reflection Questions

1. What are the trade-offs between arrays and linked lists?
2. When would you choose a stack over a queue?
3. How does Forth's manual memory management compare to garbage collection?
4. What did you learn about low-level data structure implementation?

## Deliverables

Create these files:
- `dynamic-array.fs` - Implementation
- `linked-list.fs` - Implementation
- `stack.fs` - Implementation
- `tests.fs` - Test suite
- `benchmarks.fs` - Performance tests
- `README.md` - Documentation

Good luck! Remember: correct first, fast second. Test thoroughly!
