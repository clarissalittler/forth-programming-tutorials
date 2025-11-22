# Tutorial 10: Bit Manipulation and Binary Operations

## Learning Objectives

By the end of this tutorial, you will be able to:
- 🎯 Perform bitwise operations: AND, OR, XOR, NOT, shifts
- 🎯 Use bit masks to manipulate individual bits and bit fields
- 🎯 Understand two's complement and signed vs unsigned numbers
- 🎯 Convert between binary, decimal, and hexadecimal
- 🎯 Implement bit flags for compact state storage
- 🎯 Recognize how bit manipulation enables systems programming
- 🎯 Apply bit operations to solve real-world problems (parsing, encoding, hardware control)

**Connection to other languages:** Bit manipulation is fundamental to C, assembly language, and systems programming. Understanding these operations prepares you for embedded systems, device drivers, network protocols, and performance optimization.

## Introduction

Bit manipulation is the art of working with individual bits and groups of bits. This is essential for:
- **Hardware control** - Setting GPIO pins, configuring registers
- **Data compression** - Packing multiple values efficiently
- **Cryptography** - XOR encryption, hashing
- **Graphics** - Color manipulation, pixel operations
- **Networking** - Protocol headers, checksums
- **Performance** - Bit operations are extremely fast

In Forth, bit manipulation is natural because you work directly with the underlying data representation.

## Understanding Binary Representation

### Viewing Numbers in Binary

```forth
\ Switch to binary display
2 base !

15 .            \ 1111
255 .           \ 11111111
170 .           \ 10101010

\ Back to decimal
decimal
```

### gforth Binary Literals

```forth
%1010 .         \ 10 (binary input)
$FF .           \ 255 (hex input)
42 .            \ 42 (decimal input)

\ Display in different bases
42 hex . decimal        \ 2A
42 2 base ! . decimal   \ 101010
```

### Bit Positions

```
Bit:     7  6  5  4  3  2  1  0
Value:  128 64 32 16  8  4  2  1
        ─┬──┬──┬──┬──┬──┬──┬──┬─
Example: 1  0  1  0  1  0  1  0  = 170
```

## Bitwise Operations

### AND - Both Bits Must Be 1

```forth
: show-and  ( -- )
    %1100 %1010 and .    \ %1000 (8)
;
```

**Truth table:**
```
1 AND 1 = 1
1 AND 0 = 0
0 AND 1 = 0
0 AND 0 = 0
```

**Use case:** Masking (extracting specific bits)

### OR - At Least One Bit Is 1

```forth
: show-or  ( -- )
    %1100 %1010 or .     \ %1110 (14)
;
```

**Truth table:**
```
1 OR 1 = 1
1 OR 0 = 1
0 OR 1 = 1
0 OR 0 = 0
```

**Use case:** Setting specific bits

### XOR - Bits Are Different

```forth
: show-xor  ( -- )
    %1100 %1010 xor .    \ %0110 (6)
;
```

**Truth table:**
```
1 XOR 1 = 0
1 XOR 0 = 1
0 XOR 1 = 1
0 XOR 0 = 0
```

**Use case:** Toggling bits, simple encryption

### NOT/INVERT - Flip All Bits

```forth
: show-invert  ( -- )
    %1010 invert .       \ All bits flipped
;
```

**Note:** `INVERT` flips *all* bits in a cell, including sign bit. On a 64-bit system, `%1010 invert` gives a very large number (or negative if interpreted as signed).

## Bit Shifts

### Left Shift (LSHIFT) - Multiply by 2

```forth
: show-lshift  ( -- )
    1 0 lshift .    \ 1 (1 << 0)
    1 1 lshift .    \ 2 (1 << 1)
    1 2 lshift .    \ 4 (1 << 2)
    1 3 lshift .    \ 8 (1 << 3)
    1 7 lshift .    \ 128 (1 << 7)
;
```

**Visual:**
```
Original:  0000 1010  (10)
<< 1:      0001 0100  (20)  ← Each shift left = * 2
<< 2:      0010 1000  (40)
```

### Right Shift (RSHIFT) - Divide by 2

```forth
: show-rshift  ( -- )
    128 0 rshift .  \ 128 (128 >> 0)
    128 1 rshift .  \ 64  (128 >> 1)
    128 2 rshift .  \ 32  (128 >> 2)
    128 7 rshift .  \ 1   (128 >> 7)
;
```

**Visual:**
```
Original:  1000 0000  (128)
>> 1:      0100 0000  (64)   ← Each shift right = / 2
>> 2:      0010 0000  (32)
```

