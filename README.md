# **🎮 Survival Game Project TODO List**

# **0.0.1 Core Logics**

## **Player Movement**
- **🛠️ TODO:**
  - Animations rework
    - while rotating left and right
    - smoothing

# **Player Interaction**
- **🛠️ TODO:**
  - First version should have: pick, axe, pickage animations
  - Selected cell to interact with (like s.valley)

## **Terrain System**
- **🛠️ TODO:**
  - How to position objects via editor and automatically update the terrain grid matrix to be "occupied"?
  - Support more than 1x1 objects to be placed/handled
  
## **World Objects**
- **🛠️ TODO:**
  - Implement `WorldObject` class with such subclasses:
    - `Destroyable` (e.g. trees, rocks, other resources)
      - In future: Tree, Rock, other main classes of resources/entities
    - ☑️ `Building`

## **Building System**
- **🛠️ TODO:**
  - Usable only with/for `Building` class
  - `Blueprint` concept (requires Items to be implemented)
  - ☑️ `Building` class specifies dimension (default 1x1)

## **Items**
- **🛠️ TODO:**
  - Implement `Item` class with subclasses
    - `Consumable`
      - `Food`
      - others in future
    - `Tool`
      - `Weapon`
      - others in future
  
## **Inventory System**
- **🛠️ TODO:**
  - Inventory logics

## **Quick Bar**
- **🛠️ TODO:**
  - Quick bar logics

## **Basic UI**
- **🛠️ TODO:**
  - Basic UI for
    - Inventory
    - Player Status


# **0.0.2 [To define]**

# **Ideas**
Ides for now:
- Player status: energy, food, thirst
- Farming
- Fishing
- Skills
- Day/night (like s.valley)