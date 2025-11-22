# Forth vs C: A Comparative Guide

## Introduction

This guide helps you transfer your Forth knowledge to C (and vice versa). Both are systems programming languages that give you direct control over memory and hardware. Understanding how concepts map between them will make you a better programmer in both languages.

## Philosophy Comparison

| Aspect | Forth | C |
|--------|-------|---|
| **Paradigm** | Stack-based, concatenative | Procedural, expression-based |
| **Compilation** | Interactive, incremental | Batch compilation |
| **Syntax** | Minimalist, postfix | Rich, infix |
| **Abstraction** | Bottom-up, build your language | Top-down, use libraries |
| **Memory** | Manual, dictionary-based | Manual, heap-based |
| **Type System** | Untyped (cells and bytes) | Statically typed |

## Core Concepts

### 1. Variables

**Forth:**
```forth
variable counter
42 counter !
counter @ .
```

**C:**
```c
int counter;
counter = 42;
printf("%d\n", counter);
```

**Key Difference:**
- Forth variables store *addresses*; you fetch/store with `@` and `!`
- C variables are names for memory locations; assignment is direct

### 2. Pointers and Addresses

**Forth:**
```forth
variable x
x           \ Puts address on stack
x @         \ Fetches value at address
42 x !      \ Stores 42 at address
```

**C:**
```c
int x;
&x          // Address of x
*(&x)       // Dereference (get value)
*(&x) = 42; // Store 42 at address
```

**Equivalence Table:**

| Forth | C | Description |
|-------|---|-------------|
| `variable x` | `int x;` | Allocate integer |
| `x` | `&x` | Get address |
| `x @` | `x` or `*(&x)` | Get value |
| `42 x !` | `x = 42` | Set value |
| `x cell+` | `&x + 1` | Pointer arithmetic |

### 3. Arrays

**Forth:**
```forth
create numbers 5 cells allot

\ Set numbers[2] = 42
42 2 cells numbers + !

\ Get numbers[2]
2 cells numbers + @
```

**C:**
```c
int numbers[5];

// Set numbers[2] = 42
numbers[2] = 42;
// or
*(numbers + 2) = 42;

// Get numbers[2]
numbers[2]
// or
*(numbers + 2)
```

**Key Insight:** Forth's `cells + @` is *exactly* what C's `array[index]` does under the hood!

### 4. Functions

**Forth:**
```forth
: square  ( n -- n² )
    dup * ;

5 square .
```

**C:**
```c
int square(int n) {
    return n * n;
}

printf("%d\n", square(5));
```

**Calling Convention Comparison:**

| Forth | C |
|-------|---|
| Arguments on stack | Arguments in registers/stack |
| Postfix: `5 square` | Prefix: `square(5)` |
| No parameter list | Parameters declared |
| Stack effect comments | Type signatures |

### 5. Control Flow

#### If-Else

**Forth:**
```forth
: check-positive  ( n -- )
    0 > if
        ." Positive"
    else
        ." Not positive"
    then ;
```

**C:**
```c
void check_positive(int n) {
    if (n > 0) {
        printf("Positive");
    } else {
        printf("Not positive");
    }
}
```

#### Loops

**Forth DO...LOOP:**
```forth
: count-to-10
    10 0 do
        i .
    loop ;
```

**C for loop:**
```c
void count_to_10() {
    for (int i = 0; i < 10; i++) {
        printf("%d ", i);
    }
}
```

**Forth BEGIN...UNTIL:**
```forth
: countdown  ( n -- )
    begin
        dup .
        1-
        dup 0=
    until
    drop ;
```

**C while loop:**
```c
void countdown(int n) {
    while (n > 0) {
        printf("%d ", n);
        n--;
    }
}
```

### 6. Memory Management

**Forth:**
```forth
\ Static allocation in dictionary
create buffer 100 allot

\ Dynamic allocation
100 allocate throw constant ptr
ptr @ .
ptr free throw
```

**C:**
```c
// Static allocation
char buffer[100];

// Dynamic allocation
int *ptr = malloc(100);
printf("%d\n", *ptr);
free(ptr);
```

**Key Similarities:**
- Both require manual `free`/`FREE`
- Both risk memory leaks
- Both allow direct memory manipulation

### 7. Structures/Records

**Forth (using CREATE...DOES>):**
```forth
: struct  ( size -- )
    create allot
    does> ( -- addr ) ;

: field  ( offset size "name" -- offset' )
    create over , +
    does> ( addr -- field-addr )
        @ + ;

0
    cell field point-x
    cell field point-y
constant point-size

point-size struct my-point

10 my-point point-x !
20 my-point point-y !
```

