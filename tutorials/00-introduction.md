# Tutorial 0: Introduction to Forth

## What is Forth?

Forth is a unique programming language that's been around since the 1970s. Unlike most languages you may have encountered, Forth is:

- **Stack-based**: All operations work with a data stack
- **Concatenative**: Programs are built by composing simple operations
- **Interactive**: You can test every piece of code immediately
- **Extensible**: You build the language as you program
- **Minimal**: The core language is incredibly simple

Forth has been used in:
- Spacecraft and satellites (including NASA missions)
- Embedded systems and microcontrollers
- Robotics and industrial control
- Boot firmware (OpenBoot on Sun systems)
- Scientific instruments

## Why Learn Forth?

1. **Understand computation at a fundamental level** - Forth strips away abstractions and shows you how computers really work
2. **Think differently** - Forth's paradigm will change how you think about problem-solving
3. **Rapid prototyping** - The interactive nature lets you build and test incrementally
4. **Embedded systems** - Forth is perfect for resource-constrained environments
5. **Historical significance** - Understanding Forth helps you appreciate language design

## Key Concepts

### The Stack

The stack is Forth's fundamental data structure. Think of it as a stack of plates - you can only add (push) or remove (pop) from the top.

When you type a number in Forth, it goes on the stack. When you execute an operation, it takes values from the stack and puts results back on the stack.

### Words

In Forth, everything is a "word". Words are like functions or commands. Some words push numbers onto the stack, others perform operations, and you can define your own words.

### Reverse Polish Notation (RPN)

Forth uses postfix notation, also known as RPN. Instead of writing `2 + 3`, you write `2 3 +`.

The operands come first, then the operator.

## Your First Forth Session

Let's start gforth and try some basic operations.

### Starting gforth

Open your terminal and type:
```bash
gforth
```

You should see something like:
```
Gforth 0.7.3, Copyright (C) 1995-2008 Free Software Foundation, Inc.
Gforth comes with ABSOLUTELY NO WARRANTY; for details type `license'
Type `bye' to exit
```

The cursor is now waiting for your input.

### Simple Arithmetic

Let's add two numbers. Type:
```forth
2 3 + .
```

Press Enter. You should see `5` printed.

Let's break this down:
- `2` pushes the number 2 onto the stack
- `3` pushes the number 3 onto the stack
- `+` pops two numbers, adds them, and pushes the result (5)
- `.` pops a number from the stack and prints it

Try these examples (type each line and press Enter):

```forth
10 7 - .     \ Should print 3
6 7 * .      \ Should print 42
15 3 / .     \ Should print 5
```

Note: The backslash `\` starts a comment - everything after it on that line is ignored.

### Seeing the Stack

Type:
```forth
5 10 15
```

Now type:
```forth
.s
```

You should see something like:
```
<3> 5 10 15
```

This shows you what's on the stack. The `<3>` means there are 3 items, and they're listed from bottom to top.

The `.s` word is very helpful for debugging - it shows the stack without modifying it.

Now let's clear the stack by printing each value:
```forth
. . .
```

Each `.` prints one number and removes it from the stack.

### Chaining Operations

Forth's power comes from chaining words together. Try:

```forth
2 3 + 4 * .     \ (2+3) * 4 = 20
```

This:
1. Pushes 2
2. Pushes 3
3. Adds them (stack now has 5)
4. Pushes 4
5. Multiplies (5 * 4 = 20)
6. Prints 20

### Exiting gforth

Type:
```forth
bye
```

This exits the gforth interpreter.

## Your First Forth Program

Let's create a simple Forth script file.

Create a file called `hello.fs`:

```forth
\ hello.fs - My first Forth program

." Hello, Forth!" cr
bye
```

Run it:
```bash
gforth hello.fs
```

You should see:
```
Hello, Forth!
```

Let's understand this:
- `#!/usr/bin/env gforth` - Shebang line for direct execution
- `\` - Comment
- `."` - Print a string (note the space after the `."`)
- `cr` - Carriage return (newline)
- `bye` - Exit the interpreter

## Common Beginner Mistakes

1. **Forgetting spaces** - `2 3+.` won't work. Forth needs spaces between words.
2. **Wrong order** - Remember it's `2 3 +`, not `+ 2 3`
3. **Stack underflow** - Trying to pop from an empty stack causes an error
4. **Forgetting `.`** - Numbers go on the stack but don't print unless you use `.`

## Interactive Practice

Start gforth and try these exercises:

1. Calculate `(5 + 3) * 2`
   ```forth
   5 3 + 2 * .
   ```

2. Calculate `(10 - 2) / 4`
   ```forth
   10 2 - 4 / .
   ```

3. Calculate `(6 + 2) * (7 - 3)`
   ```forth
   6 2 + 7 3 - * .
   ```

4. Use `.s` to see the stack after pushing 1, 2, and 3
   ```forth
   1 2 3 .s
   ```

## Exercises

1. Calculate the result of `(15 + 25) * 3` using Forth
2. Create a Forth script that prints your name
3. Calculate `100 - 25 + 13 * 2` (remember order of operations!)
4. Push five numbers onto the stack, use `.s` to view them, then print them all

## Solutions

1.
```forth
15 25 + 3 * .   \ Prints 120
```

2. Create `name.fs`:
```forth
#!/usr/bin/env gforth
." My name is [Your Name]" cr
bye
```

3.
```forth
\ Note: In Forth, operations are evaluated left to right
\ So this is (100 - 25) + (13 * 2) = 75 + 26 = 101
100 25 - 13 2 * + .

\ If you wanted (100 - 25 + 13) * 2, you'd do:
100 25 - 13 + 2 * .   \ Prints 176
```

4.
```forth
10 20 30 40 50 .s
. . . . .
```

## What's Next?

Now that you understand the basics of Forth - the stack, words, and RPN notation - you're ready to dive deeper.

In [Tutorial 1: The Stack and Basic Arithmetic](01-stack-arithmetic.md), we'll explore:
- More arithmetic operations
- How the stack really works
- Stack depth and management
- Number bases (hex, binary, decimal)

## Quick Reference

| Word | Description | Example |
|------|-------------|---------|
| `.` | Print top of stack | `5 .` � prints 5 |
| `.s` | Show stack contents | `.s` � `<3> 1 2 3` |
| `+` | Addition | `2 3 + .` � 5 |
| `-` | Subtraction | `10 3 - .` � 7 |
| `*` | Multiplication | `4 5 * .` � 20 |
| `/` | Division | `15 3 / .` � 5 |
| `."` | Print string | `." Hi" cr` � Hi |
| `cr` | Carriage return (newline) | `." Hello" cr` |
| `bye` | Exit interpreter | `bye` |
| `\` | Comment to end of line | `5 \ this is five` |
