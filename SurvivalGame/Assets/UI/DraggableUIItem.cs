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
        UIManager.Instance.GetInventoryUI().UpdateUI();
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
            if (linkedItemInstance != null)
            {
                if (!mouseSlot.hasDisplayedItem)
                {
                    GameObject icon = UIUtils.CreateIcon(linkedItemInstance.ItemData.uiIcon, -1, null, mouseSlotGO);
                    icon.transform.SetParent(mouseSlotGO.transform, false);
                    mouseSlotGO.GetComponent<InventoryUISlot>().SetDisplayedItem(icon, new ItemInstance(linkedItemInstance.ItemData, 1));
                    UIManager.Instance.GetMouseInventorySlot().SetReferenceUISlot(this.parent.GetComponent<InventoryUISlot>()); ;
                    linkedItemInstance.RemoveQuantity(1);
                }
                else
                {
                    if (UIManager.Instance.GetMouseInventorySlot().referenceSlot == this.parent.GetComponent<InventoryUISlot>())
                    {
                        linkedItemInstance.RemoveQuantity(1);
                        mouseSlot.childDisplayedItem.GetComponent<DraggableUIItem>().linkedItemInstance.AddQuantity(1);
                    }
                    else
                    {
                        Debug.Log("Cannot add +1 from different slot!");
                    }
                    
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.parent.GetComponent<InventoryUISlot>().OnPointerClick(eventData);
        }

        UIManager.Instance.GetInventoryUI().UpdateUI();
    }
}
