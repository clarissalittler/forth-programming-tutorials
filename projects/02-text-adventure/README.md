# Project 2: Text Adventure Game

## Difficulty: 🟡 Intermediate

## Prerequisites
- Tutorials 0-8 (Introduction through Strings)
- Understanding of data structures (arrays, records)
- String manipulation
- State management

## Overview

Create a classic text adventure game where players navigate rooms, collect items, and solve puzzles. This project teaches you about managing game state, implementing a simple parser, and designing data structures.

## Learning Goals

- Design and implement custom data structures
- Manage complex program state
- Parse natural language commands
- Create an interactive user experience
- Think about game design and user experience

## Specification

### Phase 1: Basic Navigation

**Features:**
- At least 4 interconnected rooms
- Movement commands: north, south, east, west (or n, s, e, w)
- Room descriptions
- "look" command to redisplay current room

**Example:**
```
You are in a dusty library. Books line the walls.
Exits: north, east

> north

You are in a grand hallway. Portraits watch you from the walls.
Exits: south, west

> west

You are in a kitchen. The smell of stale bread fills the air.
Exits: east

> look

You are in a kitchen. The smell of stale bread fills the air.
Exits: east
```

### Phase 2: Items and Inventory

**Features:**
- Items in rooms
- `take <item>` - Pick up an item
- `drop <item>` - Drop an item
- `inventory` - Show what you're carrying
- Item descriptions with `examine <item>`

**Example:**
```
You are in a library. Books line the walls.
You see: a rusty key, an old book
Exits: north

> take key
You take the rusty key.

> inventory
You are carrying:
  a rusty key

> examine key
A small rusty key. It looks like it might open a door.

> north

You are in a hallway. Portraits watch you from the walls.
Exits: south, west

> drop key
You drop the rusty key.
```

### Phase 3: Puzzles and Win Condition

**Features:**
- Locked doors requiring items
- Simple puzzles
- Win condition
- Optional: Lose conditions

**Example:**
```
You are in a hallway. A locked door blocks the way north.
You see: nothing
Exits: south, west

> unlock door
You need a key to unlock the door.

> take key

> unlock door
The rusty key fits! The door creaks open.
You can now go north.

> north

You escape into the sunlight! You win!
```

## Data Structure Design

### Rooms

Each room needs:
- Description
- Exits (connections to other rooms)
- Items in the room
- Special properties (locked, dark, etc.)

**Suggested structure:**
```forth
\ Room structure using CREATE...DOES>
: room  ( "name" -- )
    create
        \ Reserve space for room data
        4 cells allot   \ north, south, east, west exits
        here 100 allot , \ description string
        \ ... more fields
    does> ( -- addr )
;
```

### Items

Each item needs:
- Name
- Description
- Location (room ID or -1 if carried)
- Properties (can be taken, used, etc.)

### Player State

Track:
- Current room
- Inventory (list of items)
- Game flags (for puzzles)

## Implementation Tips

### Room System

```forth
\ Define rooms as constants with IDs
0 constant room-library
1 constant room-hallway
2 constant room-kitchen
3 constant room-bedroom

\ Create room connection table
create room-exits
    \ library: north, south, east, west
    room-hallway , -1 , room-kitchen , -1 ,
    \ hallway: north, south, east, west
    -1 , room-library , room-bedroom , -1 ,
    \ ... more rooms

\ Get exit from room in direction
: get-exit  ( room direction -- next-room )
    4 * + cells room-exits + @
;
```

### Command Parser

```forth
: parse-command  ( -- )
    \ Read command
    parse-name

    \ Check for single-word commands
    2dup s" north" compare 0= if go-north exit then
    2dup s" south" compare 0= if go-south exit then
    2dup s" look" compare 0= if describe-room exit then
    2dup s" inventory" compare 0= if show-inventory exit then

    \ Check for two-word commands (take, drop, etc.)
    2dup s" take" compare 0= if parse-name take-item exit then

    ." I don't understand that." cr
;
```

### String Storage

```forth
\ Store room descriptions
create library-desc
    s" You are in a dusty library. Books line the walls." ,

: print-description  ( addr -- )
    @ count type cr
;
```

## Game Map Example

Here's a sample 4-room map:

```
    [Kitchen]
        |
   [Hallway]
        |
    [Library] --- [Garden]
```

## Testing Checklist

- [ ] Can move between all rooms
- [ ] Can't move in blocked directions
- [ ] Room descriptions display correctly
- [ ] Can take and drop items
- [ ] Inventory displays correctly
- [ ] Can examine items
- [ ] Puzzle logic works
- [ ] Win condition triggers
- [ ] Graceful error messages for invalid commands

## Hints

<details>
<summary>Hint 1: Storing Strings</summary>

Use a simple string pool:
```forth
create string-pool 1000 allot
variable string-ptr

: add-string  ( addr len -- str-id )
    string-ptr @ >r
    dup string-ptr @ c!
    string-ptr @ 1+ swap cmove
    r>
;
```
</details>

<details>
<summary>Hint 2: Item Management</summary>

```forth
variable current-room
10 constant max-items

create item-locations max-items cells allot

: item-here?  ( item -- flag )
    cells item-locations + @
    current-room @ =
;
```
</details>

<details>
<summary>Hint 3: Direction Constants</summary>

```forth
0 constant north
1 constant south
2 constant east
3 constant west

: go-north  current-room @ north get-exit move-to-room ;
: go-south  current-room @ south get-exit move-to-room ;
```
</details>

## Extensions

1. **Combat System:**
   - Enemies in rooms
   - Health points
   - Weapons as items

2. **Advanced Puzzles:**
   - Combination locks
   - Item combinations
   - Timed challenges

3. **Richer Descriptions:**
   - Randomized description variants
   - Descriptions change based on state
   - Atmospheric details

4. **Save/Load:**
   - Save game state to file
   - Load previous game

5. **NPC Dialogues:**
   - Characters to talk to
   - Dialogue trees
   - Quests

## Example Game: "The Mansion Escape"

```
=== THE MANSION ESCAPE ===

You awaken in a dusty library. The door behind you is locked.
You must find a way out!

You are in a library. Books line the walls. Dust motes float in dim light.
You see: a brass key, an old book
Exits: north

> examine book
"Mysteries of the Mansion" - it describes a hidden passage.

> take key
You take the brass key.

> take book
You take the old book.

> north
You are in a grand hallway. Portraits watch you from the walls.
A locked door blocks the way north. The door has a brass keyhole.
Exits: south, west

> use key
The brass key fits perfectly! The door unlocks with a satisfying click.

> north
You step through the door into bright sunlight. Freedom!

*** YOU WIN! ***

Thanks for playing!
```

## Reflection Questions

1. How did you decide to structure your room data?
2. What was the most challenging part of the parser?
3. How would you extend this to support more complex commands?
4. What data structure trade-offs did you make?

Happy adventuring! Remember: good game design is iterative. Start simple, then add features.
