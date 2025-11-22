# Forth Programming Tutorials

Welcome to this comprehensive tutorial series on programming in Forth using gforth!

## What is Forth?

Forth is a stack-based, concatenative programming language known for its simplicity, efficiency, and extensibility. It's been used in everything from embedded systems and firmware to space applications and scientific instruments. Forth gives you direct control over the machine while maintaining an elegant, minimalist design philosophy.

## About This Course

This is a hands-on, examples-based course designed to teach you Forth from the ground up. Each tutorial builds on the previous one, introducing new concepts with plenty of working examples you can try yourself.

## Prerequisites

- Basic understanding of programming concepts
- A terminal/command line environment
- gforth installed (see Setup below)

## Setup

### Installing gforth

**On Ubuntu/Debian:**
```bash
apt-get install gforth
```

**On macOS:**
```bash
brew install gforth
```

**On Arch Linux:**
```bash
pacman -S gforth
```

### Testing Your Installation

Run this command to verify gforth is working:
```bash
echo '2 3 + . bye' | gforth
```

You should see `5` in the output.

### Running gforth Interactively

Simply type:
```bash
gforth
```

To exit, type `bye` and press Enter.

### Running Forth Scripts

You can run the example files in this tutorial like this:
```bash
gforth filename.fs
```

## Tutorial Structure

1. **[Tutorial 0: Introduction to Forth](tutorials/00-introduction.md)** - Forth philosophy, basic concepts, and your first program
2. **[Tutorial 1: The Stack and Basic Arithmetic](tutorials/01-stack-arithmetic.md)** - Understanding the stack and doing calculations
3. **[Tutorial 2: Stack Manipulation](tutorials/02-stack-manipulation.md)** - Stack operations like dup, swap, drop, over, and rot
4. **[Tutorial 3: Defining Words](tutorials/03-defining-words.md)** - Creating your own functions with `:` and `;`
5. **[Tutorial 4: Conditionals and Logic](tutorials/04-conditionals.md)** - IF/THEN/ELSE and boolean operations
6. **[Tutorial 5: Loops and Iteration](tutorials/05-loops.md)** - DO/LOOP, BEGIN/UNTIL, and other control structures
7. **[Tutorial 6: Variables and Memory](tutorials/06-variables.md)** - Storing and retrieving data
8. **[Tutorial 7: Arrays and Advanced Memory](tutorials/07-arrays-memory.md)** - Working with arrays and memory allocation
9. **[Tutorial 8: Strings and Characters](tutorials/08-strings.md)** - String operations and character handling
10. **[Tutorial 9: Memory Architecture & The Dictionary](tutorials/09-memory-dictionary.md)** - Understanding Forth's memory model, the dictionary, and meta-programming

## How to Use These Tutorials

1. Read each tutorial in order
2. Type out the examples yourself - don't just copy-paste!
3. Experiment by modifying the examples
4. Complete the exercises at the end of each tutorial
5. Don't rush - make sure you understand each concept before moving on

## Additional Resources

- [gforth Documentation](https://www.complang.tuwien.ac.at/forth/gforth/Docs-html/)
- [Thinking Forth by Leo Brodie](http://thinking-forth.sourceforge.net/) - Classic Forth book
- [Starting Forth by Leo Brodie](https://www.forth.com/starting-forth/) - Another excellent introduction

## Philosophy of Forth

Forth encourages:
- **Simplicity** - Keep things simple and direct
- **Incrementalism** - Build programs piece by piece, testing as you go
- **Bottom-up design** - Create a language for your problem domain
- **Interactivity** - Test every piece of code immediately

## Getting Help

When you get stuck:
1. Use `.s` to see what's on the stack
2. Use `words` to see available words
3. Use `see <word>` to examine a word's definition
4. Read the error messages carefully
5. Start gforth and experiment!

Ready to begin? Head to [Tutorial 0: Introduction to Forth](tutorials/00-introduction.md)!
