
# **🛠️ TODO List**
## **0.0.1 Core Logics**

### **Player Movement**
- **🛠️ TODO:**
  - Animations rework
    - while rotating left and right
    - smoothing, tweaks, times

- **🪲 BUGS:**
  -  Player transform.position is going up and down, why?

### **Player Interaction**
- **🛠️ TODO:**
  - Rework interaction logic:
    - tweak interaction distance
    - deeply analiyze stardew valley mechanic
    - Actual interaction system to collect sucks. Keep mouse pressed to collect, if move, nothing happens. To stop collect simply release mouse before finish.
  - Collect of type "pickup" should be without dropped items, items should be instantly added to inventory.

### **Terrain System**
- **🛠️ TODO:** ...
- **🪲 BUGS:**
  -  Seems 3D... spawn an object on y>> and you will see a tile occupied on air
  
### **World Objects**
- **🛠️ TODO:** ...

### **Building System**
- **🛠️ TODO:**
  - Usable only with/for `Building` class
  - `Blueprint` concept (requires Items to be implemented)
  - ☑️ `Building` class specifies dimension (default 1x1)

### **Items**
- **🛠️ TODO:**
    - basic concrete items
  
### **Inventory System**
- **🛠️ TODO:**

### **Quick Bar**
- **🛠️ TODO:**
  - Quick bar logics

### **Basic UI**
- **🛠️ TODO:**
  - Basic UI for
    - Inventory
    - Player Status

### **Code**
- **⚙️ Refactor:**
  - GetMouseWorldPosition() to move as static method on mainCamera and used everywhere where needed.
  - freeSelectedTile logic for building placer
- **🖼️ Visuals:**
  - Inventory System -> fix serialization so that you can debug from inspector

- **🎮 Mechanics:**
  
## **0.0.2 [To define]**

# **💡 Ideas**
Ides for now:
- Border on edge of dropped item model/mesh
- Player status: energy, food, thirst
- Farming
- Fishing
- Skills
- Day/night (like s.valley)