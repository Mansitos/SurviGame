using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

public class PlayerQuickBar : MonoBehaviour
{
    public ItemData defaultEmptyItemData;
    public ItemInstance defaultItemInstance;

    public int selectedIndex = 1;
    private ItemInstance selectedItemInstance; // Runtime item instance
    private InventorySystem inventorySystem;
    private GameManager gm;

    public event Action<int> OnChangedQuickBarSelection;

    private void Start()
    {
        gm = GameManager.Instance;
        inventorySystem = gm.GetInventorySystem();
        defaultItemInstance = new ItemInstance(defaultEmptyItemData, 1);
        SelectSlot(selectedIndex); // using selectedIndex for now, future: keys
        UpdateStep();
    }

    private void Update()
    {
        UpdateStep();
    }

    private void UpdateStep()
    {
        CheckInputForQuickBarSelection();
        SelectSlot(selectedIndex);
        UpdateTileSelectionGridVisibility();
    }

    private void CheckInputForQuickBarSelection()
    {
        InputHandler input = gm.GetInputHandler();
        for (int key = 1; key <= 9; key++) // Check 1-9
        {
            if (input.WasQuickBarKeyPressedThisFrame(key))
            {
                selectedIndex = key-1;
            }
        }
        if (input.WasQuickBarKeyPressedThisFrame(0))
        {
            selectedIndex = 9;
        }
    }

    public ItemInstance GetSelectedItemInstance()
    {
        return selectedItemInstance;
    }

    public void SetSelectedItem(ItemInstance itemInstance)
    {
        //TODO: checks if corret type of item?
        selectedItemInstance = itemInstance;
        UpdateTileSelectionGridVisibility();
    }

    private void UpdateTileSelectionGridVisibility()
    {
        if (selectedItemInstance.ItemData != null && selectedItemInstance.ItemData.IsItemOfType(ItemType.Tool))
        {
            gm.GetPlayerTileSelection().SetSelectionVisibility(true);
        }
        else
        {
            gm.GetPlayerTileSelection().SetSelectionVisibility(false);
        }
    }

    private void SelectSlot(int slotIndex)
    {
        selectedIndex = slotIndex;
        InventorySlot selectedInventorySlot = inventorySystem.slots[slotIndex];
        if (selectedInventorySlot.IsEmpty()) {
            selectedItemInstance = defaultItemInstance;
        }
        else
        {
            selectedItemInstance = selectedInventorySlot.itemInstance;
        }
    }

    public int GetSelectedSlotIndex()
    {
        return selectedIndex;
    }
}
