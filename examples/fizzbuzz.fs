\ fizzbuzz.fs - Classic FizzBuzz implementation

: fizzbuzz  ( n -- )
    dup 15 mod 0= IF
        drop ." FizzBuzz"
    ELSE dup 3 mod 0= IF
        drop ." Fizz"
    ELSE dup 5 mod 0= IF
        drop ." Buzz"
    ELSE
        .
    THEN THEN THEN
    cr
;

: fizzbuzz-to-n  ( n -- )
    1 + 1 DO
        I fizzbuzz
    LOOP
;

." FizzBuzz from 1 to 20:" cr
20 fizzbuzz-to-n

bye
