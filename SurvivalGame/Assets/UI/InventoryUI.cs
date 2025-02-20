using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUISlotPrefab;
    [SerializeField] GameObject inventoryUISlotCounterPrefab;
    [SerializeField] GameObject grid;
    private GameObject quantityGrid;
    [SerializeField] List<GameObject> inventoryUISlots;
    [SerializeField] List<TextMeshProUGUI> inventoryUIQuantitySlots;

    private GameManager gm;
    private InventorySystem inventory;
    private PlayerMovementInputHandler inputHandler;  

    void Start()
    {
        gm = GameManager.Instance;
        inputHandler = gm.GetPlayerGO().GetComponent<PlayerMovementInputHandler>();
        inventory = gm.GetInventorySystem();
        InitInventorySlots();
        UpdateInventorySlots();
        UpdateActiveStatus();
    }

    private void InitInventorySlots()
    {
        quantityGrid = Instantiate(grid, grid.transform.parent);
        quantityGrid.name = grid.name + "_quantity";
        Destroy(quantityGrid.GetComponent<Image>());

        int numSlots = inventory.maxSlots;
        for (int i = 0; i < numSlots; i++)
        {
            GameObject slot = Instantiate(inventoryUISlotPrefab, grid.transform);
            slot.GetComponent<InventoryUISlot>().SetIndex(i);
            inventoryUISlots.Add(slot);

            GameObject slotCounter = Instantiate(inventoryUISlotCounterPrefab, quantityGrid.transform);
            TextMeshProUGUI counter = slotCounter.GetComponent<TextMeshProUGUI>();
            counter.raycastTarget = false;
            inventoryUIQuantitySlots.Add(counter);
        }
    }

    public void UpdateInventorySlots()
    {
        List<InventorySlot> inventorySlots = inventory.GetInventorySlots();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            InventorySlot inventorySlot = inventorySlots[i];
            InventoryUISlot UISlot = inventoryUISlots[i].GetComponent<InventoryUISlot>();
            TextMeshProUGUI counter = inventoryUIQuantitySlots[i];

            // Clear any previous item display
            UISlot.ClearSlot(destroyChild:true);
            counter.text = "";

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

                counter.text = itemQuantity.ToString();

                UISlot.SetDisplayedItem(itemIconObject);

                //TODO: count
                //UISlot.SetItemCount(itemQuantity);
            }
        }
    }
    
    void Update()
    {
        CheckInventoryModeInput();
    }

    private void CheckInventoryModeInput()
    {
        if (inputHandler.WasInventoryModePressedThisFrame())
        {
            gm.SetInventoryMode(!gm.IsInInventoryMode());
            UpdateActiveStatus();

            if (gm.IsInInventoryMode())
            {
                UpdateInventorySlots();
            }
        }
    }

    private void UpdateActiveStatus()
    {
        grid.SetActive(gm.IsInInventoryMode());
        quantityGrid.SetActive(gm.IsInInventoryMode());
    }
}
