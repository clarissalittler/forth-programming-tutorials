\ test-example.fs - Example of using the test framework

\ Load the test framework
s" ../lib/test-framework.fs" included

\ Define some words to test
: square  ( n -- n² )
    dup * ;

: is-even?  ( n -- flag )
    2 mod 0= ;

: factorial  ( n -- n! )
    dup 1 <= if
        drop 1
    else
        dup 1- recurse *
    then ;

: average  ( a b -- avg )
    + 2 / ;

\ Test Suite 1: Arithmetic Operations
s" Arithmetic Operations" describe

s" square should compute n²" it
5 square 25 assert-equal

s" square of zero should be zero" it
0 square 0 assert-equal

s" square of negative should be positive" it
-3 square 9 assert-equal

\ Test Suite 2: Boolean Operations
s" Boolean Operations" describe

s" even numbers should return true" it
4 is-even? assert-true

s" odd numbers should return false" it
5 is-even? assert-false

s" zero should be even" it
0 is-even? assert-true

\ Test Suite 3: Factorial
s" Factorial Function" describe

s" factorial of 0 should be 1" it
0 factorial 1 assert-equal

s" factorial of 1 should be 1" it
1 factorial 1 assert-equal

s" factorial of 5 should be 120" it
5 factorial 120 assert-equal

s" factorial of 10 should be 3628800" it
10 factorial 3628800 assert-equal

\ Test Suite 4: Average
s" Average Function" describe

s" average of equal numbers" it
10 10 average 10 assert-equal

s" average of 0 and 10" it
0 10 average 5 assert-equal

s" average of 100 and 200" it
100 200 average 150 assert-equal

\ Test Suite 5: Comparison Tests
s" Comparison Assertions" describe

s" less than comparison" it
3 5 assert<

s" greater than comparison" it
10 5 assert>

s" not equal comparison" it
42 99 assert<>

\ Display results
.test-results

bye
