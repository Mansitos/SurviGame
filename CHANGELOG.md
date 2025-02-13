# **Changelog**

2-2025
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
- CraftBlueprint basic ScriptableObject logic (CanCraft, Craft)
- Inventory logics for crafting checks: available items, availalbe slots for the output
---
12-2-2025
- Debug Editor script for give, remove object and craft CraftBlueprint
- Fixed Inventory CanAdd conditions: there was a missing condition
---
13-2-2025
- CraftBlueprint is now abstract and extended by ItemCraftBlueprint and BuildingBlueprint
- DebugLogic for building buildings (remove B "building" mode).
- 