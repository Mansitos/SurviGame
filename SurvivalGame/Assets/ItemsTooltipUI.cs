using UnityEngine;
using TMPro;

public class ItemsTooltipUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemTypeText;
    public TextMeshProUGUI itemDescText;
    public RectTransform tooltipPanel;

    private CanvasGroup canvasGroup;
    private bool isVisible = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        HideTooltip();
    }

    public void ShowTooltip(ItemInstance item, Vector2 mousePosition)
    {
        itemNameText.text = item.ItemData.itemName;
        itemTypeText.text = item.ItemData.itemType.ToString();
        itemDescText.text = item.ItemData.itemDescription;
        UpdateTooltipPosition(mousePosition);

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        isVisible = true;
    }

    public void HideTooltip()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        isVisible = false;
    }

    public void UpdateTooltipPosition(Vector2 mousePosition)
    {
        Vector2 adjustedPosition = mousePosition + new Vector2(20, -20);
        tooltipPanel.position = adjustedPosition;
    }

    public bool IsVisible()
    {
        return isVisible;
    }
}
