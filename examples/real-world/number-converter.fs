\ number-converter.fs - Convert between number bases
\ Demonstrates: Number bases, digit extraction, formatting

." Number Base Converter" cr
." =====================" cr cr

\ Convert decimal to binary string representation
: .binary  ( n -- )
    2 base !
    dup .
    decimal ;

\ Convert to hex
: .hex  ( n -- )
    hex
    dup .
    decimal ;

\ Convert to octal
: .octal  ( n -- )
    8 base !
    dup .
    decimal ;

\ Print number in all bases
: show-all-bases  ( n -- )
    ." Decimal: " dup . cr
    ." Hexadecimal: $" dup .hex cr
    ." Binary: %" dup .binary cr
    ." Octal: " .octal cr ;

\ Examples
." Example 1: 42" cr
42 show-all-bases
cr

." Example 2: 255" cr
255 show-all-bases
cr

." Example 3: 1024" cr
1024 show-all-bases
cr

: show-powers-of-2  ( -- )
    ." Powers of 2:" cr
    ." -----------" cr
    10 0 do
        1 i lshift dup
        ." 2^" i . ." = " dup .
        ."  = $" dup .hex
        ."  = %" .binary cr
    loop
    cr ;

show-powers-of-2

." Bit patterns:" cr
." ------------" cr
." $FF (255) = 11111111 (all bits set)" cr
." $80 (128) = 10000000 (MSB only)" cr
." $01 (1)   = 00000001 (LSB only)" cr
." $AA (170) = 10101010 (alternating)" cr
." $55 (85)  = 01010101 (alternating)" cr
cr

." Common prefixes:" cr
." - $ or 0x for hexadecimal (base 16)" cr
." - % or 0b for binary (base 2)" cr
." - 0 for octal (base 8)" cr
." - No prefix for decimal (base 10)" cr

bye
