\ prime-checker.fs - Check if a number is prime

: is-prime?  ( n -- flag )
    dup 2 < IF
        drop 0          \ Numbers < 2 are not prime
    ELSE
        dup 2 = IF
            drop -1     \ 2 is prime
        ELSE
            -1 swap     \ Assume prime (flag)
            dup 2 DO
                dup I mod 0= IF
                    drop 0 LEAVE    \ Found divisor, not prime
                THEN
            LOOP
            swap drop
        THEN
    THEN
;

: check-prime  ( n -- )
    dup is-prime? IF
        . ." is prime" cr
    ELSE
        . ." is not prime" cr
    THEN
;

: primes-up-to  ( n -- )
    ." Primes up to " dup . ." :" cr
    2 + 2 DO
        I is-prime? IF
            I . cr
        THEN
    LOOP
;

." Testing prime numbers:" cr
7 check-prime
9 check-prime
17 check-prime
100 check-prime

cr
30 primes-up-to

bye
