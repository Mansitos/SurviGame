using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlot : MonoBehaviour, IDropHandler
{
    public GameObject childDisplayedItem;
    public int index;
    public bool hasDisplayedItem;

    public void OnDrop(PointerEventData eventData)
    {
        if (!hasDisplayedItem)
        {
            // Dropped draggable item
            GameObject dropped = eventData.pointerDrag;
            DraggableUIItem draggableItem = dropped.GetComponent<DraggableUIItem>();

            // Saving reference of old parent and setting this as the new one
            Transform oldParent = draggableItem.parent;
            draggableItem.parent = transform;

            // Clear old parent inventory UI slot
            oldParent.GetComponent<InventoryUISlot>().ClearSlot(destroyChild: false);

            // Reflecting swap also in the inventory
            int oldSlotIndex = oldParent.GetComponent<InventoryUISlot>().index;
            GameManager.Instance.GetInventorySystem().SwapSlotContents(oldSlotIndex, index);

            hasDisplayedItem = true;
            childDisplayedItem = dropped;
        }
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetDisplayedItem(GameObject item, bool draggable = true)
    {
        hasDisplayedItem = true;
        if (draggable)
        {
            item.AddComponent<DraggableUIItem>();
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
