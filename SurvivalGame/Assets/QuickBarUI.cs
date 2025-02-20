using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class QuickBarUI : MonoBehaviour
{
    [SerializeField] GameObject grid;

    private GameManager gm;
    private InventorySystem inventory;
    private PlayerQuickBar quickBar;
    [SerializeField] GameObject inventoryUISlotPrefab;
    [SerializeField] GameObject inventoryUISlotCounterPrefab;
    [SerializeField] List<GameObject> quickBarUISlots;
    private int numSlots = 10;

    void Start()
    {
        gm = GameManager.Instance;
        inventory = gm.GetInventorySystem();
        quickBar = gm.GetPlayerQuickBar();
        UpdateActiveStatus();
        InitQuickBarSlots();
    }

    private void Update()
    {
        UpdateActiveStatus();
    }

    private void InitQuickBarSlots()
    {
        for (int i = 0; i < numSlots; i++)
        {
            GameObject slot = Instantiate(inventoryUISlotPrefab, grid.transform);
            slot.GetComponent<InventoryUISlot>().SetIndex(i);
            quickBarUISlots.Add(slot);
        }
    }

    public void UpdateQuickBarSlots()
    {
        List<InventorySlot> inventorySlots = inventory.GetInventorySlots();

        for (int i = 0; i < numSlots; i++)
        {
            InventorySlot inventorySlot = inventorySlots[i];
            InventoryUISlot UISlot = quickBarUISlots[i].GetComponent<InventoryUISlot>();

            // Clear any previous item display
            UISlot.ClearSlot(destroyChild: true);

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

                UISlot.SetDisplayedItem(itemIconObject, draggable: false);

                //TODO: count
                //UISlot.SetItemCount(itemQuantity);
            }
        }
    }

    private void UpdateActiveStatus()
    {
        bool newStatus = !gm.IsInInventoryMode();
        bool oldStatus = grid.activeSelf;

        grid.SetActive(newStatus);

        if (oldStatus != newStatus)
        {
            Debug.Log("Need to update quickbar UI");
            UpdateQuickBarSlots();
        }
    }
}
