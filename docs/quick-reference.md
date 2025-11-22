# Forth Quick Reference

## Stack Notation

```
( before -- after )

Example: +  ( n1 n2 -- sum )
```

## Stack Operations

| Word | Effect | Visual | Description |
|------|--------|--------|-------------|
| `dup` | `( n -- n n )` | `[a]` → `[a a]` | Duplicate top |
| `drop` | `( n -- )` | `[a b]` → `[a]` | Remove top |
| `swap` | `( n1 n2 -- n2 n1 )` | `[a b]` → `[b a]` | Swap top two |
| `over` | `( n1 n2 -- n1 n2 n1 )` | `[a b]` → `[a b a]` | Copy second |
| `rot` | `( n1 n2 n3 -- n2 n3 n1 )` | `[a b c]` → `[b c a]` | Rotate three |
| `2dup` | `( n1 n2 -- n1 n2 n1 n2 )` | `[a b]` → `[a b a b]` | Dup top two |
| `2drop` | `( n1 n2 -- )` | `[a b c]` → `[a]` | Drop top two |
| `2swap` | `( n1 n2 n3 n4 -- n3 n4 n1 n2 )` | `[a b c d]` → `[c d a b]` | Swap pairs |
| `nip` | `( n1 n2 -- n2 )` | `[a b]` → `[b]` | Drop second |
| `tuck` | `( n1 n2 -- n2 n1 n2 )` | `[a b]` → `[b a b]` | Copy top below |

## Arithmetic

| Word | Effect | Description |
|------|--------|-------------|
| `+` | `( n1 n2 -- sum )` | Add |
| `-` | `( n1 n2 -- diff )` | Subtract (n1-n2) |
| `*` | `( n1 n2 -- prod )` | Multiply |
| `/` | `( n1 n2 -- quot )` | Divide (integer) |
| `mod` | `( n1 n2 -- rem )` | Remainder |
| `/mod` | `( n1 n2 -- rem quot )` | Both |
| `negate` | `( n -- -n )` | Negate |
| `abs` | `( n -- |n| )` | Absolute value |
| `min` | `( n1 n2 -- min )` | Minimum |
| `max` | `( n1 n2 -- max )` | Maximum |
| `1+` | `( n -- n+1 )` | Increment |
| `1-` | `( n -- n-1 )` | Decrement |
| `2*` | `( n -- n*2 )` | Double |
| `2/` | `( n -- n/2 )` | Halve |

## Comparison

| Word | Effect | Description |
|------|--------|-------------|
| `=` | `( n1 n2 -- flag )` | Equal |
| `<>` | `( n1 n2 -- flag )` | Not equal |
| `<` | `( n1 n2 -- flag )` | Less than |
| `>` | `( n1 n2 -- flag )` | Greater than |
| `<=` | `( n1 n2 -- flag )` | Less or equal |
| `>=` | `( n1 n2 -- flag )` | Greater or equal |
| `0=` | `( n -- flag )` | Equal to zero |
| `0<` | `( n -- flag )` | Less than zero |
| `0>` | `( n -- flag )` | Greater than zero |

**Note:** `-1` is true, `0` is false

## Logic

| Word | Effect | Description |
|------|--------|-------------|
| `and` | `( flag1 flag2 -- flag )` | Logical AND |
| `or` | `( flag1 flag2 -- flag )` | Logical OR |
| `xor` | `( flag1 flag2 -- flag )` | Logical XOR |
| `invert` | `( flag -- !flag )` | Logical NOT |

## Bitwise

| Word | Effect | Description |
|------|--------|-------------|
| `lshift` | `( n count -- n<<count )` | Left shift |
| `rshift` | `( n count -- n>>count )` | Right shift |
| `and` | `( n1 n2 -- n )` | Bitwise AND |
| `or` | `( n1 n2 -- n )` | Bitwise OR |
| `xor` | `( n1 n2 -- n )` | Bitwise XOR |
| `invert` | `( n -- ~n )` | Bitwise NOT |

## Control Flow

### If-Then-Else
```forth
condition IF
    \ true branch
ELSE
    \ false branch
THEN
```

### Counted Loop
```forth
limit start DO
    I    \ loop counter
LOOP

\ Custom increment
limit start DO
    I
2 +LOOP
```

### Conditional Loops
```forth
BEGIN
    \ body
condition UNTIL

BEGIN
    condition
WHILE
    \ body
REPEAT
```

### Case
```forth
value CASE
    test1 OF action1 ENDOF
    test2 OF action2 ENDOF
    default-action
ENDCASE
```

## Variables & Memory

| Word | Effect | Description |
|------|--------|-------------|
| `variable x` | Creates variable x | |
| `x` | `( -- addr )` | Get address |
| `@` | `( addr -- n )` | Fetch value |
| `!` | `( n addr -- )` | Store value |
| `+!` | `( n addr -- )` | Add to value |
| `?` | `( addr -- )` | Print value |
| `constant x` | Define constant | `42 constant answer` |
| `value x` | Define value | `42 value counter` |
| `to` | `( n -- )` | Set value | `100 to counter` |

### Byte Operations
| Word | Effect | Description |
|------|--------|-------------|
| `c@` | `( addr -- byte )` | Fetch byte |
| `c!` | `( byte addr -- )` | Store byte |

