# Project 1: RPN Calculator

## Difficulty: 🟢 Beginner

## Prerequisites
- Tutorials 0-5 (Introduction through Loops)
- Understanding of stack manipulation
- Basic input/output

## Overview

Build a Reverse Polish Notation (RPN) calculator that accepts user input and evaluates mathematical expressions. This project will help you master stack manipulation and understand how Forth itself works!

## Learning Goals

- Practice stack manipulation
- Handle user input
- Parse and interpret commands
- Manage program state
- Build a usable interactive tool

## Specification

### Phase 1: Basic Calculator (Minimum Viable Product)

Create a calculator that:
1. Accepts numbers and pushes them on the stack
2. Accepts operators: `+`, `-`, `*`, `/`
3. Prints the result with `.` command
4. Shows the stack with `.s` command
5. Has a `quit` command to exit

**Example session:**
```
> 5
ok
> 10
ok
> +
ok
> .
15 ok
> 3
ok
> *
ok
> .
45 ok
> quit
Goodbye!
```

### Phase 2: Enhanced Features

Add these features:

#### Stack Operations
- `drop` - Remove top item
- `dup` - Duplicate top item
- `swap` - Swap top two items
- `clear` - Empty the stack

#### More Operators
- `mod` - Modulo
- `abs` - Absolute value
- `negate` - Negate number
- `max` - Maximum of top two
- `min` - Minimum of top two

#### Memory
- `sto` - Store top value in memory
- `rcl` - Recall value from memory

**Example:**
```
> 42
ok
> sto
Stored: 42
> 100
ok
> rcl
ok
> +
ok
> .
142 ok
```

### Phase 3: Advanced Features (Optional)

#### Multi-word expressions
```
> 5 3 + 2 *
ok
> .
16 ok
```

#### Named constants
```
> 3.14159 pi
Defined pi
> 10 pi *
ok
> .
31 ok
```

#### History
```
> history
1: 5 3 +
2: 2 *
3: .
```

## Implementation Tips

### Getting Started

1. **Define a word to print the prompt:**
```forth
: prompt  ( -- )
    ." > " ;
```

2. **Create the main loop:**
```forth
: calculator  ( -- )
    begin
        prompt
        \ Read and process input
        \ ...
    again
;
```

3. **Handle input:** Use `word` or `parse` to read tokens

4. **Dispatch commands:** Use `CASE...ENDCASE` or IF/ELSE chains

### Parsing Strategy

```forth
: process-token  ( addr len -- )
    \ Check if it's a number
    \ Check if it's an operator
    \ Check if it's a command
    \ Otherwise, print error
;
```

### Error Handling

Handle these error conditions:
- Stack underflow (not enough operands)
- Division by zero
- Unknown command
- Invalid number format

## Testing Your Calculator

Test these cases:

**Basic arithmetic:**
```
5 3 + .         → 8
10 2 - .        → 8
6 7 * .         → 42
15 3 / .        → 5
```

**Complex expressions:**
```
2 3 + 4 * .     → 20
10 3 - 2 / .    → 3
```

**Stack operations:**
```
5 dup + .       → 10
3 4 swap - .    → 1
```

**Error cases:**
```
+               → Error: Stack underflow
10 0 /          → Error: Division by zero
foo             → Error: Unknown command
```

## Hints

<details>
<summary>Hint 1: Detecting numbers</summary>

Use `>number` to try converting a string to a number:
```forth
: is-number?  ( addr len -- n true | false )
    \ Try to convert to number
    \ Return number and true if successful
    \ Return false if not a number
;
```
</details>

<details>
<summary>Hint 2: Command dispatch</summary>

```forth
: handle-command  ( addr len -- )
    2dup s" +" compare 0= if 2drop + exit then
    2dup s" -" compare 0= if 2drop - exit then
    2dup s" *" compare 0= if 2drop * exit then
    \ ... more commands
    ." Unknown command: " type cr
;
```
</details>

<details>
<summary>Hint 3: Main loop structure</summary>

```forth
: calculator  ( -- )
    cr ." RPN Calculator" cr
    ." Type 'help' for help, 'quit' to exit" cr
    begin
        cr ." > "
        \ Read line
        \ Parse and execute
    again
;
```
</details>

## Solution

A complete solution is provided in `solution.fs`. Try to implement it yourself first!

## Extensions

Once your calculator works, try adding:

1. **Scientific functions:** `sqrt`, `pow`, `sin`, `cos`
2. **Number bases:** Hex input/output (0xFF, etc.)
3. **Variables:** User-defined variables
4. **Macros:** Record and replay sequences
5. **File input:** Read expressions from a file
6. **Stack visualization:** Show stack graphically

## Example Complete Session

```
RPN Calculator
Type 'help' for help, 'quit' to exit

> 10 20
ok

> .s
<2> 10 20

> +
ok

> 5 *
ok

> .
150 ok

> 100 sto
Stored: 100

> 50 rcl +
ok

> .
150 ok

> .s
<0>

> help
Commands:
  Operators: + - * / mod abs negate max min
  Stack: drop dup swap clear .s
  Memory: sto rcl
  Other: . help quit

> quit
Goodbye!
```

## Reflection Questions

After completing this project, consider:

1. How does RPN make calculation easier/harder than infix notation?
2. What are the advantages of a stack-based calculator?
3. How is your calculator similar to the Forth interpreter itself?
4. What would you need to add to make this a full programming language?

Good luck! Remember: build incrementally, test often, and use `.s` liberally!
