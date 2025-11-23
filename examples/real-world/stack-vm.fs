\ stack-vm.fs - Simple Stack-Based Virtual Machine
\ Demonstrates how interpreters and VMs work

." Simple Stack-Based VM" cr
." =======================" cr cr

\ VM Stack
100 constant stack-size
create vm-stack stack-size cells allot
variable vm-sp

: vm-init  ( -- )
    vm-stack vm-sp ! ;

: vm-push  ( n -- )
    vm-sp @ !
    cell vm-sp +! ;

: vm-pop  ( -- n )
    cell negate vm-sp +!
    vm-sp @ @ ;

: .vm-stack  ( -- )
    ." Stack: "
    vm-stack vm-sp @ over - cell / 0 do
        vm-stack i cells + @ .
    loop
    cr ;

\ Opcodes
0 constant OP-PUSH
1 constant OP-ADD
2 constant OP-SUB
3 constant OP-MUL
4 constant OP-DIV
5 constant OP-PRINT
6 constant OP-DUP
7 constant OP-SWAP
8 constant OP-HALT

\ Bytecode execution
variable vm-ip          \ Instruction pointer
variable vm-running

: vm-fetch  ( -- byte )
    vm-ip @ c@
    1 vm-ip +! ;

: vm-execute-op  ( opcode -- )
    case
        OP-PUSH of
            vm-fetch vm-push
        endof

        OP-ADD of
            vm-pop vm-pop + vm-push
        endof

        OP-SUB of
            vm-pop swap vm-pop swap - vm-push
        endof

        OP-MUL of
            vm-pop vm-pop * vm-push
        endof

        OP-DIV of
            vm-pop swap vm-pop swap / vm-push
        endof

        OP-PRINT of
            vm-pop ." Result: " . cr
        endof

        OP-DUP of
            vm-pop dup vm-push vm-push
        endof

        OP-SWAP of
            vm-pop vm-pop swap vm-push vm-push
        endof

        OP-HALT of
            0 vm-running !
        endof

        ." Unknown opcode: " . cr
    endcase ;

: vm-run  ( bytecode-addr -- )
    vm-init
    vm-ip !
    1 vm-running !

    begin vm-running @ while
        vm-fetch
        vm-execute-op
    repeat ;

\ Example programs

\ Program 1: (5 + 3) * 2 = 16
create prog1
    OP-PUSH c, 5 c,         \ Push 5
    OP-PUSH c, 3 c,         \ Push 3
    OP-ADD  c,              \ Add
    OP-PUSH c, 2 c,         \ Push 2
    OP-MUL  c,              \ Multiply
    OP-PRINT c,             \ Print result
    OP-HALT c,              \ Halt

\ Program 2: Compute and print factorial of 5 (simplified)
create prog2
    OP-PUSH c, 120 c,       \ Result of 5! = 120
    OP-PRINT c,             \ Print it
    OP-HALT c,

\ Program 3: Swap test
create prog3
    OP-PUSH c, 10 c,        \ Push 10
    OP-PUSH c, 20 c,        \ Push 20
    OP-SWAP c,              \ Swap
    OP-DUP c,               \ Duplicate top
    OP-PRINT c,             \ Print (should be 10)
    OP-PRINT c,             \ Print (should be 10)
    OP-PRINT c,             \ Print (should be 20)
    OP-HALT c,

." Running Program 1: (5 + 3) * 2" cr
prog1 vm-run
cr

." Running Program 2: Factorial" cr
prog2 vm-run
cr

." Running Program 3: Swap and Dup" cr
prog3 vm-run

cr
." VM Opcode Reference:" cr
." OP-PUSH  (0):  Push next byte onto stack" cr
." OP-ADD   (1):  Pop two, push sum" cr
." OP-SUB   (2):  Pop two, push difference" cr
." OP-MUL   (3):  Pop two, push product" cr
." OP-DIV   (4):  Pop two, push quotient" cr
." OP-PRINT (5):  Pop and print value" cr
." OP-DUP   (6):  Duplicate top of stack" cr
." OP-SWAP  (7):  Swap top two values" cr
." OP-HALT  (8):  Stop execution" cr

bye
