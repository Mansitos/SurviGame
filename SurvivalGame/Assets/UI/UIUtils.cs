using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class UIUtils
{
    public static GameObject CreateItemIcon(ItemInstance itemInstance, GameObject counterPrefab, GameObject parent)
    {
        Sprite itemSprite = itemInstance.ItemData.uiIcon;
        int itemQuantity = itemInstance.Quantity;

        return CreateIcon(itemSprite, itemQuantity, counterPrefab, parent);
    }

    public static GameObject CreateIcon(Sprite itemSprite, int quantity, GameObject counterPrefab, GameObject parent)
    {
        // Create a new GameObject for the item icon
        GameObject itemIconObject = new GameObject("ItemIcon");

        // Add Image component and assign sprite
        Image itemImage = itemIconObject.AddComponent<Image>();
        itemImage.sprite = itemSprite;

        RectTransform rectTransform = itemIconObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(64, 64);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        // Spawning the counter
        if (quantity > 0)
        {
            GameObject slotCounter = Object.Instantiate(counterPrefab, parent.transform);
            TextMeshProUGUI counter = slotCounter.GetComponent<TextMeshProUGUI>();
            counter.raycastTarget = false;
            slotCounter.transform.SetParent(itemIconObject.transform, false);
            counter.text = quantity.ToString();
        }

        return itemIconObject;
    }

}