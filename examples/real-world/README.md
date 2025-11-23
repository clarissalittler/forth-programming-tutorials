# Real-World Forth Examples

This directory contains practical examples that demonstrate how Forth is used in real-world scenarios. Each example teaches systems programming concepts while solving actual problems.

## Examples

### 1. Hexdump Utility (`hexdump.fs`) 🔍
**Difficulty:** Beginner
**Concepts:** Memory inspection, byte operations, formatting

Displays memory contents in hexadecimal format with ASCII representation, like the Unix `hexdump` command.

**What you'll learn:**
- Reading memory byte-by-byte
- Hexadecimal formatting
- ASCII character handling
- Creating practical debugging tools

**Output example:**
```
Address  : 00 01 02 03 04 05 06 07 - 08 09 0A 0B 0C 0D 0E 0F  | ASCII          |
7EA4B9A57580 : 48  65  6C  6C  6F  20  57  6F  - 72  6C  64  21   | Hello World!... |
```

### 2. Stack-Based Virtual Machine (`stack-vm.fs`) 🤖
**Difficulty:** Intermediate
**Concepts:** Bytecode interpretation, instruction dispatch, VM design

Implements a minimal virtual machine with a stack and basic operations (PUSH, ADD, SUB, MUL, etc.).

**What you'll learn:**
- How interpreters work
- Bytecode execution
- Opcode dispatch with CASE
- Stack-based computation
- VM architecture

**Features:**
- 9 opcodes (PUSH, ADD, SUB, MUL, DIV, PRINT, DUP, SWAP, HALT)
- Example programs included
- Shows how languages like Python/Java work internally

### 3. State Machine (`state-machine.fs`) 🚦
**Difficulty:** Beginner-Intermediate
**Concepts:** State machines, dispatch tables, execution tokens

Traffic light controller demonstrating state machine implementation.

**What you'll learn:**
- State machine design
- State transitions
- Execution token (XT) dispatch tables
- Timer-based events
- Real-world embedded systems patterns

**States:** RED → GREEN → YELLOW → RED (with timing)

### 4. Number Base Converter (`number-converter.fs`) 🔢
**Difficulty:** Beginner
**Concepts:** Number bases, formatting, bit patterns

Convert and display numbers in decimal, hexadecimal, binary, and octal.

**What you'll learn:**
- Working with different number bases
- The `base` variable
- Formatting output
- Powers of 2
- Common bit patterns ($FF, $AA, $55, etc.)

**Example output:**
```
Decimal: 42
Hexadecimal: $2A
Binary: %101010
Octal: 52
```

### 5. Temperature Converter (`temperature.fs`) 🌡️
**Difficulty:** Beginner
**Concepts:** Unit conversion, arithmetic, formatting

Convert temperatures between Celsius, Fahrenheit, and Kelvin.

**What you'll learn:**
- Unit conversion formulas
- Integer arithmetic for conversions
- Formatting output
- Common temperature reference points

**Features:**
- Converts between all three temperature scales
- Shows common reference points (freezing, boiling, etc.)
- Demonstrates practical use of arithmetic operations

### 6. RPN Calculator (`calculator.fs`) 🧮
**Difficulty:** Beginner
**Concepts:** Stack-based computation, RPN notation, arithmetic operations

Demonstrates Reverse Polish Notation (RPN) calculation used in HP calculators and stack-based languages.

**What you'll learn:**
- How RPN works vs. infix notation
- Stack-based computation
- No need for parentheses or operator precedence
- Practical operations: square, min, max, abs, clamp, percentages

**Example:**
```
Infix: (5 + 3) * 2 = 16
RPN:   5 3 + 2 *   = 16
```

### 7. Binary Protocol Parser (`protocol-parser.fs`) 📡
**Difficulty:** Intermediate
**Concepts:** Binary data, bit fields, protocol parsing, byte operations

Parse a simple binary network packet format with version, type, flags, and length fields.

**What you'll learn:**
- Binary data structures
- Bit field extraction with masks and shifts
- Big-endian multi-byte values
- Flag bits for boolean options
- Protocol design patterns

**Features:**
- 8-byte packet format with packed bit fields
- Version (4 bits) + Type (4 bits) in first byte
- Flag byte with individual bit flags
- 16-bit length field (big-endian)
- Real-world application to network protocols

## How to Run

```bash
cd examples/real-world
gforth hexdump.fs
gforth stack-vm.fs
gforth state-machine.fs
gforth number-converter.fs
gforth temperature.fs
gforth calculator.fs
gforth protocol-parser.fs
```

## Learning Path

**Beginners:** Start with `temperature.fs` → `calculator.fs` → `number-converter.fs` → `hexdump.fs`
**Intermediate:** Try `state-machine.fs` → `protocol-parser.fs` → `stack-vm.fs`
**Advanced:** Modify and extend the examples (see below)

## Extending the Examples

