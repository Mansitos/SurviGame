using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Inventory,
    QuickBar,
    Generic,
    Mouse,
    ProcessingStation,
    RefinementStation,
    InventoryUISlot,
    Chest
}

public class InventoryUISlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public GameObject childDisplayedItem;
    public SlotType slotType = SlotType.Generic;

    private int index;
    private bool hasDisplayedItem;
    private InventorySystem parentInventory;
    public InventorySlot linkedInventorySlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!UIManager.Instance.GetMouseInventorySlot().GetInventorySlot().IsEmpty())
        {
            return;
        }

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
        else if (oldParentSlot.slotType == SlotType.Mouse)
        {
            ActionFromMouse(droppedDraggableItem, oldParentSlot, dropped);
        }
    }

    private bool AddToStation(ItemInstance item, SlotType slotType)
    {
        IStationUI stationUI = null;

        if (slotType == SlotType.ProcessingStation)
        {
            stationUI = UIManager.Instance.GetProcessingStationUI();
        }
        else if (slotType == SlotType.RefinementStation)
        {
            stationUI = UIManager.Instance.GetRefinementStationUI();
        }
        else
        {
            return false;
        }

        if (slotType == SlotType.RefinementStation) // TODO: DOESN?T WORK?
        {
            if (!this.linkedInventorySlot.IsEmpty()){
                return false; // only 1 item at once.
            }
        }
        

        if (stationUI.VerifyCanAddToSlot(item.ItemData, this.index))
        {
            if (slotType == SlotType.ProcessingStation)
            {
                stationUI.AddToSlot(item, this.index);
                GameManager.Instance.GetPlayerInventory().TryRemoveItem(item);
                return true;
            }
            else if (slotType == SlotType.RefinementStation)
            {
                int originalQuantity = item.Quantity;
                Debug.Log(originalQuantity);
                if (originalQuantity == 1) // just one, add and ok!
                {
                    stationUI.AddToSlot(item, this.index);
                    GameManager.Instance.GetPlayerInventory().TryRemoveItem(item);
                    return true;
                }
                else // more than one, but just 1 can be added
                {
                    ItemInstance newInstance = new ItemInstance(item.ItemData, 1);
                    stationUI.AddToSlot(newInstance, this.index);
                    GameManager.Instance.GetPlayerInventory().TryRemoveItem(newInstance);
                    return false;
                }
            }

            return false;
        }
        else
        {
            Debug.Log("Cannot add this item in this slot");
            return false;
        }
    }

    private void ActionFromMouse(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        ItemInstance incomingItem = droppedDraggableItem.linkedInventorySlot.itemInstance;
        bool esit = false;

        if (this.slotType == SlotType.Inventory || this.slotType == SlotType.Chest)
        {
            if (parentInventory.CanAddItem(incomingItem))
            {
                esit = parentInventory.TryAddItem(incomingItem, hasTargetSlot: true, targetSlotIndex: this.index);
            }
        }
        else if (this.slotType == SlotType.ProcessingStation || this.slotType == SlotType.RefinementStation)
        {
            esit = AddToStation(incomingItem, this.slotType);
        }

        if (esit == true)
        {
            UIManager.Instance.GetMouseInventorySlot().Clear();
        }
    }

    private void ActionFromChest(DraggableUIItem droppedDraggableItem, InventoryUISlot oldParentSlot, GameObject dropped)
    {
        int oldSlotIndex = oldParentSlot.index;

        if (this.slotType == SlotType.Inventory)
        {
            ItemInstance itemToAdd = droppedDraggableItem.linkedInventorySlot.itemInstance;
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
        else if (this.slotType == SlotType.ProcessingStation || this.slotType == SlotType.RefinementStation)
        {
            AddToStation(incomingItem, this.slotType);
        }
        else if (this.slotType == SlotType.Chest)
        {
            ItemInstance itemToAdd = droppedDraggableItem.linkedInventorySlot.itemInstance;
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
            ItemInstance itemToAdd = droppedDraggableItem.linkedInventorySlot.itemInstance;

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

    public void SetDisplayedItemIcon(InventorySlot slot)
    {
        GameObject icon = UIUtils.CreateIcon(slot.itemInstance.ItemData.uiIcon, -1, null, this.gameObject);
        SetDisplayedItem(icon, slot); // Todo: totalmente da rivedere
    }

    // TODO: MAKE IT PRIVATE and use SetDisplayedItemIcon() which is cleaner
    public void SetDisplayedItem(GameObject item, InventorySlot linkedInventorySlot, bool draggable = true)
    {
        if (hasDisplayedItem == false) { 
            hasDisplayedItem = true;
            if (draggable)
            {
                item.AddComponent<DraggableUIItem>();
                item.GetComponent<DraggableUIItem>().linkedInventorySlot = linkedInventorySlot;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject mouseSlotGO = UIManager.Instance.GetMouseInventorySlot().GetInventorySlotGO();
        InventoryUISlot mouseSlot = mouseSlotGO.GetComponent<InventoryUISlot>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (mouseSlot.childDisplayedItem != null)
            {
                if (!mouseSlot.childDisplayedItem.GetComponent<DraggableUIItem>().linkedInventorySlot.IsEmpty())
                {
                    // Dropped draggable item
                    DraggableUIItem droppedDraggableItem = mouseSlot.childDisplayedItem.GetComponent<DraggableUIItem>();
                    GameObject dropped = eventData.pointerDrag;
                    Transform oldParent = droppedDraggableItem.parent;
                    InventoryUISlot oldParentSlot = oldParent.GetComponent<InventoryUISlot>();

                    PerformActionBasedOnTypes(droppedDraggableItem, oldParentSlot, dropped);
                }
            }
            else
            {
                Debug.Log("nothing to release");
            }
        }
    }

    public bool IsDisplayingAnItemIcon()
    {
        return hasDisplayedItem;
    }

    public void SetLinkedInventorySlot(InventorySlot slot)
    {
        linkedInventorySlot = slot;
    }

    public InventorySlot GetLinkedInventorySlot()
    {
        return linkedInventorySlot;
    }
}
