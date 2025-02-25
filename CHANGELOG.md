# **Changelog**

1-2-2025
- Pushing project bones
---
2-2-2025
- Rudimental Building system
- Classes refactoring
---
7-2-2025
- Dummy PlayerQuickBar script to select an item
- Item invoke main action method based on select item
- Player Input tile selection for interaction with objects
---
8-2-2025
- Added rocks and trees assets
- Fixed building placer bug which did not check if the object to delete was strictly a buidling
- Rudimental animation logic for resource collection
- Slight rework of animations + fixed bug that allowed the player to run without walking
- Rudimental collect resource + item spawn + destroy on collect logic
---
9-2-2025
- Fixed build mode could be toggled while performing action
- Tweaked animation from chopping/ming to walking/running
---
10-2-2025
- Added assets
- PickUp collection type + related animation
---
11-2-2025
- `CraftBlueprint` basic ScriptableObject logic (`CanCraft()`, `Craft()`)
- Inventory logics for crafting checks: available items, availalbe slots for the output
---
12-2-2025
- Debug Editor script for give, remove object and craft `CraftBlueprint`
- Fixed Inventory CanAdd conditions: there was a missing condition
---
13-2-2025
- `CraftBlueprint` is now abstract and extended by `ItemCraftBlueprint` and `BuildingBlueprint`
- DebugLogic for building buildings (remove B "building" mode).
---
15-2-2025
- It is now not possible to build in the 3x3 area occupied by player
- `CraftItemBlueprint` can now specify a required type of `CraftingTable` to be in range (a new subclass of `Building`)
- Rudimental check for required `CraftingTable` in-range methods + support methods in `TerrainGridSystem`
- Item `CanCraft()` strong refactoring + fixed bug -> craft causing overweight was not checked
- `InventorySlots` now are enumerated + QuickBar logic reworked -> it watches the inventory based on a selectedIndex slot
---
16-2-2025
- Inventory Debug UI now highlihts in red the selected quickbar slot
- Minor refactoring on quickbar and inventory logic
---
17-2-2025
- Core prototype (incomplete) for processing buildings (to model things like furnaces), new Building subclass: `ProcessingStation`
- `ProcessingItemCraftBlueprint` blueprint for declaring input and output (to model things like 5 raw iron -> 1 iron ingot)
- `ItemInstance` is now serializable and editable from the inspector
---
18-2-2025
- Continued `ProcessingStation` logic: available fuel check and code refactoring
- BugFound: ItemInstance initiated as null fails at == null. Try to remove [System.Serializable].
---
19-2-2025
- First Inventory UI core logics
---
20-2-2025
- Inventory UI logic expanded: now UI swap reflects on inventory + `I` key to open/close and related `InventoryMode` flag.
- QuickBar UI implemented.
- Dummy sprite for pickaxe, axe, fiber.
- Minor `GameManager` refactoring.
---
21-2-2025
- Minor refactoring: introduction of `UIManager`
---
22-2-2025
- Strong inventory and quickbar UI refactoring. Added parent class: `BaseInventoryUI` with common logic.
- Rudimental `ProcessingStationUI` with some of the starting logic.
- Implemented base game time logics with related `GameTimeManager`.
- Partial code for moving Items between difference UI inventories (e.g. ProcessingStation -> Inventory). 
---
23-2-2025
- Fixed potential bug in `InventorySystem`. `AddItem()` now if the slot is empty create a clone of the given `ItemInstance`, does not save the reference. Just for safety to avoid duplication glitches.
- Completed rudimental UI logic to move objects between inventory and processing station: still to refine, refactor and fix bugs.
- Modified Inventory to Inventory item move so that items are swapped if target slot is not empty
- Fixed: you can now drop item x on item x and it executes add logic
- Quickbar rudimental item selection UI highlight
---
24-2-2025
- `QuickBar` selection with keys
- weight counter in `InventoryUI`
- Renamed `PlayerInputHandler` in `InputHandler`
- SecondaryAction implemented. You can now interact with buildings and open `ProcessingStations`
- ESC for quitting inventory tabs (rudimental)
---
25-2-2025
- Strong UI open/close and keys input rework. Implementation is now much more cleaner.
- Fixed missing parent set on `DraggableUIItem` via `InventoryUISlot.SetDisplayedItem()` method.