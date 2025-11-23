# Forth Programming Exercises

This directory contains programming exercises (katas) to practice your Forth skills. Each kata focuses on specific concepts and gradually increases in difficulty.

## How to Use These Exercises

1. Read the kata description
2. Try to solve it yourself without looking at the solution
3. Test your solution
4. Compare with the provided solution
5. Understand why the solution works

## Exercise Categories

### Katas (Progressive Challenges)
Small, focused exercises that build specific skills. Located in `katas/`.

### Project Exercises
Larger projects that combine multiple concepts. Located in `../projects/`.

## Kata Difficulty Levels

- 🟢 **Beginner**: Basic stack operations, simple logic
- 🟡 **Intermediate**: Multiple concepts, requires planning
- 🔴 **Advanced**: Complex algorithms, optimization

## Kata List

### Stack Manipulation (Beginner 🟢)
1. **Reverse Three** - Reverse order of top 3 stack items
2. **Fourth Item** - Access 4th item on stack
3. **Minimum of Three** - Find smallest of 3 numbers

### Arithmetic (Beginner 🟢)
4. **Power** - Compute n^m
5. **GCD** - Greatest common divisor
6. **LCM** - Least common multiple

### Logic & Conditionals (Intermediate 🟡)
7. **Leap Year** - Determine if year is leap year
8. **FizzBuzz** - Classic FizzBuzz (1-100)
9. **Grade Calculator** - Convert score to letter grade

### Loops & Iteration (Intermediate 🟡)
10. **Sum of Squares** - Sum of squares from 1 to n
11. **Fibonacci** - nth Fibonacci number
12. **Prime Checker** - Test if number is prime

### Arrays & Memory (Intermediate 🟡)
13. **Array Reverse** - Reverse an array in place
14. **Find Maximum** - Find largest element in array
15. **Array Sort** - Implement bubble sort

### Strings (Intermediate 🟡)
16. **Palindrome** - Check if string is palindrome
17. **String Reverse** - Reverse a string
18. **Count Vowels** - Count vowels in string

### Bit Manipulation (Advanced 🔴)
19. **Binary to Decimal** - Convert binary string to decimal
20. **Count Set Bits** - Count 1s in binary representation
21. **Power of 2** - Check if number is power of 2

### Algorithms (Advanced 🔴)
22. **Binary Search** - Search sorted array
23. **Quicksort** - Implement quicksort
24. **Balanced Parentheses** - Check if parens are balanced

### Data Structures (Advanced 🔴)
25. **Stack Implementation** - Implement stack with push/pop
26. **Queue Implementation** - Implement FIFO queue
27. **Linked List** - Basic linked list operations

## Using the Test Framework

Many katas include tests. Run them like this:

```forth
\ Include the test framework
s" ../lib/test-framework.fs" included

\ Your solution here
: my-solution  ( ... -- ... )
    \ ...
;

\ Run tests
s" My Solution Tests" describe

s" should handle basic case" it
\ test code
assert-equal

.test-results
bye
```

## Tips for Success

1. **Start simple** - Get basic case working first
2. **Test incrementally** - Test after each small change
3. **Use `.s`** - Check stack constantly
4. **Read solutions only after trying** - Learn more by struggling
5. **Understand, don't memorize** - Focus on concepts
6. **Time yourself** - Track improvement over time

## Suggested Learning Path

### Week 1: Basics
- Complete all Beginner (🟢) katas
- Focus on stack manipulation and arithmetic
- Goal: Comfort with basic Forth operations

### Week 2: Intermediate
- Start Intermediate (🟡) katas
- Complete FizzBuzz, Fibonacci, Prime Checker
- Goal: Handle loops and conditionals confidently

### Week 3: Advanced
- Tackle Advanced (🔴) katas
- Implement at least one sorting algorithm
- Goal: Understand complex control flow

### Week 4: Projects
- Choose a project from `../projects/`
- Apply kata skills to larger program
- Goal: Build complete application

## Additional Challenges

Once you complete the katas, try these:

### Code Golf
Solve katas in fewest words possible while maintaining readability.

### Performance
Optimize solutions for speed. Benchmark with test framework.

### Alternative Solutions
Find multiple ways to solve each kata.

### Explain to Others
Best way to learn: explain your solution to someone else.

## Resources

- [Quick Reference](../docs/quick-reference.md) - Forth command reference
- [Common Patterns](../docs/common-patterns.md) - Idioms and best practices
- [Debugging Guide](../docs/debugging-guide.md) - When you get stuck

## Contributing

Create your own katas! Good katas:
- Focus on one concept
- Have clear requirements
- Include test cases
- Provide learning value

Happy coding! Remember: the struggle is where the learning happens. 🚀
