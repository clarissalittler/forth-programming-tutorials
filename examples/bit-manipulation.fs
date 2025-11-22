\ bit-manipulation.fs - Demonstrate bit operations

." Bit Manipulation Examples" cr
." ==========================" cr cr

\ Helper to show binary
: .bin  ( n -- )
    2 base ! . decimal ;

\ Basic bitwise operations
." 1. Basic Bitwise Operations:" cr
." %1100 AND %1010 = " %1100 %1010 and .bin cr
." %1100 OR  %1010 = " %1100 %1010 or .bin cr
." %1100 XOR %1010 = " %1100 %1010 xor .bin cr
cr

\ Bit shifting
." 2. Bit Shifting:" cr
." 1 << 3 = " 1 3 lshift . cr
." 128 >> 3 = " 128 3 rshift . cr
cr

\ Bit manipulation utilities
: set-bit  ( value bit-pos -- value' )
    1 swap lshift or ;

: clear-bit  ( value bit-pos -- value' )
    1 swap lshift invert and ;

: test-bit  ( value bit-pos -- flag )
    1 swap lshift and 0<> ;

: toggle-bit  ( value bit-pos -- value' )
    1 swap lshift xor ;

." 3. Bit Manipulation:" cr
." Set bit 3 in 0: " 0 3 set-bit . cr
." Clear bit 2 in 15: " 15 2 clear-bit . cr
." Test bit 2 in 12: " 12 2 test-bit . cr
." Toggle bit 1 in 12: " 12 1 toggle-bit . cr
cr

\ RGB color manipulation
: get-red  ( color -- red )
    16 rshift $FF and ;

: get-green  ( color -- green )
    8 rshift $FF and ;

: get-blue  ( color -- blue )
    $FF and ;

: make-color  ( r g b -- color )
    $FF and
    swap $FF and 8 lshift or
    swap $FF and 16 lshift or ;

." 4. RGB Color (Orange = #FF8000):" cr
$FF8000 get-red ." Red: " . cr
$FF8000 get-green ." Green: " . cr
$FF8000 get-blue ." Blue: " . cr

." Make color from RGB(128, 64, 32): " 128 64 32 make-color hex . decimal cr
cr

\ Count set bits
: count-bits  ( n -- count )
    0 swap
    begin dup while
        dup 1 and rot + swap
        1 rshift
    repeat
    drop ;

." 5. Population Count (count 1-bits):" cr
." Bits in %10110110: " %10110110 count-bits . cr
." Bits in 255: " 255 count-bits . cr
cr

\ Check if power of 2
: power-of-2?  ( n -- flag )
    dup 1- and 0= ;

." 6. Power of 2 Check:" cr
." 8 is power of 2? " 8 power-of-2? . cr
." 7 is power of 2? " 7 power-of-2? . cr
cr

\ Bit flags example
0 constant FLAG-READY
1 constant FLAG-BUSY
2 constant FLAG-ERROR
3 constant FLAG-DEBUG

variable system-flags

: set-flag  ( flag-pos -- )
    1 swap lshift system-flags @ or system-flags ! ;

: flag?  ( flag-pos -- flag )
    1 swap lshift system-flags @ and 0<> ;

." 7. Bit Flags:" cr
FLAG-READY set-flag
FLAG-BUSY set-flag

." Ready flag: " FLAG-READY flag? . cr
." Busy flag: " FLAG-BUSY flag? . cr
." Error flag: " FLAG-ERROR flag? . cr
cr

\ Simple checksum
variable checksum-acc

: checksum  ( addr len -- checksum )
    0 checksum-acc !        \ Initialize accumulator
    0 do
        dup i + c@          \ Get byte at addr+i
        checksum-acc @ xor  \ XOR with accumulator
        checksum-acc !      \ Store result
    loop
    drop checksum-acc @ ;

create test-data  1 c, 2 c, 3 c, 4 c, 5 c,

." 8. XOR Checksum:" cr
." Checksum of {1,2,3,4,5}: " test-data 5 checksum . cr

bye