## Bit Masking

### Extract Specific Bits

```forth
\ Extract lower 4 bits (nibble)
: low-nibble  ( n -- nibble )
    $0F and ;       \ Mask with 0000 1111

197 low-nibble .    \ 5 (binary: 11000101 AND 00001111 = 00000101)

\ Extract upper 4 bits
: high-nibble  ( n -- nibble )
    4 rshift
    $0F and ;

197 high-nibble .   \ 12 (binary: 11000101 >> 4 = 00001100)
```

### Set Specific Bits

```forth
\ Set bit n to 1
: set-bit  ( value bit-pos -- value' )
    1 swap lshift
    or ;

0 3 set-bit .       \ 8 (set bit 3: 00000000 → 00001000)
5 1 set-bit .       \ 7 (set bit 1: 00000101 → 00000111)
```

### Clear Specific Bits

```forth
\ Clear bit n to 0
: clear-bit  ( value bit-pos -- value' )
    1 swap lshift
    invert
    and ;

15 2 clear-bit .    \ 11 (clear bit 2: 00001111 → 00001011)
```

### Test Specific Bits

```forth
\ Test if bit n is set
: test-bit  ( value bit-pos -- flag )
    1 swap lshift
    and 0<> ;

12 2 test-bit .     \ -1 (bit 2 is set: 00001100)
12 0 test-bit .     \ 0  (bit 0 is clear: 00001100)
```

### Toggle Specific Bits

```forth
\ Flip bit n
: toggle-bit  ( value bit-pos -- value' )
    1 swap lshift
    xor ;

12 1 toggle-bit .   \ 14 (toggle bit 1: 00001100 → 00001110)
14 1 toggle-bit .   \ 12 (toggle bit 1: 00001110 → 00001100)
```

## Practical Applications

### 1. Bit Flags

Use bits to store multiple boolean flags efficiently:

```forth
\ Define flag positions
0 constant FLAG-READY
1 constant FLAG-BUSY
2 constant FLAG-ERROR
3 constant FLAG-DEBUG

variable system-flags

\ Set a flag
: set-flag  ( flag-pos -- )
    1 swap lshift
    system-flags @ or
    system-flags ! ;

\ Clear a flag
: clear-flag  ( flag-pos -- )
    1 swap lshift invert
    system-flags @ and
    system-flags ! ;

\ Test a flag
: flag?  ( flag-pos -- flag )
    1 swap lshift
    system-flags @ and 0<> ;

\ Usage:
FLAG-READY set-flag
FLAG-BUSY set-flag

FLAG-READY flag? . cr      \ -1 (true)
FLAG-ERROR flag? . cr      \ 0 (false)
```

### 2. Color Manipulation (RGB)

```forth
\ RGB color: 0xRRGGBB (8 bits per channel)

\ Extract red channel
: get-red  ( color -- red )
    16 rshift $FF and ;

\ Extract green channel
: get-green  ( color -- green )
    8 rshift $FF and ;

\ Extract blue channel
: get-blue  ( color -- blue )
    $FF and ;

\ Create color from components
: make-color  ( r g b -- color )
    $FF and                 \ Mask blue
    swap $FF and 8 lshift or    \ Add green
    swap $FF and 16 lshift or ; \ Add red

\ Test:
$FF $80 $00 make-color hex . decimal  \ FF8000 (orange)

$FF8000 get-red .          \ 255
$FF8000 get-green .        \ 128
$FF8000 get-blue .         \ 0
```

### 3. Packing Multiple Small Values

```forth
\ Pack 4 bytes into one cell
: pack-bytes  ( b3 b2 b1 b0 -- packed )
    $FF and                     \ b0
    swap $FF and 8 lshift or    \ b1
    swap $FF and 16 lshift or   \ b2
    swap $FF and 24 lshift or ; \ b3

\ Unpack bytes
: unpack-byte0  ( packed -- b0 )  $FF and ;
: unpack-byte1  ( packed -- b1 )  8 rshift $FF and ;
: unpack-byte2  ( packed -- b2 )  16 rshift $FF and ;
: unpack-byte3  ( packed -- b3 )  24 rshift $FF and ;

\ Test:
10 20 30 40 pack-bytes constant test-packed

test-packed unpack-byte0 .  \ 40
test-packed unpack-byte1 .  \ 30
test-packed unpack-byte2 .  \ 20
test-packed unpack-byte3 .  \ 10
```

