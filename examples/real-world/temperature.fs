\ temperature.fs - Temperature Conversion Utility
\ Demonstrates: Arithmetic, fractional calculations, formatting

." Temperature Converter" cr
." ====================" cr cr

\ Convert Celsius to Fahrenheit: F = C * 9/5 + 32
: c->f  ( c -- f )
    9 * 5 / 32 + ;

\ Convert Fahrenheit to Celsius: C = (F - 32) * 5/9
: f->c  ( f -- c )
    32 - 5 * 9 / ;

\ Convert Celsius to Kelvin: K = C + 273
: c->k  ( c -- k )
    273 + ;

\ Convert Kelvin to Celsius: C = K - 273
: k->c  ( k -- c )
    273 - ;

\ Convert Fahrenheit to Kelvin
: f->k  ( f -- k )
    f->c c->k ;

\ Convert Kelvin to Fahrenheit
: k->f  ( k -- f )
    k->c c->f ;

\ Display all conversions for a Celsius temperature
: show-celsius  ( c -- )
    ." Celsius:    " dup . ." °C" cr
    ." Fahrenheit: " dup c->f . ." °F" cr
    ." Kelvin:     " c->k . ." K" cr ;

\ Display all conversions for a Fahrenheit temperature
: show-fahrenheit  ( f -- )
    ." Fahrenheit: " dup . ." °F" cr
    ." Celsius:    " dup f->c . ." °C" cr
    ." Kelvin:     " f->k . ." K" cr ;

\ Display all conversions for a Kelvin temperature
: show-kelvin  ( k -- )
    ." Kelvin:     " dup . ." K" cr
    ." Celsius:    " dup k->c . ." °C" cr
    ." Fahrenheit: " k->f . ." °F" cr ;

\ Examples
." Example 1: Water freezing point (0°C)" cr
0 show-celsius
cr

." Example 2: Water boiling point (100°C)" cr
100 show-celsius
cr

." Example 3: Room temperature (72°F)" cr
72 show-fahrenheit
cr

." Example 4: Absolute zero (0 K)" cr
0 show-kelvin
cr

." Example 5: Human body temperature (98.6°F ≈ 98°F)" cr
98 show-fahrenheit
cr

\ Common temperature reference points
." Reference Points:" cr
." ================" cr
." Absolute zero:    0 K = -273°C = -459°F" cr
." Water freezes:    273 K = 0°C = 32°F" cr
." Room temperature: 293 K = 20°C = 68°F" cr
." Body temperature: 310 K = 37°C = 98°F" cr
." Water boils:      373 K = 100°C = 212°F" cr
cr

\ Fun facts
." Fun Facts:" cr
." -40°C = -40°F (same in both scales!)" cr
." Celsius and Fahrenheit meet at -40 degrees" cr

bye
