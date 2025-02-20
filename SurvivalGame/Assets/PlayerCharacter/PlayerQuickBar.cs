using UnityEngine;

public class PlayerQuickBar : MonoBehaviour
{
    public ItemData defaultEmptyItemData;
    public ItemInstance defaultItemInstance;

    private ItemData selectedItemData; // Assign in Inspector
    public int selectedIndex = 1;
    private ItemInstance selectedItemInstance; // Runtime item instance
    private InventorySystem inventorySystem;
    private GameManager gm;

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
        SelectSlot(selectedIndex);
        UpdateTileSelectionGridVisibility();
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
        InventorySlot selectedInventorySlot = inventorySystem.slots[slotIndex - 1];
        if (selectedInventorySlot.IsEmpty()) {
            //Debug.Log("Selected slot empty");
            selectedItemInstance = defaultItemInstance;
            selectedItemData = defaultEmptyItemData;
        }
        else
        {
            //Debug.Log("Selected slot!");
            selectedItemInstance = selectedInventorySlot.itemInstance;
            selectedItemData = selectedItemInstance.ItemData;
        }
    }

    public int GetSelectedSlotIndex()
    {
        return selectedIndex;
    }
}
