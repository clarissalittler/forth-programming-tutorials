\ memory-explorer.fs - Explore Forth's dictionary and memory

." Memory Architecture Explorer" cr
." =============================" cr cr

\ Show current dictionary position
: show-here  ( -- )
    ." Dictionary pointer (HERE): " here . cr
;

." Initial dictionary state:" cr
show-here
cr

." Creating a variable..." cr
here
variable test-var
here swap -
." Variable allocates: " . ." bytes" cr
cr

." Creating a constant..." cr
here
42 constant test-const
here swap -
." Constant allocates: " . ." bytes" cr
cr

\ Demonstrate CREATE and comma
." Creating an array with CREATE and ," cr
create numbers 10 , 20 , 30 , 40 , 50 ,

: show-numbers  ( -- )
    ." Array contents: "
    numbers 5 cells bounds do
        i @ .
    cell +loop
    cr
;

show-numbers
cr

\ Create a custom defining word
: array  ( n "name" -- )
    create cells allot
    does> ( index -- addr )
        swap cells +
;

." Creating arrays using custom defining word:" cr
5 array scores

." Setting scores..." cr
100 0 scores !
95 1 scores !
87 2 scores !

: show-scores  ( -- )
    ." Scores: "
    3 0 do
        i scores @ .
    loop
    cr
;

show-scores
cr

\ Show CREATE...DOES> with a point structure
: point  ( x y "name" -- )
    create , ,
    does> ( -- addr )
;

10 20 point p1
30 40 point p2

: point-x  ( point -- x )  @ ;
: point-y  ( point -- y )  cell+ @ ;

." Point p1: (" p1 point-x . ." , " p1 point-y . ." )" cr
." Point p2: (" p2 point-x . ." , " p2 point-y . ." )" cr
cr

." Final dictionary state:" cr
show-here

bye
