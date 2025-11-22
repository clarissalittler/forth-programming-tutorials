# Forth Programming Projects

This directory contains hands-on projects to reinforce your Forth skills. Each project builds on concepts from the tutorials and challenges you to apply what you've learned.

## How to Use These Projects

1. **Complete the tutorials first** - Each project lists prerequisite tutorials
2. **Read the project specification** carefully
3. **Plan before coding** - Think about the words you'll need
4. **Build incrementally** - Test each word as you write it
5. **Compare with the solution** only after attempting it yourself

## Project List

### 1. RPN Calculator (After Tutorial 5)
**Directory:** `01-rpn-calculator/`
**Skills:** Stack manipulation, parsing, conditionals, loops

Build a Reverse Polish Notation calculator that reads user input and evaluates expressions.

**Features to implement:**
- Basic arithmetic (+, -, *, /)
- Stack viewing (.s)
- Clear command
- Memory storage/recall
- Scientific functions (optional)

### 2. Text Adventure Game (After Tutorial 8)
**Directory:** `02-text-adventure/`
**Skills:** Strings, data structures, state management, parsing

Create a simple interactive fiction game with rooms, items, and puzzles.

**Features to implement:**
- Room navigation (north, south, east, west)
- Item management (take, drop, inventory)
- Simple puzzles
- Game state (win/lose conditions)
- Save/load (optional)

### 3. Data Structures Library (After Tutorial 9)
**Directory:** `03-data-structures/`
**Skills:** Memory management, CREATE...DOES>, algorithms

Implement fundamental data structures from scratch.

**Structures to build:**
- Dynamic arrays (with growth)
- Linked lists
- Stacks and queues
- Hash tables (optional)
- Binary search trees (optional)

## Project Difficulty Levels

- 🟢 **Beginner**: Follows tutorials closely, clear requirements
- 🟡 **Intermediate**: Requires combining concepts, some design decisions
- 🔴 **Advanced**: Open-ended, requires research and creativity

### Current Projects by Difficulty

| Project | Difficulty | Prerequisites |
|---------|-----------|---------------|
| RPN Calculator | 🟢 Beginner | Tutorials 0-5 |
| Text Adventure | 🟡 Intermediate | Tutorials 0-8 |
| Data Structures | 🔴 Advanced | Tutorials 0-9 |

## Tips for Success

### Before You Start
- Review the relevant tutorials
- Sketch out your design on paper
- List the words you'll need to define
- Think about the stack effects

### While Coding
- Test every word immediately after defining it
- Use `.s` frequently to debug stack issues
- Add comments with stack effects
- Build from simple to complex

### When Stuck
- Review the tutorial sections on relevant topics
- Try the problem in small pieces
- Use `see` to examine word definitions
- Check the debugging guide (docs/debugging-guide.md)

### Code Review Checklist
- [ ] All words have stack effect comments
- [ ] No magic numbers (use constants)
- [ ] Words are appropriately factored (not too long)
- [ ] Code is tested with various inputs
- [ ] Edge cases are handled

## Extending the Projects

Once you complete a project, try these extensions:

**RPN Calculator:**
- Add hex/binary number support
- Implement user-defined functions
- Add graphing capabilities
- Create a persistent history

**Text Adventure:**
- Add NPCs (non-player characters)
- Implement a combat system
- Create a parser for natural language
- Add a scoring system

**Data Structures:**
- Add iterators
- Implement generic versions (working with any data type)
- Add serialization (save to file)
- Benchmark and optimize

## Sharing Your Solutions

If you'd like to share your solutions or get feedback:
1. Create a GitHub repository
2. Document your design decisions
3. Include test cases
4. Compare different approaches

## Additional Project Ideas

Want more challenges? Try these:

- **Assembler**: Write a simple assembler for a fictional CPU
- **Database**: Create a simple key-value store with persistence
- **Compiler**: Build a calculator language compiler
- **Web Server**: Implement a basic HTTP server (advanced!)
- **Game of Life**: Conway's Game of Life with display
- **Music Tracker**: Simple music composition tool
- **Chip-8 Emulator**: Emulate the Chip-8 virtual machine

Happy coding! Remember: in Forth, you're not just writing a program - you're building a language for your problem domain.
