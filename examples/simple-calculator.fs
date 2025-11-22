\ simple-calculator.fs - A simple RPN calculator demo

: calculate  ( a b op -- result )
    CASE
        '+' OF + ENDOF
        '-' OF - ENDOF
        '*' OF * ENDOF
        '/' OF / ENDOF
        ." Unknown operator" drop drop 0 swap
    ENDCASE
;

." Simple RPN Calculator Demo" cr
." =========================" cr
cr

." 10 + 5 = " 10 5 '+' calculate . cr
." 20 - 8 = " 20 8 '-' calculate . cr
." 6 * 7 = " 6 7 '*' calculate . cr
." 100 / 4 = " 100 4 '/' calculate . cr

cr
." More complex: (5 + 3) * (10 - 2)" cr
5 3 '+' calculate 10 2 '-' calculate '*' calculate
." Result: " . cr

bye