### Arrays
```forth
create array 10 cells allot
42 2 cells array + !    \ Set array[2]
2 cells array + @       \ Get array[2]
```

## Defining Words

```forth
\ Basic definition
: word-name  ( stack-effect )
    \ code
;

\ Constant
42 constant answer

\ Variable
variable counter

\ Array/buffer
create buffer 100 allot

\ Compile data
create primes  2 , 3 , 5 , 7 , 11 ,

\ Meta-programming
: array  ( n "name" -- )
    create cells allot
    does> ( index -- addr )
        swap cells + ;
```

## I/O

| Word | Effect | Description |
|------|--------|-------------|
| `.` | `( n -- )` | Print number |
| `.s` | `( -- )` | Show stack (non-destructive) |
| `emit` | `( char -- )` | Print character |
| `type` | `( addr len -- )` | Print string |
| `cr` | `( -- )` | Newline |
| `space` | `( -- )` | Print space |
| `spaces` | `( n -- )` | Print n spaces |
| `."` | Compile-time string | `." Hello" cr` |
| `s"` | Runtime string | `s" text" type` |

## Debugging

| Word | Effect | Description |
|------|--------|-------------|
| `.s` | Show stack | Non-destructive |
| `depth` | `( -- n )` | Stack depth |
| `see word` | Show definition | Decompile |
| `words` | List all words | |
| `bye` | Exit Forth | |

## Number Bases

| Word | Effect | Description |
|------|--------|-------------|
| `decimal` | Switch to base 10 | |
| `hex` | Switch to base 16 | |
| `binary` | Switch to base 2 | (if available) |
| `base` | `( -- addr )` | Base variable |

### Prefixes (gforth)
```forth
$FF     \ Hex (255)
%1010   \ Binary (10)
42      \ Decimal (42)
```

## Memory Words

| Word | Effect | Description |
|------|--------|-------------|
| `here` | `( -- addr )` | Dictionary pointer |
| `allot` | `( n -- )` | Allocate n bytes |
| `,` | `( n -- )` | Compile cell |
| `c,` | `( byte -- )` | Compile byte |
| `allocate` | `( n -- addr ior )` | Malloc |
| `free` | `( addr -- ior )` | Free |
| `cells` | `( n -- bytes )` | Convert cells to bytes |
| `cell+` | `( addr -- addr+cell )` | Increment by cell |

## Common Patterns

### Swap values in memory
```forth
a @ b @ b ! a !
```

### Increment variable
```forth
1 counter +!
```

### Range check
```forth
: between?  ( n low high -- flag )
    rot dup rot >= -rot <= and ;
```

### Apply function to array
```forth
: array-map  ( xt array len -- )
    0 do
        dup i cells + dup @
        rot execute
        swap !
    loop drop ;
```

## Stack Effect Patterns

| Pattern | Means |
|---------|-------|
| `( -- )` | No inputs, no outputs |
| `( n -- )` | Consumes one value |
| `( -- n )` | Produces one value |
| `( n -- n )` | Transforms one value |
| `( n1 n2 -- n3 )` | Two inputs, one output |
| `( ... -- )` | Consumes entire stack |

## Common Errors

| Error | Likely Cause | Fix |
|-------|-------------|-----|
| Stack underflow | Not enough items | Use `.s` before operation |
| Stack overflow | Too many items | Check for missing drops |
| Undefined word | Typo or not defined | Check spelling, use `words` |
| Division by zero | Dividing by 0 | Add zero check |

## Quick Tips

1. **Use `.s` constantly** - Check stack after every operation
2. **Test incrementally** - Define and test each word immediately
3. **Document stack effects** - Always use `( -- )` notation
4. **Factor early** - Break complex words into simple ones
5. **Think data flow** - Follow values through the stack
6. **Start simple** - Build from working simple words
7. **Read aloud** - Helps catch stack errors

## Example Program

```forth
\ Calculate average of two numbers
: average  ( n1 n2 -- avg )
    + 2 / ;

\ Test it
10 20 average .    \ 15

\ Store in variable
variable result
10 20 average result !
result ? .         \ 15
```

## Conversion Table: Forth ↔ C

| Forth | C | Meaning |
|-------|---|---------|
| `variable x` | `int x;` | Declare variable |
| `x` | `&x` | Get address |
| `x @` | `x` | Get value |
| `42 x !` | `x = 42` | Set value |
| `create a 10 cells allot` | `int a[10];` | Array |
| `i cells a + @` | `a[i]` | Index array |
| `: foo ... ;` | `void foo() {...}` | Function |
| `if ... then` | `if (...) {...}` | Conditional |
| `10 0 do ... loop` | `for (i=0; i<10; i++) {...}` | Loop |

## ASCII Values

```
'A' = 65    'a' = 97    '0' = 48
'Z' = 90    'z' = 122   '9' = 57
' ' = 32    '\n' = 10   '\t' = 9
```

## Forth Vocabulary

- **Word**: A Forth function/command
- **Dictionary**: Where words are stored
- **Cell**: Machine word size (typically 8 bytes)
- **Stack**: LIFO data structure
- **Immediate**: Word that executes during compilation
- **Factor**: Break a word into smaller words

---

**Print this page and keep it handy while learning Forth!**

**Online help:** Type `words` to see all words, `see <word>` to view definition
