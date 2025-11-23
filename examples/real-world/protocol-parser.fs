\ protocol-parser.fs - Simple Binary Protocol Parser
\ Demonstrates: Binary data structures, bit fields, protocol parsing

." Binary Protocol Parser" cr
." =======================" cr cr

\ Simple packet format (8 bytes total):
\ Byte 0: Protocol version (4 bits) + Packet type (4 bits)
\ Byte 1: Flags (8 bits)
\ Bytes 2-3: Payload length (16-bit big-endian)
\ Bytes 4-7: Payload data (or header continuation)

\ Define packet structure
create packet-buffer 8 allot

\ Accessor words
: packet-version  ( -- version )
    packet-buffer c@ 4 rshift ;

: packet-type  ( -- type )
    packet-buffer c@ $0F and ;

: packet-flags  ( -- flags )
    packet-buffer 1 + c@ ;

: packet-length  ( -- length )
    packet-buffer 2 + c@          \ High byte
    8 lshift
    packet-buffer 3 + c@ or ;     \ Low byte

\ Flag bit positions
1 constant FLAG-ACK
2 constant FLAG-SYNC
4 constant FLAG-FIN
8 constant FLAG-PRIORITY

\ Check if a flag is set
: flag-set?  ( flag -- ? )
    packet-flags and 0<> ;

\ Packet type names
: type-name  ( type -- )
    case
        0 of ." DATA" endof
        1 of ." ACK" endof
        2 of ." SYNC" endof
        3 of ." PING" endof
        4 of ." PONG" endof
        5 of ." CLOSE" endof
        ." UNKNOWN"
    endcase ;

\ Display packet information
: .packet  ( -- )
    ." Packet Analysis:" cr
    ." ================" cr
    ."   Version: " packet-version . cr
    ."   Type: " packet-type dup type-name ."  (" . ." )" cr
    ."   Flags: $" packet-flags hex . decimal cr
    ."     ACK:      " FLAG-ACK flag-set? if ." Yes" else ." No" then cr
    ."     SYNC:     " FLAG-SYNC flag-set? if ." Yes" else ." No" then cr
    ."     FIN:      " FLAG-FIN flag-set? if ." Yes" else ." No" then cr
    ."     PRIORITY: " FLAG-PRIORITY flag-set? if ." Yes" else ." No" then cr
    ."   Length: " packet-length . ." bytes" cr ;

\ Create a packet
: make-packet  ( version type flags length -- )
    dup packet-buffer 3 + c!          \ Length low byte
    8 rshift packet-buffer 2 + c!     \ Length high byte
    packet-buffer 1 + c!              \ Flags
    $0F and                           \ Mask type to 4 bits
    swap 4 lshift or                  \ Combine version and type
    packet-buffer c! ;                \ Store first byte

\ Example 1: Simple DATA packet
." Example 1: Simple DATA packet" cr
2 0 0 256 make-packet  \ Version 2, Type DATA, No flags, 256 bytes
.packet
cr

\ Example 2: ACK packet with PRIORITY flag
." Example 2: ACK packet with PRIORITY flag" cr
2 1 FLAG-ACK FLAG-PRIORITY or 0 make-packet
.packet
cr

\ Example 3: SYNC packet with multiple flags
." Example 3: SYNC packet with FIN and SYNC flags" cr
2 2 FLAG-SYNC FLAG-FIN or 512 make-packet
.packet
cr

\ Example 4: Parse raw bytes
." Example 4: Parse raw packet bytes" cr
\ Manually create a packet: Version 3, Type PING, ACK+PRIORITY, Length 1024
$33 packet-buffer c!           \ Version 3 (0011) + Type 3 (0011)
FLAG-ACK FLAG-PRIORITY or packet-buffer 1 + c!
4 packet-buffer 2 + c!         \ High byte of 1024
0 packet-buffer 3 + c!         \ Low byte of 1024
.packet
cr

\ Demonstrate bit masking
." Bit Field Extraction:" cr
." =====================" cr
." First byte: $" packet-buffer c@ hex . decimal cr
." Version (upper 4 bits): " packet-version . cr
." Type (lower 4 bits): " packet-type . cr
cr

\ Real-world applications
." Real-World Applications:" cr
." =======================" cr
." - Network protocols (TCP/IP, UDP, custom protocols)" cr
." - File formats (PNG, ZIP, ELF headers)" cr
." - Hardware communication (SPI, I2C, CAN bus)" cr
." - Embedded systems (sensor data packets)" cr
." - Game networking (multiplayer game packets)" cr
cr

." Key Concepts:" cr
." =============" cr
." 1. Bit fields pack multiple values in one byte" cr
." 2. Big-endian vs little-endian for multi-byte values" cr
." 3. Flag bits for boolean options" cr
." 4. Masking with AND to extract bit fields" cr
." 5. Shifting to position bit fields" cr

bye
