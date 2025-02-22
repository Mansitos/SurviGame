using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parent;
    public ItemInstance linkedItemInstance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parent = this.transform.parent;
        this.transform.SetParent(this.transform.root);
        this.transform.SetAsLastSibling();
        this.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Mouse.current != null)
        {
            this.transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // NB: At this point the parent could have changed due to InventoryUISlot.OnDrop()
        this.transform.SetParent(parent);
        this.GetComponent<Image>().raycastTarget = true;
        GameManager.Instance.GetUIManager().GetInventoryUI().UpdateSlots();
    }
}