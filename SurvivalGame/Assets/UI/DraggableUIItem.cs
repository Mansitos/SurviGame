using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform parent;
    public ItemInstance linkedItemInstance;

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

        if (Mouse.current != null)
        {
            this.transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        this.transform.SetParent(parent);
        this.GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (linkedItemInstance != null)
        {
            isMouseOver = true;
            tooltip.ShowTooltip(linkedItemInstance, Mouse.current.position.ReadValue());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        tooltip.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject mouseSlotGO = UIManager.Instance.GetMouseInventorySlot().slot;
        InventoryUISlot mouseSlot = mouseSlotGO.GetComponent<InventoryUISlot>();

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (linkedItemInstance != null) // If this UI Slot has an instance attached
            {
                if (!mouseSlot.IsDisplayingAnItemIcon())
                {
                    // Mouse "inventory" is empty, create new icon and populate its slot.
                    GameObject icon = UIUtils.CreateIcon(linkedItemInstance.ItemData.uiIcon, -1, null, mouseSlotGO);
                    icon.transform.SetParent(mouseSlotGO.transform, false);
                    mouseSlotGO.GetComponent<InventoryUISlot>().SetDisplayedItem(icon, new ItemInstance(linkedItemInstance.ItemData, 1));
                    
                    // Set the origin slot from where the items have been taken
                    UIManager.Instance.GetMouseInventorySlot().SetOriginReferenceSlot(this.parent.GetComponent<InventoryUISlot>()); ;
                    
                    // Remove quantity from original item instance
                    linkedItemInstance.RemoveQuantity(1);

                    //TODO: check if now empty? if 0 -> clear slot maybe
                }
                else
                {
                    // TODO: IMPROVE: same item! looser condition.
                    // When mouse slot is already with some item, additional +1 can arrive only from the same origin slot
                    if (UIManager.Instance.GetMouseInventorySlot().referenceUISlot == this.parent.GetComponent<InventoryUISlot>())
                    {
                        linkedItemInstance.RemoveQuantity(1);
                        //TODO: check if now empty? if 0 -> clear slot maybe

                        mouseSlot.childDisplayedItem.GetComponent<DraggableUIItem>().linkedItemInstance.AddQuantity(1);
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


        // TODO: Get a cleaner way to invoke update ui event...
        GameManager.Instance.GetPlayerInventory().UpdateUI();
    }
}