### 4. Simple XOR Encryption

```forth
: xor-encrypt  ( byte key -- encrypted )
    xor ;

: xor-decrypt  ( encrypted key -- byte )
    xor ;       \ XOR is its own inverse!

\ Encrypt a message
: encrypt-string  ( addr len key -- )
    -rot 0 do
        2dup i + dup c@
        rot xor-encrypt
        swap c!
    loop
    2drop ;

\ Test:
create message 10 allot
s" Hello" message swap cmove

." Original: " message 5 type cr
message 5 42 encrypt-string
." Encrypted: " message 5 type cr
message 5 42 encrypt-string
." Decrypted: " message 5 type cr
```

### 5. Bit Field Operations

```forth
\ Extract bit field from value
: extract-field  ( value bit-pos width -- field )
    1 swap lshift 1-    \ Create mask: width bits
    swap rshift
    and ;

\ Set bit field in value
: set-field  ( value field bit-pos width -- value' )
    >r >r               \ Save bit-pos and width
    1 r@ lshift 1-      \ Create mask
    r> lshift invert    \ Shift and invert mask
    over and            \ Clear field in value
    r> lshift or ;      \ Insert new field

\ Example: Manipulate 8-bit value with fields:
\ Bits 0-2: channel (0-7)
\ Bits 3-5: mode (0-7)
\ Bits 6-7: status (0-3)

: get-channel  ( value -- channel )  0 3 extract-field ;
: get-mode     ( value -- mode )     3 3 extract-field ;
: get-status   ( value -- status )   6 2 extract-field ;

: set-channel  ( value channel -- value' )  0 3 set-field ;
: set-mode     ( value mode -- value' )     3 3 set-field ;
: set-status   ( value status -- value' )   6 2 set-field ;

\ Create a configuration byte
0
5 set-channel   \ channel 5
3 set-mode      \ mode 3
2 set-status    \ status 2
constant config

config get-channel .    \ 5
config get-mode .       \ 3
config get-status .     \ 2
```

## Two's Complement and Signed Numbers

### Understanding Two's Complement

In two's complement representation:
- Positive numbers: normal binary
- Negative numbers: flip all bits and add 1

```forth
\ Negate using two's complement
: twos-complement  ( n -- -n )
    invert 1+ ;

5 twos-complement .     \ -5
-5 twos-complement .    \ 5
```

### Checking Sign Bit

```forth
\ Check if number is negative (MSB set)
: negative?  ( n -- flag )
    0< ;

-10 negative? .     \ -1 (true)
10 negative? .      \ 0 (false)
```

## Practical Examples

### Example 1: Simple Checksum

```forth
: checksum  ( addr len -- checksum )
    0 -rot              \ Accumulator
    0 do
        over i + c@
        xor             \ XOR all bytes together
    loop
    nip ;

create data  1 c, 2 c, 3 c, 4 c, 5 c,
data 5 checksum .   \ Simple XOR checksum
```

### Example 2: Count Set Bits (Population Count)

```forth
: count-bits  ( n -- count )
    0 swap              \ count n
    begin dup while
        dup 1 and rot + swap
        1 rshift
    repeat
    drop ;

%10110110 count-bits .  \ 5
255 count-bits .        \ 8
```

### Example 3: Reverse Bits

```forth
: reverse-bits  ( n -- n-reversed )
    0 swap                  \ result n
    8 0 do                  \ For 8 bits
        1 lshift            \ Shift result left
        over 1 and or       \ Add lowest bit of n
        swap 1 rshift swap  \ Shift n right
    loop
    nip ;

%10110001 reverse-bits
2 base ! . decimal      \ 10001101
```

### Example 4: Binary to BCD

```forth
\ Convert binary to BCD (Binary Coded Decimal)
: bin>bcd  ( n -- bcd )
    dup 10 / 4 lshift   \ Tens digit in upper nibble
    swap 10 mod or ;    \ Ones digit in lower nibble

42 bin>bcd hex . decimal    \ 42 (looks the same in hex!)
99 bin>bcd hex . decimal    \ 99
```

## Bit Manipulation Patterns

### Pattern 1: Check if Power of 2

```forth
: power-of-2?  ( n -- flag )
    dup 1- and 0= ;

8 power-of-2? .     \ -1 (true: 1000 AND 0111 = 0)
7 power-of-2? .     \ 0 (false: 0111 AND 0110 ≠ 0)
```

### Pattern 2: Round Up to Power of 2

