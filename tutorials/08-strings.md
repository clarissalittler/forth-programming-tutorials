# Tutorial 8: Strings and Character Operations

## Introduction

Strings in Forth are different from most languages. Forth uses two common representations:
- **Counted strings**: First byte contains length
- **Address-length pairs**: Separate address and count on stack

## Printing Strings

We've already seen `."` for printing strings:

```forth
." Hello, World!" cr
```

This is compile-time - the string is embedded in the word definition.

## String Literals with S"

`S"` creates a string at runtime, leaving address and length on the stack.

**Stack effect:** `( -- addr len )`

```forth
s" Hello" type cr
```

- `s"` pushes address and length
- `type` prints a string given address and length

### Example: Storing a String

```forth
variable str-addr
variable str-len

: save-string
    s" Forth is awesome!"
    str-len ! str-addr !
;

: print-saved
    str-addr @ str-len @ type cr
;
```

Test it:
```forth
save-string
print-saved     \ Forth is awesome!
```

## Character Literals

Get the ASCII code of a character with `char`:

```forth
char A .        \ 65
char Z .        \ 90
char 0 .        \ 48
```

Or use the `[char]` form in word definitions:
```forth
: print-A
    [char] A emit cr
;
```

## Character Operations

### EMIT: Print a Character

**Stack effect:** `( char -- )`

```forth
65 emit         \ A
char B emit     \ B
10 emit         \ Newline
```

### KEY: Read a Character

**Stack effect:** `( -- char )`

```forth
: wait-for-key
    ." Press a key: "
    key dup emit cr
    ." You pressed: " .
;
```

### TYPE: Print a String

**Stack effect:** `( addr len -- )`

```forth
s" Hello" type cr
```

### COUNT: Convert Counted String

**Stack effect:** `( c-addr -- addr len )`

Converts a counted string (length in first byte) to addr/len pair:

```forth
create message 5 c, char H c, char e c, char l c, char l c, char o c,
message count type cr       \ Hello
```

## String Comparison

### COMPARE: Compare Two Strings

**Stack effect:** `( addr1 len1 addr2 len2 -- n )`

Returns:
- 0 if equal
- <0 if string1 < string2
- \>0 if string1 > string2

```forth
s" abc" s" abc" compare .   \ 0 (equal)
s" abc" s" def" compare .   \ Negative (abc < def)
```

### String Equality

```forth
: string=  ( addr1 len1 addr2 len2 -- flag )
    compare 0=
;

s" hello" s" hello" string= .   \ -1 (true)
s" hello" s" world" string= .   \ 0 (false)
```

## String Building and Buffers

### Creating a String Buffer

```forth
256 constant MAX-STRING
create string-buffer MAX-STRING allot
variable string-length

: clear-buffer
    0 string-length !
    string-buffer MAX-STRING erase
;

: append-char  ( char -- )
    string-length @ string-buffer + c!
    1 string-length +!
;

: append-string  ( addr len -- )
    dup string-length @ + MAX-STRING > IF
        2drop ." Buffer overflow!" cr
    ELSE
        string-length @ string-buffer + swap move
        string-length +!
    THEN
;

: show-buffer
    string-buffer string-length @ type cr
;
```

Test it:
```forth
clear-buffer
char H append-char
char i append-char
show-buffer         \ Hi
s"  there" append-string
show-buffer         \ Hi there
```

## Counted Strings

Counted strings store length in the first byte:

```forth
create greeting 20 allot

: set-greeting  ( addr len -- )
    dup greeting c!         \ Store length
    greeting 1+ swap move   \ Copy string after length byte
;

: show-greeting
    greeting count type cr
;
```

Test it:
```forth
s" Hello, Forth!" set-greeting
show-greeting       \ Hello, Forth!
```

## Character Classification

Check character types:

```forth
: is-digit?  ( char -- flag )
    dup [char] 0 >= swap [char] 9 <= and
;

: is-upper?  ( char -- flag )
    dup [char] A >= swap [char] Z <= and
;

: is-lower?  ( char -- flag )
    dup [char] a >= swap [char] z <= and
;

: is-alpha?  ( char -- flag )
    dup is-upper? swap is-lower? or
;

: is-space?  ( char -- flag )
    dup 32 = swap 9 = or    \ Space or tab
;
```

Test it:
```forth
char 5 is-digit? .      \ -1 (true)
char A is-upper? .      \ -1 (true)
char z is-lower? .      \ -1 (true)
char Z is-lower? .      \ 0 (false)
```

## Character Conversion

```forth
: to-upper  ( char -- upper-char )
    dup is-lower? IF
        32 -
    THEN
;

: to-lower  ( char -- lower-char )
    dup is-upper? IF
        32 +
    THEN
;
```

Test it:
```forth
char a to-upper emit cr     \ A
char Z to-lower emit cr     \ z
```

