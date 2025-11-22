\ starter.fs - RPN Calculator Starter Template
\ Fill in the implementation for each word

\ ============================================
\ PHASE 1: Basic Calculator
\ ============================================

\ Display prompt
: prompt  ( -- )
    ." > "
;

\ Print help message
: show-help  ( -- )
    cr
    ." Commands:" cr
    ." Operators: + - * / " cr
    ." Stack: .s clear" cr
    ." Other: . help quit" cr
;

\ Clear the stack (remove all items)
: clear  ( ... -- )
    \ TODO: Implement clear stack
    \ Hint: Use depth to find how many items, then drop them all
;

\ Check if a string is a number and convert it
\ Returns: number and -1 if success, or 0 if not a number
: try-number  ( addr len -- n -1 | 0 )
    \ TODO: Implement number parsing
    \ Hint: Look at >NUMBER or use 0. to parse
    0 0 2swap >number 0=
;

\ Execute an operator
: do-operator  ( addr len -- )
    \ TODO: Implement operator dispatch
    \ Hint: Use 2dup s" +" compare 0= to check for each operator
    \ Example:
    \ 2dup s" +" compare 0= if 2drop + exit then
    \ 2dup s" -" compare 0= if 2drop - exit then
    \ ...
    2drop ." Unknown operator" cr
;

\ Execute a command (non-operator)
: do-command  ( addr len -- )
    \ TODO: Implement command dispatch for .s, clear, help, etc.
    2drop ." Unknown command" cr
;

\ Process one token (word) from input
: process-token  ( addr len -- )
    \ TODO: Implement token processing
    \ 1. Try to parse as number - if success, leave on stack
    \ 2. Check if it's an operator - execute it
    \ 3. Check if it's a command - execute it
    \ 4. Otherwise, print error
    2dup try-number if
        -rot 2drop          \ Keep number, drop string
    else
        \ Not a number, check for operator or command
        2drop ." Unknown token" cr
    then
;

\ Main calculator loop
: calculator  ( -- )
    cr
    ." RPN Calculator - Phase 1" cr
    ." Type 'help' for commands" cr
    cr

    \ TODO: Implement the main loop
    \ 1. Show prompt
    \ 2. Read input (use REFILL and PARSE-NAME)
    \ 3. Process each token
    \ 4. Repeat until 'quit'

    ." Calculator not yet implemented" cr
    ." Implement the calculator word to get started!" cr
;

\ Start the calculator
\ Uncomment when ready:
\ calculator
\ bye
