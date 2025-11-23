# Kata 01: Reverse Three

**Difficulty:** 🟢 Beginner
**Topics:** Stack Manipulation
**Time:** 5-10 minutes

## Problem

Write a word `reverse3` that reverses the order of the top three items on the stack.

## Signature

```forth
: reverse3  ( a b c -- c b a )
```

## Examples

```forth
1 2 3 reverse3 .s    \ <3> 3 2 1
5 10 15 reverse3 .s  \ <3> 15 10 5
```

## Requirements

- Must work with any three numbers
- Should only use stack manipulation words
- Don't use variables or memory

## Hints

<details>
<summary>Hint 1</summary>

What stack word can you use to access the third item?

</details>

<details>
<summary>Hint 2</summary>

Think about rot and swap. How can you combine them?

</details>

<details>
<summary>Hint 3</summary>

Try working through it step by step:
- Start: `( a b c )`
- What if you rot? `( b c a )`
- Now what?

</details>

## Test Your Solution

```forth
\ Test cases
: test-reverse3
    clearstack
    1 2 3 reverse3
    3 = swap 2 = and swap 1 = and if
        ." Test 1: PASS" cr
    else
        ." Test 1: FAIL" cr
    then

    clearstack
    100 200 300 reverse3
    300 = swap 200 = and swap 100 = and if
        ." Test 2: PASS" cr
    else
        ." Test 2: FAIL" cr
    then ;

test-reverse3
```

## Solution

<details>
<summary>Click to reveal solution</summary>

```forth
: reverse3  ( a b c -- c b a )
    rot rot ;
```

**Explanation:**
- First `rot`: `( a b c )` → `( b c a )`
- Second `rot`: `( b c a )` → `( c a b )`
- Wait, that's not right!

Actually, the solution is:
```forth
: reverse3  ( a b c -- c b a )
    swap rot swap ;
```

**Step by step:**
- Start: `( a b c )`
- `swap`: `( a c b )`
- `rot`: `( c b a )` ✓

**Alternative solution:**
```forth
: reverse3  ( a b c -- c b a )
    -rot swap ;
```

Where `-rot` is `rot rot` (available in some Forth systems).

</details>

## Learn More

After completing this kata, you should understand:
- How `rot` moves items in groups of three
- How `swap` exchanges the top two items
- How to combine basic stack operations
- How to think about stack transformations

## Next Steps

Try these related katas:
- Kata 02: Fourth Item
- Kata 03: Minimum of Three