## String Parsing

### WORD: Parse a Word

`WORD` reads the next word from input:

```forth
: test-word
    word count type cr
;

\ In interactive mode:
\ test-word hello
\ hello
```

### PARSE: Parse Until Delimiter

**Stack effect:** `( char -- addr len )`

```forth
: parse-line
    10 parse        \ 10 is newline
    type cr
;
```

## Formatted Output

### Numeric Formatting

```forth
\ Print number right-aligned in field
: .r  ( n width -- )
    >r dup abs <# #s swap sign #>
    r> over - spaces type
;

42 5 .r         \ "   42"
-7 4 .r         \ "  -7"
```

### Creating Formatted Strings

```forth
: show-score  ( score -- )
    ." Player score: " . cr
;

: show-player  ( name-addr name-len score -- )
    -rot type ."  - Score: " . cr
;
```

Test it:
```forth
100 show-score                      \ Player score: 100
s" Alice" 95 show-player            \ Alice - Score: 95
```

## Practical Examples

### Example 1: String Length

```forth
: string-length  ( addr len -- len )
    nip
;

s" Hello" string-length .   \ 5
```

### Example 2: String Copy

```forth
create dest 50 allot

: string-copy  ( src-addr src-len -- )
    dup dest c!             \ Store length
    dest 1+ swap move       \ Copy string
;

: show-dest
    dest count type cr
;

s" Copied!" string-copy
show-dest                   \ Copied!
```

### Example 3: String Concatenation

```forth
create result 100 allot

: string-concat  ( addr1 len1 addr2 len2 -- addr len )
    result 100 erase
    2swap                   \ addr2 len2 addr1 len1
    dup result c!           \ Store first length
    result 1+ swap move     \ Copy first string
    result count + swap move    \ Append second string
    result dup c@ 2swap + swap c!   \ Update length
    result count
;

s" Hello, " s" World!" string-concat type cr
```

### Example 4: Word Counter

```forth
: count-words  ( addr len -- count )
    0 -rot          \ Counter
    BEGIN
        dup 0>
    WHILE
        over c@ is-space? IF
            1 /string
        ELSE
            1 rot + -rot        \ Increment counter
            BEGIN
                dup 0> over c@ is-space? 0= and
            WHILE
                1 /string
            REPEAT
        THEN
    REPEAT
    2drop
;

s" hello world forth" count-words . \ 3
```

### Example 5: Simple Text Justification

```forth
: center-text  ( addr len width -- )
    over - 2 /      \ Calculate left padding
    dup 0> IF
        spaces
    ELSE
        drop
    THEN
    type cr
;

s" Forth" 20 center-text    \ Centers "Forth" in 20 chars
```

### Example 6: String Search

```forth
: string-find  ( substr-addr substr-len str-addr str-len -- index|-1 )
    2>r 2dup 2r> 2swap      \ Rearrange for loop
    2over nip 1 + 0 DO
        2dup I /string
        2over compare 0= IF
            2drop 2drop I UNLOOP EXIT
        THEN
    LOOP
    2drop 2drop -1
;

s" or" s" Hello World" string-find .    \ 7
s" xyz" s" Hello World" string-find .   \ -1
```

## Input Operations

### ACCEPT: Read a Line

**Stack effect:** `( addr maxlen -- len )`

```forth
create input-buffer 80 allot

: get-input
    ." Enter text: "
    input-buffer 80 accept
    input-buffer swap
;

: echo-input
    get-input type cr
;
```

### Reading Numbers

```forth
: read-number  ( -- n )
    pad 20 accept
    pad swap evaluate
;

: ask-age
    ." How old are you? "
    read-number
    ." You are " . ." years old." cr
;
```

## String Table

Create a table of strings:

```forth
create days
    s" Monday" 2,
    s" Tuesday" 2,
    s" Wednesday" 2,
    s" Thursday" 2,
    s" Friday" 2,
    s" Saturday" 2,
    s" Sunday" 2,

: get-day  ( index -- addr len )
    2 * cells days + 2@
;

: show-day  ( index -- )
    get-day type cr
;

0 show-day      \ Monday
4 show-day      \ Friday
```

## Common Patterns

### Pattern: Build a String from Parts

```forth
: build-greeting  ( name-addr name-len -- addr len )
    s" Hello, " 2swap string-concat
    s" !" string-concat
;

s" Alice" build-greeting type cr    \ Hello, Alice!
```

### Pattern: Filter Characters

```forth
: filter-digits  ( addr len -- addr2 len2 )
    result 100 erase
    0 -rot          \ result-len src-addr src-len
    0 DO
        over I + c@
        dup is-digit? IF
            over result + c!
            swap 1 + swap
        ELSE
            drop
        THEN
    LOOP
    drop result swap
;

s" abc123def456" filter-digits type cr  \ 123456
```