**C:**
```c
struct Point {
    int x;
    int y;
};

struct Point my_point;
my_point.x = 10;
my_point.y = 20;
```

### 8. Strings

**Forth:**
```forth
\ Address-length pair
s" Hello" type cr

\ Counted string
create msg
    5 c,
    char H c, char e c, char l c, char l c, char o c,
```

**C:**
```c
// Null-terminated string
char *msg = "Hello";
printf("%s\n", msg);

// Character array
char msg2[] = {'H', 'e', 'l', 'l', 'o', '\0'};
```

**Key Difference:**
- Forth: length is explicit (safer!)
- C: null-terminated (compact but risky)

## Common Patterns

### Pattern 1: Swap Two Variables

**Forth:**
```forth
a @ b @     \ Get both values
b ! a !     \ Store in swapped locations
```

**C:**
```c
int temp = a;
a = b;
b = temp;
```

**Insight:** Forth uses stack naturally; C needs temporary variable.

### Pattern 2: Increment a Variable

**Forth:**
```forth
counter @ 1+ counter !

\ Or define:
: ++  ( addr -- )
    dup @ 1+ swap ! ;

counter ++
```

**C:**
```c
counter++;
// or
counter = counter + 1;
```

### Pattern 3: Function Pointers

**Forth:**
```forth
: apply  ( n xt -- result )
    execute ;

: double  ( n -- 2n )  dup + ;
: triple  ( n -- 3n )  3 * ;

5 ' double apply .    \ 10
5 ' triple apply .    \ 15
```

**C:**
```c
int apply(int n, int (*func)(int)) {
    return func(n);
}

int double_it(int n) { return n + n; }
int triple_it(int n) { return n * 3; }

printf("%d\n", apply(5, double_it)); // 10
printf("%d\n", apply(5, triple_it)); // 15
```

## The Stack vs. Registers

### Forth Stack Model

```
Operation    Stack (top on right)
---------    --------------------
5            [5]
10           [5, 10]
+            [15]
3            [15, 3]
*            [45]
```

### C Expression Model

```c
result = (5 + 10) * 3;  // Uses CPU registers internally
```

**Under the hood:** C compilers translate expressions to assembly that uses registers and stack, similar to Forth!

## Assembly Language Connection

### Forth is Closer to Assembly

**Forth:**
```forth
: add-multiply  ( a b c -- result )
    + * ;
```

**Equivalent Assembly (x86-64):**
```asm
add_multiply:
    pop  rcx        ; c
    pop  rbx        ; b
    pop  rax        ; a
    add  rax, rbx   ; a + b
    imul rax, rcx   ; result * c
    push rax        ; return value
    ret
```

**C:**
```c
int add_multiply(int a, int b, int c) {
    return (a + b) * c;
}
```

**Key Insight:** Forth's stack operations map almost 1:1 to assembly instructions!

## Performance Comparison

| Operation | Forth | C | Notes |
|-----------|-------|---|-------|
| **Integer Math** | Same | Same | Both compile to same assembly |
| **Function Calls** | Slightly faster | Fast | Forth: no parameter copying |
| **Memory Access** | Same | Same | Direct memory operations |
| **Abstraction Cost** | None | None | Both are low-level |
| **Compilation** | Instant | Slow | Forth compiles immediately |

## Type Safety

**Forth: No Type Checking**
```forth
: example  ( n -- )
    dup .           \ Print as number
    emit ;          \ Print as character

65 example      \ 65 followed by 'A'
```

**C: Static Type Checking**
```c
void example(int n) {
    printf("%d ", n);
    printf("%c", (char)n);  // Explicit cast needed
}

example(65);  // 65 followed by 'A'
```

**Trade-off:**
- Forth: More flexible, easier to make mistakes
- C: Safer, catches errors at compile-time

## Macro Systems

### Forth: Immediate Words

```forth
: [+]  ( n -- )
    postpone literal
    postpone + ; immediate

: test  5 [+] ;
\ Compiles 5 + directly into test
```

### C: Preprocessor Macros

```c
#define ADD(x) (x) +

void test() {
    int result = 5 ADD(3);  // Expands to: 5 + (3)
}
```

## Complete Program Example

### Forth: Calculate Average

