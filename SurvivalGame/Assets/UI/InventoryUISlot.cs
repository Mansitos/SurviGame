using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryUISlot;


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
    public int index;
    public bool hasDisplayedItem;
    public SlotType slotType = SlotType.Generic;

    public static event Func<bool> itemMovedToInventory;

    public void OnDrop(PointerEventData eventData)
    {
        if (!hasDisplayedItem)
        {
            // Dropped draggable item
            GameObject dropped = eventData.pointerDrag;
            DraggableUIItem droppedDraggableItem = dropped.GetComponent<DraggableUIItem>();

            // Saving reference of old parent
            Transform oldParent = droppedDraggableItem.parent;
            InventoryUISlot oldParentSlot = oldParent.GetComponent<InventoryUISlot>();

            PerformActionBasedOnTypes(droppedDraggableItem, oldParentSlot, dropped);
        }
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
        if (this.slotType == SlotType.Inventory)
        {
            // Setting this as new parent
            droppedDraggableItem.parent = transform;

            // Clear old parent inventory UI slot
            oldParentSlot.ClearSlot(destroyChild: false);

            // Reflecting swap also in the inventory
            int oldSlotIndex = oldParentSlot.index;
            GameManager.Instance.GetInventorySystem().SwapSlotContents(oldSlotIndex, index);

            hasDisplayedItem = true;
            childDisplayedItem = dropped;
        }
        else
        {
            Debug.Log("From inventory to other");
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
                Debug.Log("YES YOU CAN");

                // Setting this as new parent
                droppedDraggableItem.parent = transform;

                // Clear old parent inventory UI slot
                oldParentSlot.ClearSlot(destroyChild: false);

                GameManager.Instance.GetInventorySystem().TryAddItem(itemToAdd, hasTargetSlot: true, targetSlotIndex: this.index);

                hasDisplayedItem = true;
                childDisplayedItem = dropped;

            }
            else
            {
                Debug.Log("YOU CANT!!!!!!!!!");
            }
        }
        else
        {
            Debug.Log("From other to other");
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
