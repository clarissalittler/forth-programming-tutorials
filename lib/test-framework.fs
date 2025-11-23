\ test-framework.fs - Simple Unit Testing Framework for Forth
\ Usage: include lib/test-framework.fs at the start of your test file

\ Test statistics
variable test-count
variable pass-count
variable fail-count
variable suite-count

\ Current test name storage
256 constant max-test-name-len
create current-test-name max-test-name-len allot
variable current-test-name-len

\ Initialize test counters
: reset-tests  ( -- )
    0 test-count !
    0 pass-count !
    0 fail-count ! ;

\ Set current test name
: test-name  ( addr len -- )
    dup current-test-name-len !
    current-test-name swap cmove ;

\ Print test name if set
: .test-name  ( -- )
    current-test-name-len @ 0> if
        ."   Test: " current-test-name current-test-name-len @ type cr
    then ;

\ Basic assertions
: assert-true  ( flag "test-name" -- )
    1 test-count +!
    if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
    then ;

: assert-false  ( flag "test-name" -- )
    1 test-count +!
    0= if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
    then ;

: assert=  ( actual expected "test-name" -- )
    1 test-count +!
    = if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
    then ;

: assert<>  ( actual expected "test-name" -- )
    1 test-count +!
    <> if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
    then ;

: assert<  ( n1 n2 "test-name" -- )
    1 test-count +!
    < if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
    then ;

: assert>  ( n1 n2 "test-name" -- )
    1 test-count +!
    > if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
    then ;

\ Enhanced assertion with better error messages
: assert-equal  ( actual expected -- )
    1 test-count +!
    2dup = if
        2drop
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
        ."   Expected: " . cr
        ."   Actual:   " . cr
    then ;

\ String comparison assertion
: assert-str=  ( addr1 len1 addr2 len2 -- )
    1 test-count +!
    compare 0= if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
        ."   Strings not equal" cr
    then ;

\ Test that code throws an error
: assert-throws  ( xt -- )
    1 test-count +!
    catch if
        1 pass-count +!
        drop    \ Drop error code
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
        ."   Expected exception but none was thrown" cr
    then ;

\ Test suite management
: describe  ( addr len -- )
    1 suite-count +!
    cr
    ." ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" cr
    ." 📋 Test Suite: " type cr
    ." ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" cr ;

: it  ( addr len -- )
    test-name
    ." ▸ " current-test-name current-test-name-len @ type
    ."  ... " ;

\ Report results
: .test-results  ( -- )
    cr
    ." ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" cr
    ." 📊 Test Results" cr
    ." ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" cr
    ." Total Tests:  " test-count ? cr
    ." Passed:       " pass-count @ dup .
    ."  (" 100 pass-count @ * test-count @ / . ." %)" cr
    ." Failed:       " fail-count ? cr
    cr
    fail-count @ 0= if
        ." ✅ All tests passed!" cr
    else
        ." ❌ Some tests failed!" cr
    then
    ." ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" cr ;

\ Benchmark utilities
: ms-time  ( -- ms )
    utime drop 1000 / ;

: benchmark  ( xt iterations -- avg-ms )
    ms-time >r          \ Save start time
    0 do dup execute loop
    drop
    ms-time r> -        \ Calculate elapsed time
    swap / ;            \ Average per iteration

: .benchmark  ( xt iterations "name" -- )
    >r >r
    ." Benchmarking: " type cr
    r> r>
    2dup benchmark
    ." Average time: " . ." ms over " . ." iterations" cr ;

\ Stack depth checking
: assert-stack-depth  ( expected-depth -- )
    1 test-count +!
    depth = if
        1 pass-count +!
    else
        1 fail-count +!
        ." ✗ FAIL: " .test-name
        ."   Stack depth mismatch" cr
        ."   Current depth: " depth . cr
    then ;

\ Convenience word for running a test suite
: run-tests  ( -- )
    reset-tests
    \ Tests go here
;

\ Example usage template (commented out)
\ s" My Test Suite" describe
\
\ s" should add two numbers" it
\ 2 3 + 5 assert-equal
\
\ s" should handle zero" it
\ 0 5 + 5 assert-equal
\
\ .test-results
\ bye

cr
." ═══════════════════════════════════════════" cr
." Test Framework Loaded" cr
." ═══════════════════════════════════════════" cr