### Hexdump
- [ ] Add color coding for different byte ranges
- [ ] Support different byte groupings (1, 2, 4, 8)
- [ ] Add file input (read from disk)
- [ ] Implement reverse operation (hex → binary)

### Stack VM
- [ ] Add more opcodes (NEG, MOD, COMPARE)
- [ ] Implement conditional jumps (IF, GOTO)
- [ ] Add a simple assembler
- [ ] Create a disassembler
- [ ] Add function call support (CALL, RET)

### State Machine
- [ ] Add pedestrian button (interrupt-driven transition)
- [ ] Implement emergency mode (flashing red)
- [ ] Add multiple intersections
- [ ] Create a vending machine state machine
- [ ] Implement a simple game AI

### Number Converter
- [ ] Parse string input ("0xFF" → 255)
- [ ] Add Roman numerals
- [ ] Implement scientific notation
- [ ] Add fractional/floating point support

### Temperature Converter
- [ ] Add Rankine scale (like Kelvin but Fahrenheit-based)
- [ ] Support fractional temperatures (fixed-point math)
- [ ] Add input parsing (e.g., "32F" or "100C")
- [ ] Create a temperature range checker (warn if out of bounds)
- [ ] Add more reference points (liquid nitrogen, sun surface, etc.)

### RPN Calculator
- [ ] Add a calculation history (keep track of results)
- [ ] Implement variables/memory (STO/RCL like HP calculators)
- [ ] Add trigonometric functions (sin, cos, tan)
- [ ] Implement undo functionality
- [ ] Create a REPL mode for interactive calculations

### Protocol Parser
- [ ] Add CRC/checksum field and validation
- [ ] Implement packet serialization (build packets from data)
- [ ] Add variable-length payload support
- [ ] Create multiple protocol versions
- [ ] Implement a packet logger/analyzer
- [ ] Add support for nested/layered protocols

## Real-World Applications

These patterns are used in:

**Hexdump:**
- Debuggers (GDB, LLDB)
- Hex editors
- Firmware tools
- Network packet analysis

**Stack VM:**
- Java Virtual Machine (JVM)
- Python bytecode interpreter
- WebAssembly
- Forth itself!
- PostScript

**State Machines:**
- Embedded systems (appliances, IoT)
- Game development (AI, animations)
- Network protocols (TCP/IP stack)
- Compilers (lexers, parsers)
- UI components

**Number Converters:**
- Programmer calculators
- Assembly language tools
- Network utilities (IP addresses)
- Color pickers (RGB hex codes)

**Temperature Converters:**
- Scientific instruments (thermometers, sensors)
- Weather stations and apps
- HVAC systems
- Cooking appliances (ovens, sous vide)
- Medical devices

**RPN Calculators:**
- HP calculators (HP-48, HP-50g)
- Scientific computing
- Financial calculators
- Engineering applications
- Postfix expression evaluation in compilers

**Protocol Parsers:**
- Network routers and switches
- Firewalls and packet filters
- Wireshark and packet analyzers
- Embedded network stacks
- IoT device communication
- File format readers (PNG, ZIP, ELF)

## Additional Examples to Explore

Want more? Try implementing these:

### Easy
- Roman numeral converter
- Morse code encoder/decoder
- Simple text cipher (Caesar, ROT13)

### Medium
- CRC-32 checksum
- Base64 encoder/decoder
- Run-length encoding (RLE)
- Simple hash function

### Advanced
- Mini assembler (for the stack VM)
- Simple compression algorithm
- File system utilities
- Tiny REPL (Read-Eval-Print Loop)
- Advanced protocol parser with multiple layers

## Systems Programming Concepts

These examples teach:

1. **Memory Management**
   - Direct memory access
   - Byte vs. cell operations
   - Buffer handling

2. **Low-Level I/O**
   - Hex/binary formatting
   - Character encoding
   - Binary data structures

3. **Control Flow**
   - State machines
   - Dispatch tables
   - Event handling

4. **Interpreters & VMs**
   - Bytecode execution
   - Opcode dispatch
   - Stack-based computation

5. **Data Representation**
   - Number bases
   - Bit manipulation
   - Binary formats

## Tips for Learning

1. **Read the code first** - Understand before running
2. **Trace execution** - Use `.s` to watch the stack
3. **Modify incrementally** - Change one thing at a time
4. **Compare to C** - How would you do this in C?
5. **Build your own** - Best way to learn is to create

## Contributing

Have an interesting real-world example? Criteria for inclusion:
- Demonstrates practical application
- Teaches systems programming concepts
- Is well-commented and tested
- Solves a real problem

## Further Reading

- [Thinking Forth](http://thinking-forth.sourceforge.net/) - Philosophy and patterns
- [Starting Forth](https://www.forth.com/starting-forth/) - Classic tutorial
- [gforth Manual](https://www.complang.tuwien.ac.at/forth/gforth/Docs-html/) - Reference documentation

Happy hacking! 🚀
