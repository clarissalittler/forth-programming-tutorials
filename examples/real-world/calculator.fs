\ calculator.fs - RPN Calculator
\ Demonstrates: Stack-based computation, operator precedence, practical tool

." RPN Calculator" cr
." ===============" cr cr

." This demonstrates Reverse Polish Notation (RPN) calculation" cr
." used in HP calculators and stack-based languages." cr
cr

\ RPN doesn't need parentheses - operations work on stack!
\ Infix:  (5 + 3) * 2 = 16
\ RPN:    5 3 + 2 * = 16

." Example 1: (5 + 3) * 2" cr
." RPN: 5 3 + 2 *" cr
5 3 + 2 *
." Result: " . cr
cr

\ More complex example
." Example 2: ((10 + 5) * 3) - 7" cr
." RPN: 10 5 + 3 * 7 -" cr
10 5 + 3 * 7 -
." Result: " . cr
cr

\ Division
." Example 3: 100 / 4" cr
." RPN: 100 4 /" cr
100 4 /
." Result: " . cr
cr

\ Modulo
." Example 4: 17 mod 5 (remainder)" cr
." RPN: 17 5 mod" cr
17 5 mod
." Result: " . cr
cr

\ Average of three numbers
." Example 5: Average of 10, 20, 30" cr
." RPN: 10 20 + 30 + 3 /" cr
10 20 + 30 + 3 /
." Result: " . cr
cr

\ Square (duplicate and multiply)
: square  ( n -- n*n )
    dup * ;

." Example 6: Square of 7" cr
." RPN: 7 square" cr
7 square
." Result: " . cr
cr

\ Pythagorean theorem: c = sqrt(a² + b²)
\ Note: Forth doesn't have built-in sqrt, so we'll show a² + b²
: pyth-squared  ( a b -- c² )
    square swap square + ;

." Example 7: Pythagorean (3, 4) -> 3² + 4²" cr
." RPN: 3 4 pyth-squared" cr
3 4 pyth-squared
." Result: " . ." (which is 5²)" cr
cr

\ Min and Max (using built-in min and max)
." Example 8: Min and Max" cr
." Min of 15 and 23: " 15 23 min . cr
." Max of 15 and 23: " 15 23 max . cr
cr

\ Absolute value (using built-in abs)
." Example 9: Absolute value" cr
." abs(-42): " -42 abs . cr
." abs(42):  " 42 abs . cr
cr

\ Clamp (limit value to range)
\ First ensures n >= min, then ensures result <= max
: clamp  ( n min max -- n' )
    >r max r> min ;

." Example 10: Clamp to range [0, 100]" cr
." clamp(-10, 0, 100): " -10 0 100 clamp . cr
." clamp(50, 0, 100):  " 50 0 100 clamp . cr
." clamp(150, 0, 100): " 150 0 100 clamp . cr
cr

\ Percentage
: percent  ( n percent -- result )
    100 */ ;

." Example 11: Percentages" cr
." 15% of 200: " 200 15 percent . cr
." 25% of 80:  " 80 25 percent . cr
cr

\ Tips
." RPN Calculator Tips:" cr
." ===================" cr
." 1. Enter numbers first, operators last" cr
." 2. No parentheses needed - stack handles order" cr
." 3. Use .s to see the stack anytime" cr
." 4. Each operator consumes inputs and produces output" cr
." 5. HP calculators (HP-48, HP-50g) use RPN" cr
cr

." Why RPN?" cr
." ========" cr
." - No operator precedence ambiguity" cr
." - No parentheses needed" cr
." - Efficient for calculators (less memory)" cr
." - Natural for stack-based computers" cr
." - Fewer keystrokes for complex calculations" cr

bye
