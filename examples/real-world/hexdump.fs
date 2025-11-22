\ hexdump.fs - Display memory in hexadecimal format
\ Like the Unix hexdump command

." Hexdump Utility" cr
." ===============" cr cr

\ Create some test data
create test-data
    72 c,  101 c,  108 c,  108 c,  111 c,   \ "Hello"
    32 c,  87 c,  111 c,  114 c,  108 c,    \ " Worl"
    100 c,  33 c,  10 c,   0 c,    0 c,     \ "d!\n"
    255 c,  128 c,  64 c,   32 c,   16 c,   \ Binary data

\ Print a byte in hex with leading zero if needed
: .hex-byte  ( byte -- )
    hex
    dup 16 < if ." 0" then
    . space
    decimal ;

\ Print ASCII character or dot if non-printable
: .ascii  ( byte -- )
    dup 32 >= over 127 < and if
        emit
    else
        drop [char] . emit
    then ;

\ Print 16 bytes in hex + ASCII
: hexdump-line  ( addr -- )
    \ Print address
    dup hex . ." : " decimal

    \ Print 16 bytes in hex
    16 0 do
        dup i + c@ .hex-byte
        i 7 = if ." - " then  \ Separator after 8 bytes
    loop

    ."  | "

    \ Print ASCII representation
    16 0 do
        dup i + c@ .ascii
    loop

    ."  |" cr
    drop ;

\ Hexdump a memory region
: hexdump  ( addr len -- )
    ." Address  : 00 01 02 03 04 05 06 07 - 08 09 0A 0B 0C 0D 0E 0F  | ASCII          |" cr
    ." ---------|--------------------------------------------------|----------------|" cr

    16 / 1+ 0 do        \ Number of 16-byte lines
        dup hexdump-line
        16 +
    loop
    drop ;

." Dumping test data:" cr cr
test-data 20 hexdump

cr
." Dumping Forth dictionary (first 64 bytes):" cr cr
here 64 - 64 hexdump

bye
