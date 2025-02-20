using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUISlotPrefab;
    [SerializeField] GameObject grid;
    [SerializeField] List<GameObject> inventoryUISlots;

    private GameManager gm;
    private InventorySystem inventory;

    void Start()
    {
        gm = GameManager.Instance;
        inventory = gm.GetInventorySystem();
        InitInventorySlots();
        UpdateInventorySlots();
    }

    private void InitInventorySlots()
    {
        int numSlots = inventory.maxSlots;
        for (int i = 0; i < numSlots; i++)
        {
            GameObject slot = Instantiate(inventoryUISlotPrefab, grid.transform);
            inventoryUISlots.Add(slot);
        }
    }

    private void UpdateInventorySlots()
    {
        List<InventorySlot> inventorySlots = inventory.GetInventorySlots();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot inventorySlot = inventorySlots[i];
            InventoryUISlot UISlot = inventoryUISlots[i].GetComponent<InventoryUISlot>();

            // Clear any previous item display
            UISlot.ClearSlot();

            if (!inventorySlot.IsEmpty())
            {
                Sprite itemSprite = inventorySlot.itemInstance.ItemData.uiIcon;
                int itemQuantity = inventorySlot.itemInstance.Quantity;

                // Create a new GameObject for the item icon
                GameObject itemIconObject = new GameObject("ItemIcon");
                itemIconObject.transform.SetParent(UISlot.transform, false); // Attach to the slot UI

                // Add Image component and assign sprite
                Image itemImage = itemIconObject.AddComponent<Image>();
                itemImage.sprite = itemSprite;

                RectTransform rectTransform = itemIconObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(64, 64);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = Vector2.zero;

                UISlot.SetDisplayedItem(itemIconObject);

                //UISlot.SetItemCount(itemQuantity);
            }
        }
    }


    void Update()
    {
        //UpdateInventorySlots();
    }
}
