\ factorial.fs - Calculate factorial

: factorial  ( n -- n! )
    dup 1 <= IF
        drop 1
    ELSE
        1 swap 1 + 1 DO
            I *
        LOOP
    THEN
;

: show-factorial  ( n -- )
    dup . ." ! = " factorial . cr
;

." Factorials:" cr
5 show-factorial
10 show-factorial
12 show-factorial

bye
