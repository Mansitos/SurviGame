using System.Collections.Generic;
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
            GameObject dropped = eventData.pointerDrag;
            DraggableUIItem draggableItem = dropped.GetComponent<DraggableUIItem>();

            Transform oldParent = draggableItem.parent;
            draggableItem.parent = transform;

            oldParent.GetComponent<InventoryUISlot>().ClearSlot();

            int oldSlotIndex = oldParent.GetComponent<InventoryUISlot>().index;
            int newSlotIndex = index;

            GameManager.Instance.GetInventorySystem().SwapSlotContents(oldSlotIndex, newSlotIndex);

            hasDisplayedItem = true;
            childDisplayedItem = dropped;
        }
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetDisplayedItem(GameObject item)
    {
        hasDisplayedItem = true;
        item.AddComponent<DraggableUIItem>();
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