```forth
: next-power-of-2  ( n -- power )
    1- dup
    1 rshift or dup
    2 rshift or dup
    4 rshift or dup
    8 rshift or dup
    16 rshift or
    1+ ;

100 next-power-of-2 .   \ 128
```

### Pattern 3: Swap Nibbles

```forth
: swap-nibbles  ( byte -- byte' )
    dup 4 rshift $0F and
    swap $0F and 4 lshift
    or ;

$AB swap-nibbles hex . decimal  \ BA
```

### Pattern 4: Isolate Rightmost 1-bit

```forth
: rightmost-1  ( n -- bit )
    dup negate and ;

%10110000 rightmost-1
2 base ! . decimal      \ 10000
```

## Common Mistakes

### 1. Forgetting to Mask

```forth
\ WRONG: Not masking can give unexpected results
: bad-extract
    8 rshift ;      \ Doesn't remove upper bits!

\ RIGHT: Always mask after shift
: good-extract
    8 rshift $FF and ;
```

### 2. Sign Extension

```forth
\ Be careful with signed right shifts
\ Most Forths do arithmetic right shift (sign extension)

-1 1 rshift .       \ Still -1 (sign bit copied)
```

### 3. Shift Amount Too Large

```forth
\ Shifting by more than cell width is undefined
1 64 lshift .       \ Undefined behavior on 64-bit
```

## Exercises

1. Write a word that swaps the upper and lower bytes of a 16-bit value
2. Implement a word that counts trailing zeros
3. Create a bit-rotation operation (rotate left/right)
4. Write a word to extract the middle 4 bits of a byte
5. Implement a parity checker (odd/even number of 1-bits)
6. Create a word that sets a range of bits (e.g., bits 3-5)
7. Write a CRC-8 calculator
8. Implement Gray code conversion (binary ↔ Gray)

## Solutions

### 1. Swap Bytes

```forth
: swap-bytes  ( n16 -- n16' )
    dup 8 lshift $FF00 and
    swap 8 rshift $00FF and
    or ;

$1234 swap-bytes hex . decimal  \ 3412
```

### 2. Count Trailing Zeros

```forth
: count-trailing-zeros  ( n -- count )
    0 swap
    begin dup 1 and 0= while
        swap 1+ swap
        1 rshift
    repeat
    drop ;

%1000 count-trailing-zeros .    \ 3
12 count-trailing-zeros .       \ 2 (%1100)
```

### 3. Bit Rotation

```forth
: rotate-left  ( n count bits -- n' )
    >r over r@ lshift           \ High bits
    -rot r@ swap - rshift       \ Low bits
    1 r> lshift 1- and          \ Mask to bit width
    or ;

%10110001 3 8 rotate-left
2 base ! . decimal              \ 10001101
```

### 4. Extract Middle 4 Bits

```forth
: middle-4-bits  ( byte -- nibble )
    2 rshift $0F and ;

%11011010 middle-4-bits
2 base ! . decimal              \ 0110
```

### 5. Parity Checker

```forth
: parity  ( n -- flag )
    0 swap
    begin dup while
        dup 1 and rot xor swap
        1 rshift
    repeat
    drop ;

%10110110 parity .  \ -1 (odd parity)
%10110111 parity .  \ 0 (even parity)
```

## Quick Reference

| Operation | Word | Example | Result |
|-----------|------|---------|--------|
| AND | `and` | `%1100 %1010 and` | `%1000` |
| OR | `or` | `%1100 %1010 or` | `%1110` |
| XOR | `xor` | `%1100 %1010 xor` | `%0110` |
| NOT | `invert` | `%1010 invert` | All bits flipped |
| Left shift | `lshift` | `1 3 lshift` | `8` |
| Right shift | `rshift` | `8 3 rshift` | `1` |
| Set bit n | `1 swap lshift or` | `0 3 set-bit` | `8` |
| Clear bit n | `1 swap lshift invert and` | `15 2 clear-bit` | `11` |
| Test bit n | `1 swap lshift and 0<>` | `12 2 test-bit` | `-1` |
| Toggle bit n | `1 swap lshift xor` | `12 1 toggle-bit` | `14` |

## What's Next?

Now that you understand bit manipulation, you're ready for:
- **Tutorial 11: File I/O** - Reading and writing binary files
- **Tutorial 12: Building an Interpreter** - Implementing a stack-based VM

Bit manipulation is fundamental to systems programming, embedded development, and low-level optimization. These skills transfer directly to C, assembly language, and hardware interfacing!
