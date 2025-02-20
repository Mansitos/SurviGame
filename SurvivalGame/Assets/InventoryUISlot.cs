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

            Debug.Log(draggableItem.parent);

            draggableItem.GetComponent<DraggableUIItem>().parent.GetComponent<InventoryUISlot>().ClearSlot();

            draggableItem.parent = transform;
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
        Destroy(childDisplayedItem);
        hasDisplayedItem = false;
    }
}
