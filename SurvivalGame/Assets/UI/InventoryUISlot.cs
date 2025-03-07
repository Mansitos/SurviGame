using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Generic,
    Inventory,
    QuickBar,
    ProcessingStation,
    Chest
}

public class InventoryUISlot : MonoBehaviour, IDropHandler
{
    public GameObject childDisplayedItem;
    public bool hasDisplayedItem;
    public SlotType slotType = SlotType.Generic;

    private int index;
    private InventorySystem parentInventory;

    public void OnDrop(PointerEventData eventData)
    {
        // Dropped draggable item
        GameObject dropped = eventData.pointerDrag;
        DraggableUIItem droppedDraggableItem = dropped.GetComponent<DraggableUIItem>();

        // Saving reference of old parent
        Transform oldParent = droppedDraggableItem.parent;
        InventoryUISlot oldParentSlot = oldParent.GetComponent<InventoryUISlot>();

        PerformActionBasedOnTypes(droppedDraggableItem, oldParentSlot, dropped);
    }

    public int GetIndex()
    {
        return index;
    }

    private void PerformActionBasedOnTypes(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        if (oldParentSlot.slotType == SlotType.Inventory)
        {
            ActionFromInventory(droppedDraggableItem, oldParentSlot, dropped);
        }
        else if (oldParentSlot.slotType == SlotType.ProcessingStation)
        {
            ActionFromProcessingStation(droppedDraggableItem, oldParentSlot, dropped);
        }
        else if (oldParentSlot.slotType == SlotType.Chest){
            ActionFromChest(droppedDraggableItem, oldParentSlot, dropped);
        }
    }

    private void ActionFromChest(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        int oldSlotIndex = oldParentSlot.index;

        if (this.slotType == SlotType.Inventory)
        {
            ItemInstance itemToAdd = droppedDraggableItem.linkedItemInstance;
            Debug.LogWarning("PARENT:" + parentInventory);

            if (parentInventory.CanAddItem(itemToAdd))
            {
                parentInventory.TryAddItem(itemToAdd, hasTargetSlot: true, targetSlotIndex: this.index);
                oldParentSlot.parentInventory.TryRemoveItem(itemToAdd);
            }
            else
            {
                Debug.Log("Cannot move item from chest to inventory (no space etc.).");
            }
        }
        if (this.slotType == SlotType.Chest)
        {
            parentInventory.SwapSlotContents(oldSlotIndex, index);
        }
    }

    private void ActionFromInventory(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        int oldSlotIndex = oldParentSlot.index;
        InventorySlot incomingItemSlot = GameManager.Instance.GetPlayerInventory().GetInventorySlotAtIndex(oldSlotIndex);
        ItemInstance incomingItem = incomingItemSlot.itemInstance;

        if (this.slotType == SlotType.Inventory)
        {
            GameManager.Instance.GetPlayerInventory().SwapSlotContents(oldSlotIndex, index);
        }
        else if (this.slotType == SlotType.ProcessingStation)
        {
            ProcessingStationUI stationUI = UIManager.Instance.GetProcessingStationUI();

            if (stationUI.VerifyCanAddToSlot(incomingItem.ItemData, this.index)){
                stationUI.AddToSlot(incomingItem, this.index);
                GameManager.Instance.GetPlayerInventory().TryRemoveItem(incomingItem);
            }
            else
            {
                Debug.Log("Cannot add this item in this slot");
            }
        }
        else if (this.slotType == SlotType.Chest)
        {
            ItemInstance itemToAdd = droppedDraggableItem.linkedItemInstance;
            if (parentInventory.CanAddItem(itemToAdd))
            {
                parentInventory.TryAddItem(itemToAdd, hasTargetSlot: true, targetSlotIndex: this.index);
                GameManager.Instance.GetPlayerInventory().TryRemoveItem(incomingItem);
            }
            else
            {
                Debug.Log("Cannot add item to chest, canAddItem = false");
            }
        }
        else
        {
            Debug.Log("not handled! should never happend!");
        }
    }

    private void ActionFromProcessingStation(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        if (this.slotType == SlotType.Inventory)
        {
            ItemInstance itemToAdd = droppedDraggableItem.linkedItemInstance;

            // TODO change with CanAddItemOnSlot
            if (GameManager.Instance.GetPlayerInventory().CanAddItem(itemToAdd))
            {
                GameManager.Instance.GetPlayerInventory().TryAddItem(itemToAdd, hasTargetSlot: true, targetSlotIndex: this.index);
                ProcessingStationUI stationUI = UIManager.Instance.GetProcessingStationUI();
                stationUI.RemoveItemFromIndexSlot(oldParentSlot.GetIndex());
            }
            else
            {
                Debug.Log("Cannot move item from processing station to inventory.");
            }
        }
        else if (this.slotType == SlotType.ProcessingStation)
        {
            Debug.Log("Moving station to station: not supported for now!");
        }
        else
        {
            Debug.Log("not handled! should never happend!");
        }
    }

    public void SetParentInventory(InventorySystem inventory)
    {
        parentInventory = inventory;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetSlotType(SlotType slotType)
    {
        this.slotType = slotType;
    }

    public void SetDisplayedItem(GameObject item, ItemInstance linkedItemInstance, bool draggable = true)
    {
        if (hasDisplayedItem == false) { 
            hasDisplayedItem = true;
            if (draggable)
            {
                item.AddComponent<DraggableUIItem>();
                item.GetComponent<DraggableUIItem>().linkedItemInstance = linkedItemInstance;

                item.transform.SetParent(this.transform, false);
                item.GetComponent<DraggableUIItem>().parent = this.transform;
            }
            childDisplayedItem = item;
        }
        else { 

            childDisplayedItem = item;
            Debug.Log("UPDATE DISPLAYED ITEM LOGIC HERE");
        }
    }

    public void ClearSlot(bool destroyChild = false)
    {
        if (destroyChild)
        {
            Destroy(childDisplayedItem);
        }
        childDisplayedItem = null;
        hasDisplayedItem = false;
    }

    public GameObject GetDisplayedItem()
    {
        return childDisplayedItem;
    }
}
