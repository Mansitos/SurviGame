using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DraggableUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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
        parent = this.transform.parent;
        this.transform.SetParent(this.transform.root);
        this.transform.SetAsLastSibling();
        this.GetComponent<Image>().raycastTarget = false;

        tooltip.HideTooltip();
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
}
