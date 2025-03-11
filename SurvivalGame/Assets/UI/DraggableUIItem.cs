using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform parent;
    public InventorySlot linkedInventorySlot;

    private ItemsTooltipUI tooltip;
    private bool isMouseOver;

    private void Start()
    {
        tooltip = UIManager.Instance.GetItemsTooltipUI();
    }

    private void Update()
    {
        UpdateTooltipPosition();
    }

    public void UpdateTooltipPosition()
    {
        if (isMouseOver && tooltip.IsVisible())
        {
            tooltip.UpdateTooltipPosition(Mouse.current.position.ReadValue());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!UIManager.Instance.GetMouseInventorySlot().GetInventorySlot().IsEmpty())
        {
            return;
        }

        parent = this.transform.parent;
        this.transform.SetParent(this.transform.root);
        this.transform.SetAsLastSibling();
        this.GetComponent<Image>().raycastTarget = false;

        tooltip.HideTooltip();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!UIManager.Instance.GetMouseInventorySlot().GetInventorySlot().IsEmpty())
        {
            return;
        }

        if (Mouse.current != null)
        {
            this.transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!UIManager.Instance.GetMouseInventorySlot().GetInventorySlot().IsEmpty())
        {
            return;
        }

        this.transform.SetParent(parent);
        this.GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (linkedInventorySlot.itemInstance != null)
        {
            isMouseOver = true;
            tooltip.ShowTooltip(linkedInventorySlot.itemInstance, Mouse.current.position.ReadValue());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        tooltip.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MouseInventoryUISlot mouseInventory = UIManager.Instance.GetMouseInventorySlot();
        InventorySlot mouseInventorySlot = mouseInventory.GetInventorySlot();
        InventoryUISlot mouseInventoryUISlot = mouseInventory.GetInventoryUISlot();

        ItemInstance itemToAdd = new ItemInstance(linkedInventorySlot.itemInstance.ItemData, 1);

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (linkedInventorySlot.itemInstance != null) // If this UI Slot has an instance attached
            {
                if (mouseInventorySlot.IsEmpty())
                {
                    // Set the origin slot from where the items have been taken
                    mouseInventory.SetOriginReferenceSlot(this.parent.GetComponent<InventoryUISlot>().GetLinkedInventorySlot());

                    // Remove quantity from original item instance
                    linkedInventorySlot.RemoveItem(1);
                    mouseInventorySlot.AddItem(itemToAdd);

                    // Update inventory slot ui
                    mouseInventoryUISlot.SetDisplayedItemIcon(mouseInventorySlot);
                }
                else
                {
                    // When mouse slot is already with some item, additional +1 can arrive only from the same origin slot
                    if (mouseInventory.originInventorySlot.itemInstance.ItemData == this.parent.GetComponent<InventoryUISlot>().GetLinkedInventorySlot().itemInstance.ItemData)
                    {
                        linkedInventorySlot.RemoveItem(1);
                        mouseInventorySlot.AddItem(itemToAdd);
                    }
                    else
                    {
                        Debug.Log("DIFFERENT ORIGIN SLOT");
                    }
                }
            }
            else
            {
                Debug.LogWarning("No instances attached but has an icon.. should never happen!");
            }
        }

        // Redirect the handling to the underlying InventoryUISlot
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.parent.GetComponent<InventoryUISlot>().OnPointerClick(eventData);
        }

        // Check if clear slot is needed
        if (linkedInventorySlot.itemInstance.Quantity <= 0)
        {
            Debug.Log("aaaa");
            this.parent.GetComponent<InventoryUISlot>().ClearSlot(destroyChild: true);
        }

        GameManager.Instance.GetPlayerInventory().UpdateUI();
        UIManager.Instance.GetChestUI().UpdateUI();
    }
}
