\ geometry.fs - Geometric calculations

\ Define pi approximation
: pi  ( -- π-approx )
    314 100 /
;

\ Square a number
: square  ( n -- n² )
    dup *
;

\ Calculate circle area: A = πr²
: circle-area  ( radius -- area )
    square pi *
;

\ Calculate circle circumference: C = 2πr
: circle-circumference  ( radius -- circumference )
    2 * pi *
;

\ Test the words
." Circle with radius 10:" cr
." Area: " 10 circle-area . cr
." Circumference: " 10 circle-circumference . cr

bye
