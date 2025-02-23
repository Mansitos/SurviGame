using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Generic,
    Inventory,
    QuickBar,
    ProcessingStation
}

public class InventoryUISlot : MonoBehaviour, IDropHandler
{
    public GameObject childDisplayedItem;
    private int index;
    public bool hasDisplayedItem;
    public SlotType slotType = SlotType.Generic;

    public void OnDrop(PointerEventData eventData)
    {
        //if (!hasDisplayedItem)

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
        else
        {
            ActionFromProcessingStation(droppedDraggableItem, oldParentSlot, dropped);
        }
    }

    private void ActionFromInventory(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        int oldSlotIndex = oldParentSlot.index;

        if (this.slotType == SlotType.Inventory)
        {
            GameManager.Instance.GetInventorySystem().SwapSlotContents(oldSlotIndex, index);
            GameManager.Instance.GetUIManager().GetInventoryUI().UpdateSlots();
        }
        else if (this.slotType == SlotType.ProcessingStation)
        {
            // Get inventory content at index
            InventorySlot incomingItemSlot = GameManager.Instance.GetInventorySystem().GetInventorySlotAtIndex(oldSlotIndex);
            ItemInstance incomingItem = incomingItemSlot.itemInstance;
            ProcessingStationUI stationUI = GameManager.Instance.GetUIManager().GetProcessingStationUI();

            if (stationUI.VerifyCanAddToSlot(incomingItem.ItemData, this.index)){
                Debug.Log("asd");
                stationUI.AddToSlot(incomingItem, this.index);
                GameManager.Instance.GetInventorySystem().TryRemoveItem(incomingItem);
            }
            else
            {
                Debug.Log("Cannot add this item in this slot");
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
            if (GameManager.Instance.GetInventorySystem().CanAddItem(itemToAdd))
            {
                GameManager.Instance.GetInventorySystem().TryAddItem(itemToAdd, hasTargetSlot: true, targetSlotIndex: this.index);
                ProcessingStationUI stationUI = GameManager.Instance.GetUIManager().GetProcessingStationUI();
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
        hasDisplayedItem = true;
        if (draggable)
        {
            item.AddComponent<DraggableUIItem>();
            item.GetComponent<DraggableUIItem>().linkedItemInstance = linkedItemInstance;
        }
        childDisplayedItem = item;
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
}
