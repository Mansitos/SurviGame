using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseInventoryUI : MonoBehaviour
{
    [SerializeField] protected GameObject grid;
    [SerializeField] protected GameObject inventoryUISlotPrefab;
    [SerializeField] protected GameObject inventoryUISlotCounterPrefab;
    protected virtual bool ItemsAreDraggable => true; // default
    [SerializeField] protected List<GameObject> uiSlots = new List<GameObject>();

    protected GameManager gm;
    protected InventorySystem inventory;
    protected int numSlots;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        inventory = gm.GetInventorySystem();
        InitSlots();
        UpdateSlots();
        UpdateActiveStatus();
    }

    protected abstract void InitSlots(); // Each UI type initializes differently

    protected void PopulateSlots(int numToPopulate)
    {
        for (int i = 0; i < numToPopulate; i++)
        {
            GameObject slot = Instantiate(inventoryUISlotPrefab, grid.transform);
            slot.GetComponent<InventoryUISlot>().SetIndex(i);
            uiSlots.Add(slot);
        }
    }

    public void UpdateSlots()
    {
        List<InventorySlot> inventorySlots = inventory.GetInventorySlots();

        for (int i = 0; i < numSlots; i++)
        {
            InventorySlot inventorySlot = inventorySlots[i];
            InventoryUISlot UISlot = uiSlots[i].GetComponent<InventoryUISlot>();

            // Clear any previous item display
            UISlot.ClearSlot(destroyChild: true);

            if (!inventorySlot.IsEmpty())
            {
                GameObject itemIconObject = CreateItemIcon(UISlot, inventorySlot);
                UISlot.SetDisplayedItem(itemIconObject, draggable: ItemsAreDraggable);
            }
        }
    }

    protected GameObject CreateItemIcon(InventoryUISlot uiSlot, InventorySlot inventorySlot)
    {
        Sprite itemSprite = inventorySlot.itemInstance.ItemData.uiIcon;
        int itemQuantity = inventorySlot.itemInstance.Quantity;

        // Create a new GameObject for the item icon
        GameObject itemIconObject = new GameObject("ItemIcon");
        itemIconObject.transform.SetParent(uiSlot.transform, false);

        // Add Image component and assign sprite
        Image itemImage = itemIconObject.AddComponent<Image>();
        itemImage.sprite = itemSprite;

        RectTransform rectTransform = itemIconObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(64, 64);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        // Spawning the counter
        GameObject slotCounter = Instantiate(inventoryUISlotCounterPrefab, grid.transform);
        TextMeshProUGUI counter = slotCounter.GetComponent<TextMeshProUGUI>();
        counter.raycastTarget = false;
        slotCounter.transform.SetParent(itemIconObject.transform, false);
        counter.text = itemQuantity.ToString();

        return itemIconObject;
    }

    protected virtual void UpdateActiveStatus()
    {
        grid.SetActive(gm.IsInInventoryMode());
    }
}
