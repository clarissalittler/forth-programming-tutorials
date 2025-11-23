\ state-machine.fs - Traffic Light State Machine
\ Demonstrates: State machines, execution tokens, dispatch tables

." Traffic Light State Machine" cr
." ============================" cr cr

\ State constants
0 constant STATE-RED
1 constant STATE-YELLOW
2 constant STATE-GREEN
3 constant STATE-WALK

\ Current state
variable current-state

\ Timing (in arbitrary units)
variable timer

\ State handler for RED
: red-state  ( -- )
    ." [RED LIGHT]    Cars STOP, pedestrians may cross" cr
    timer @ 1+ dup timer !
    5 >= if
        ." Timer expired, switching to GREEN" cr
        STATE-GREEN current-state !
        0 timer !
    then ;

\ State handler for YELLOW
: yellow-state  ( -- )
    ." [YELLOW LIGHT] Cars prepare to stop" cr
    timer @ 1+ dup timer !
    2 >= if
        ." Timer expired, switching to RED" cr
        STATE-RED current-state !
        0 timer !
    then ;

\ State handler for GREEN
: green-state  ( -- )
    ." [GREEN LIGHT]  Cars GO, pedestrians STOP" cr
    timer @ 1+ dup timer !
    7 >= if
        ." Timer expired, switching to YELLOW" cr
        STATE-YELLOW current-state !
        0 timer !
    then ;

\ State handler for WALK signal
: walk-state  ( -- )
    ." [WALK SIGNAL]  Pedestrians may cross" cr
    timer @ 1+ dup timer !
    5 >= if
        ." Timer expired, switching to GREEN" cr
        STATE-GREEN current-state !
        0 timer !
    then ;

\ State dispatch table
create state-table
    ' red-state ,
    ' yellow-state ,
    ' green-state ,
    ' walk-state ,

\ Execute current state
: run-state  ( -- )
    current-state @ cells state-table + @ execute ;

\ Initialize state machine
: init-traffic-light  ( -- )
    STATE-RED current-state !
    0 timer ! ;

\ Run state machine for N cycles
: simulate  ( n -- )
    ." Starting traffic light simulation..." cr
    ." ======================================" cr cr

    init-traffic-light

    0 do
        ." Cycle " i 1+ . ." : "
        run-state cr
    loop

    cr
    ." Simulation complete!" cr ;

\ Run simulation
20 simulate

cr
." State Machine Concepts:" cr
." ----------------------" cr
." - State: Current condition (RED, YELLOW, GREEN, WALK)" cr
." - Transitions: Rules for changing states" cr
." - Events: Timer expiration triggers transitions" cr
." - Dispatch Table: Array of execution tokens" cr
cr

." Real-world applications:" cr
." - Embedded systems (traffic lights, washing machines)" cr
." - Game AI (enemy behavior)" cr
." - Network protocols (TCP state machine)" cr
." - User interfaces (button states)" cr
." - Compilers (lexer/parser states)" cr

bye