## Common Mistakes

### 1. Confusing String Representations

```forth
\ WRONG: Mixing representations
s" hello" c@        \ addr is not a counted string!

\ RIGHT:
s" hello" drop c@   \ Get first char of addr/len string
```

### 2. Not Preserving Strings

```forth
\ WRONG: s" strings may not persist
: broken
    s" temporary"
;
broken type         \ May crash! String might be overwritten

\ RIGHT: Copy to permanent storage
create permanent 50 allot
: correct
    s" permanent" dup permanent c! permanent 1+ swap move
    permanent count
;
```

### 3. Buffer Overflow

```forth
\ WRONG: No bounds check
: unsafe-append  ( addr len -- )
    string-buffer swap move     \ Oops! Could overflow!
;

\ RIGHT: Check bounds
: safe-append  ( addr len -- )
    dup string-length @ + MAX-STRING > IF
        drop drop ." Overflow!" cr
    ELSE
        string-length @ string-buffer + swap move
        string-length +!
    THEN
;
```

## Quick Reference

### String Creation
```forth
." text"            \ Print at compile time
s" text"            \ Create string (addr len)
char c              \ Get ASCII code
[char] c            \ Compile-time char
```

### String Output
```forth
type                \ ( addr len -- )
emit                \ ( char -- )
cr                  \ Newline
spaces              \ ( n -- ) print n spaces
```

### String Input
```forth
key                 \ ( -- char )
accept              \ ( addr maxlen -- len )
word                \ Parse next word
parse               \ ( char -- addr len )
```

### String Operations
```forth
compare             \ ( a1 l1 a2 l2 -- n )
move                \ ( src dest len -- )
count               \ ( c-addr -- addr len )
```

### Character Tests
```forth
is-digit?           \ Check if 0-9
is-upper?           \ Check if A-Z
is-lower?           \ Check if a-z
is-alpha?           \ Check if letter
```

## Best Practices

1. **Use addr/len pairs** - More flexible than counted strings
2. **Always check buffer bounds** - Prevent overflow
3. **Copy temporary strings** - s" strings may be ephemeral
4. **Use meaningful names** - `user-input` not `buff`
5. **Validate input** - Check length and content
6. **Preallocate buffers** - Avoid dynamic allocation
7. **Document string format** - Counted vs addr/len

## Exercises

1. Write a word to reverse a string
2. Implement a word to check if a string is a palindrome
3. Create a word to count vowels in a string
4. Implement string trimming (remove leading/trailing spaces)
5. Write a word to replace all occurrences of a character
6. Create a simple template system (replace {name} with value)
7. Implement word wrapping for text
8. Create a word to validate email format (simplified)
9. Build a simple command parser
10. Implement a ROT13 cipher

## Solutions

1.
```forth
: reverse-string  ( addr len -- addr2 len2 )
    result over 1 - + swap
    0 DO
        over I + c@
        over I - c!
    LOOP
    drop result swap
;
```

2.
```forth
: palindrome?  ( addr len -- flag )
    2dup reverse-string compare 0=
;
```

3.
```forth
: count-vowels  ( addr len -- count )
    0 -rot
    0 DO
        over I + c@ to-lower
        dup [char] a = over [char] e = or
        over [char] i = or over [char] o = or
        swap [char] u = or
        IF 1 rot + swap THEN
    LOOP
    drop
;
```

4.
```forth
: trim-string  ( addr len -- addr2 len2 )
    \ Remove leading spaces
    BEGIN
        dup 0> over c@ is-space? and
    WHILE
        1 /string
    REPEAT
    \ Remove trailing spaces
    BEGIN
        dup 0> 2dup + 1 - c@ is-space? and
    WHILE
        1 -
    REPEAT
;
```

5.
```forth
: replace-char  ( addr len old new -- )
    -rot 0 DO
        2over drop I + c@
        over = IF
            over 2over drop I + c!
        THEN
    LOOP
    2drop drop
;
```

## Conclusion

You've completed the Forth programming tutorial series! You now know:

- Stack operations and manipulation
- Arithmetic and logic
- Defining words and building vocabularies
- Control flow (conditionals and loops)
- Variables and memory management
- Arrays and data structures
- String handling and character operations

### What's Next?

To continue your Forth journey:

1. **Practice!** - Build real programs
2. **Read the gforth manual** - Learn system-specific features
3. **Study Thinking Forth** - Learn Forth philosophy and design
4. **Join the community** - comp.lang.forth newsgroup, forums
5. **Write libraries** - Create reusable code
6. **Explore applications** - Embedded systems, DSLs, etc.

### Project Ideas

- Calculator with RPN interface
- Text adventure game
- Turtle graphics system
- Simple database
- Forth-based scripting language
- Embedded system controller
- Math library (trig, matrices)
- Network protocol implementation

Happy Forth programming!
