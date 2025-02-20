using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlot : MonoBehaviour, IDropHandler
{
    public GameObject childDisplayedItem;
    public bool hasDisplayedItem;

    public void OnDrop(PointerEventData eventData)
    {
        if (!hasDisplayedItem)
        {
            Debug.Log("B");
            GameObject dropped = eventData.pointerDrag;
            DraggableUIItem draggableItem = dropped.GetComponent<DraggableUIItem>();

            Transform oldParent = draggableItem.parent;
            draggableItem.parent = transform;

            oldParent.GetComponent<InventoryUISlot>().ClearSlot();

            hasDisplayedItem = true;
            childDisplayedItem = dropped;
        }
    }

    public void SetDisplayedItem(GameObject item)
    {
        hasDisplayedItem = true;
        item.AddComponent<DraggableUIItem>();
        childDisplayedItem = item;
    }

    public void ClearSlot()
    {
        childDisplayedItem = null;
        hasDisplayedItem = false;
    }
}