```forth
\ average.fs
: average  ( n1 n2 -- avg )
    + 2 / ;

: main
    ." Enter two numbers:" cr
    ." First: " pad 10 accept pad swap evaluate
    ." Second: " pad 10 accept pad swap evaluate
    average
    ." Average: " . cr
    bye ;

main
```

### C: Calculate Average

```c
// average.c
#include <stdio.h>

int average(int a, int b) {
    return (a + b) / 2;
}

int main() {
    int n1, n2;
    printf("Enter two numbers:\n");
    printf("First: ");
    scanf("%d", &n1);
    printf("Second: ");
    scanf("%d", &n2);
    printf("Average: %d\n", average(n1, n2));
    return 0;
}
```

## When to Use Each Language

### Use Forth When:
- ✅ Interactive development is valuable
- ✅ Embedded systems with limited resources
- ✅ Rapid prototyping
- ✅ Building domain-specific languages
- ✅ Learning low-level programming concepts
- ✅ Firmware and bootloaders

### Use C When:
- ✅ Large team collaboration (more familiar)
- ✅ Rich ecosystem of libraries needed
- ✅ Type safety is critical
- ✅ Industry standard requirements
- ✅ Operating system development
- ✅ Interfacing with existing C code

### Use Both When:
- ✅ You want to truly understand systems programming
- ✅ Learning how languages work under the hood
- ✅ Experimenting with language design

## Translation Cheat Sheet

| Concept | Forth | C |
|---------|-------|---|
| **Variable** | `variable x` | `int x;` |
| **Get address** | `x` | `&x` |
| **Get value** | `x @` | `x` |
| **Set value** | `42 x !` | `x = 42;` |
| **Array** | `create a 10 cells allot` | `int a[10];` |
| **Index** | `i cells a + @` | `a[i]` |
| **Pointer** | `x` (variables ARE pointers) | `int *p = &x;` |
| **Dereference** | `@` | `*` |
| **Function** | `: foo ... ;` | `void foo() { ... }` |
| **If** | `if ... then` | `if (...) { ... }` |
| **Loop** | `begin ... until` | `while (...) { ... }` |
| **For loop** | `10 0 do ... loop` | `for (i=0; i<10; i++) { ... }` |
| **Malloc** | `allocate` | `malloc()` |
| **Free** | `free` | `free()` |
| **Print int** | `.` | `printf("%d", n);` |
| **Print string** | `type` | `printf("%s", s);` |

## Interoperability

You can call C from Forth and vice versa!

**Calling C from Forth (gforth):**
```forth
c-library mylib
    \c #include <math.h>
    c-function sqrt sqrt d -- d
end-c-library

25e sqrt f. cr    \ 5.0
```

**Calling Forth from C:**
```c
// Embed Forth interpreter
#include <gforth.h>

int main() {
    gforth_init();
    gforth_evaluate("5 5 + .");
    return 0;
}
```

## Learning Path: Forth → C

If you know Forth and want to learn C:

1. ✅ **You already understand:**
   - Pointers and memory
   - Manual memory management
   - Low-level operations
   - How functions work

2. **Learn these new concepts:**
   - Infix syntax
   - Type system
   - Structs and enums
   - Preprocessor
   - Header files
   - Standard library

3. **Practice translation:**
   - Convert your Forth programs to C
   - Implement C algorithms in Forth
   - Compare the assembly output

## Learning Path: C → Forth

If you know C and want to learn Forth:

1. ✅ **You already understand:**
   - Systems programming concepts
   - Memory management
   - Pointers
   - Function calls

2. **Learn these new concepts:**
   - Stack-based thinking
   - Postfix notation (RPN)
   - Word definitions
   - Interactive development
   - Concatenative programming
   - Factoring strategies

3. **Mental shift required:**
   - Think "data flow" not "variables"
   - Think "words" not "functions"
   - Think "bottom-up" not "top-down"

## Conclusion

Forth and C are both excellent systems programming languages. Learning both gives you:

- **Deep understanding** of how computers work
- **Versatile skills** applicable to many domains
- **Appreciation** for different programming paradigms
- **Ability to choose** the right tool for the job

Key takeaway: **Forth's `@ !` are C's `* &`**. Once you understand this equivalence, the languages feel like two ways of expressing the same low-level operations!

## Further Reading

- **C Primer:** Learn C from a Forth background
- **Assembly Guide:** See how both compile to machine code
- **Embedded Systems:** Using Forth and C together
- **Compiler Design:** How C and Forth compilers work

Happy programming! 🚀
