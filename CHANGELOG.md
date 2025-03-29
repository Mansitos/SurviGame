# **📝 Changelog**

## **Version 0.0.1**

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
---
26-2-2025
- Rudimental `PlayerStatus` logic and realted debug UI. Health, hunger, thirst and energy implemented with related GET and MODIFY methods.
---
27-2-2025
- Rudimental `EndOfDayUI` UI + pause game on end of day.
- Handled two types of end of day: from sleep and from end of day time (with related -50% max energy debuff on day n+1).
- Slightly reworked `BuildingPlacer` logic.
---
1-3-2025
- Rudimental `CraftingUI` and related `CraftUISlot`.
---
2-3-2025
- Added superclass for `CraftUISlot` and `BuildingBlueprintUISlot`: `HorizontalBlueprintUISlot`.
- Added usperclass for `CraftingUI` and `BuildingUI`: `BaseScrollableCraftUI`.
- Rudimental `BuilginUI`.
- Started `WorldObjects` and Building refactoring to use `ScriptableObject` approach to store data (similar to `ItemData` and `ItemInstance`).
---
3-3-2025
- `ScriptableObjects` approach for `ProcessingStation`.
---
4-3-2025
- File organizing + `ScriptableObjects` names refactoring.
- `ScriptableObjects` better editor menu for creation.
- First working rudimental `BuildingUI` + dummy UI icon for buildings.
- Refactoring of the `WorldObject` hierarchy, added IBuildable interface and `WorldObjectBase`.
- Fixed missing update red marked as not craftable/buildable in UI slots.
- Implemented `Eat()` main action for `Food` + added energy value for foods.
- Added multiple Items dummy UI icons.
---
6-3-2025
- Detached UI logic from `InventorySystem`. `InventoryUI` and `QuickBarUI` now listen for `OnInventoryUpdated` event.
- Fixed some `ProcessingStation` and `BuildingPlacer` bugs.
- Rudimental `Chest` logic and `ChestUI`.
---
7-3-2025
- Fixed missing updateUI on inventory swap logic.
- Moving items from `Chest` to `PlayerInventory` and viceversa.
- `InventoryUISlot` now have a linked `InventorySystem` reference for quicker access.
- Finished rudimental Chest to/from Player Inventory item movements.
- Rudimental `Food` rotting logic at end of day.
- Collecting resources now have an energy cost.
- Added `PerformMainAction()` condition for `Tool`: `PlayerHasEnoughEnergyToCollect()`.
- Rudimental Tooltip for UI Items + added `itemData.itemDescription`.
- Right Mouse Button can no longer be used for UI item dragging.
---
9-3-2025
- Right click mouse split logic continued.
---
10-3-2025
- Some rework on `DraggableUIItem` and related (code broken for now).
---
11-3-2025
- Partial refactoring to `ProcessingStation`, `ProcessingStationUI` and `DraggableUIItem` related UI logics.
- `ProcessingStation` now uses 3 `InventorySlot` for internal inventory.
- `RightClickMouse` inventory rudimental logic now mostly working.
- Drag UI Items is now disabled if right mouse slot have items.
---
13-3-2025
- Fixed missing `PlayerInventoryUI` update on inventory change event trigger.
- Fixed and refined right click single item pick-up from multiple stacks of same item.
- Rudimental Item collection `ItemPopupManager` and `ItemPopup`.
---
15-3-2025
- Added rudimental shader to fade objects that are between camera and player (Fading/See-through shader tecnique with dithering).
- Rudimental logic for `Resource` objects to produce items at end of day (e.g. grow bananas on trees).
- Some new resources and item UI icons.
---
16-3-2025
- Added logic to harvest produced items from `Resource` world objects.
- Refactored `Resource` class producing and spawning items logics.
---
18-3-2025
- `Unlink()` Station fix missing clearing state and internal variables for the UI.
- First rudimental implementation for `RefiningStation` with related Data Component and UI. First one is Fish Dryier.
---
23-3-2025
- Fixed missing UI update for `ProcessingStation`.
- Completed `RefinementStation` first logic version.
- `DraggableUIItem` renamed into `DraggableUIItemInstance`. 
- `InventorySlot` now has a properties `canReceiveContent` and `canPickUpContent`, true by default. If false they are used to check if item interaction (drag and drop, pick etc.) is possible.
---
25-03-2025
- Fixed not working chesto to/from inventory item moving
- Fixed missing `RefinementStationUI` update on start/end refining process
- Fixed a bug that caused the possibility to add more than 1 input item into `RefinementStation`.
- Fixed a bug that caused valid `RefinementStation` input item to be added to the output slot.
- You can now no longer pick up input of `RefinementStation` and no longer add items to output slot for `RefinementStation` and `ProducingStation`.
- Fixed a bug that did not hide item tooltip when right picking to zero.
- Fixed null error check of same item when rick click picking up from multiple different inventory slots.
- Experimental `TreeFader2` based on sphere collider.
---
27-03-2025
- Improved Raycasting `TreeFader`.
---
29-03-2025
- `TreeFader` now fades all trees childs plus corrected trees meshes.
- SeeThrough material tweaked values plus added perlin noise effect.
- New Items, blueprints, buildings, craftings, icons, ...
- Terrain grid overlay is now active only when building
  