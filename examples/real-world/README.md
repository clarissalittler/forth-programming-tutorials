# Real-World Forth Examples

This directory contains practical examples that demonstrate how Forth is used in real-world scenarios. Each example teaches systems programming concepts while solving actual problems.

## Examples

### 1. Simple Stack-Based VM (`stack-vm.fs`)
Implements a minimal virtual machine with a stack and basic operations. Demonstrates:
- Bytecode interpretation
- Opcode dispatch
- Stack management
- How interpreters work

### 2. Hexdump Utility (`hexdump.fs`)
Displays memory contents in hexadecimal format, like the Unix `hexdump` command. Demonstrates:
- Memory inspection
- Binary data formatting
- Byte-level operations
- Practical systems tool

### 3. Checksum Calculator (`checksum.fs`)
Calculates various checksums (XOR, sum, CRC). Demonstrates:
- Data integrity checking
- Bit manipulation
- Algorithm implementation
- Network/file protocols

## How to Run

```bash
gforth examples/real-world/hexdump.fs
gforth examples/real-world/stack-vm.fs
```

## Learning Goals

These examples teach:
- **Systems thinking**: Understanding how tools work at a low level
- **Binary data**: Working with bytes and memory
- **Algorithms**: Implementing standard algorithms
- **Practical skills**: Building useful utilities

## Extending the Examples

Try these modifications:
1. Add more opcodes to the VM
2. Implement CRC-32 in the checksum utility
3. Add color coding to the hexdump output
4. Create a disassembler for the VM bytecode

## Real-World Applications

Forth is used in:
- **Embedded systems**: Microcontroller firmware
- **Space missions**: NASA spacecraft (including rovers)
- **Boot firmware**: OpenBoot (Sun/Oracle systems)
- **Industrial control**: PLCs, robotics
- **Scientific instruments**: Lab equipment, telescopes
- **Network devices**: Routers, switches
