# Kata 05: Greatest Common Divisor (GCD)

**Difficulty:** 🟡 Intermediate
**Topics:** Algorithms, Loops, Recursion
**Time:** 15-20 minutes

## Problem

Implement Euclid's algorithm to find the greatest common divisor (GCD) of two positive integers.

The GCD of two numbers is the largest number that divides both of them evenly.

## Signature

```forth
: gcd  ( a b -- gcd )
```

## Examples

```forth
12 8 gcd .     \ 4
48 18 gcd .    \ 6
100 50 gcd .   \ 50
17 19 gcd .    \ 1 (coprime)
```

## Algorithm

**Euclid's Algorithm:**
1. If b = 0, return a
2. Otherwise, return gcd(b, a mod b)

**Example:** gcd(48, 18)
- gcd(48, 18) → gcd(18, 12) → gcd(12, 6) → gcd(6, 0) → 6

## Requirements

- Must handle any two positive integers
- Should use Euclid's algorithm
- Can be recursive or iterative

## Hints

<details>
<summary>Hint 1: Recursive Approach</summary>

Use the `recurse` keyword to call yourself:

```forth
: gcd  ( a b -- gcd )
    dup 0= if
        drop
    else
        \ ... recurse ...
    then ;
```

</details>

<details>
<summary>Hint 2: Iterative Approach</summary>

Use `begin...until` to loop until b is zero:

```forth
: gcd  ( a b -- gcd )
    begin
        dup 0<>
    while
        \ ... update a and b ...
    repeat
    drop ;
```

</details>

<details>
<summary>Hint 3: The Modulo Operation</summary>

The key is computing `a mod b` and swapping:
- `2dup mod` gives you `a b (a mod b)`
- Then rearrange for next iteration

</details>

## Test Your Solution

```forth
\ Load test framework if available
\ s" ../lib/test-framework.fs" included

: test-gcd
    s" GCD Tests" describe

    s" gcd(12, 8) = 4" it
    12 8 gcd 4 assert-equal

    s" gcd(48, 18) = 6" it
    48 18 gcd 6 assert-equal

    s" gcd(100, 50) = 50" it
    100 50 gcd 50 assert-equal

    s" gcd(17, 19) = 1" it
    17 19 gcd 1 assert-equal

    s" gcd(0, 5) = 5" it
    0 5 gcd 5 assert-equal

    .test-results ;
```

## Solution

<details>
<summary>Solution 1: Recursive</summary>

```forth
: gcd  ( a b -- gcd )
    dup 0= if
        drop
    else
        2dup mod
        swap drop
        recurse
    then ;
```

**Explanation:**
- `dup 0=` - Check if b is zero
- If yes: `drop` b and return a
- If no: compute `a mod b`, then `gcd(b, a mod b)`

**Trace for gcd(12, 8):**
```
gcd(12, 8)
  dup 0= → false
  2dup mod → 12 8 4
  swap drop → 8 4
  recurse → gcd(8, 4)
    dup 0= → false
    2dup mod → 8 4 0
    swap drop → 4 0
    recurse → gcd(4, 0)
      dup 0= → true
      drop → 4
```

</details>

<details>
<summary>Solution 2: Iterative</summary>

```forth
: gcd  ( a b -- gcd )
    begin
        dup 0<>
    while
        2dup mod
        swap drop
    repeat
    drop ;
```

**Explanation:**
- Loop while b ≠ 0
- Each iteration: replace (a, b) with (b, a mod b)
- When b = 0, a is the GCD

**Trace for gcd(12, 8):**
```
Start: 12 8
  mod: 12 8 4
  swap drop: 8 4
Loop: 8 4
  mod: 8 4 0
  swap drop: 4 0
Loop: 4 0
  0<> → false, exit loop
  drop: 4
```

</details>

<details>
<summary>Solution 3: Optimized</summary>

```forth
\ Most concise version
: gcd  ( a b -- gcd )
    begin ?dup while tuck mod repeat ;
```

**Explanation:**
- `?dup` - Duplicate if non-zero
- `tuck` - Copy b under a: `( a b -- b a b )`
- `mod` - Compute `a mod b`
- Repeat until b = 0

This is very idiomatic Forth!

</details>

## Related Problems

After solving GCD, try:
1. **LCM** (Least Common Multiple): `lcm(a,b) = (a*b) / gcd(a,b)`
2. **Simplify Fraction**: Divide numerator and denominator by their GCD
3. **Extended GCD**: Find x, y such that ax + by = gcd(a,b)

## Learn More

This kata teaches:
- Classic algorithm implementation
- Choosing between recursive vs iterative solutions
- Forth loop constructs (`begin...while...repeat`)
- Using `recurse` for recursion
- The elegance of stack-based computation

## Fun Fact

Euclid's algorithm (circa 300 BC) is one of the oldest algorithms still in common use!

## Next Steps

- Kata 06: Least Common Multiple (LCM)
- Kata 12: Prime Checker (uses similar concepts)
- Project: Fraction Calculator (uses GCD for simplification)
