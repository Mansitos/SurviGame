using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public float maxWeight = 100.0f;

    private PlayerQuickBar quickBar;
    private GameManager gm;
    private UIManager uim;

    private InventoryUI ui;
    public TextMeshProUGUI inventoryDebugUI;
    private QuickBarUI quickBarUI;

    private void Awake()
    {
        InitSlots();
    }
    
    void Start()
    {
        gm = GameManager.Instance;
        uim = gm.GetUIManager();
        ui = uim.GetInventoryUI();
        quickBarUI = uim.GetQuickBarUI();
        quickBar = gm.GetPlayerQuickBar();
    }

    public void Update()
    {
        UpdateDebugUI();
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return slots;
    }

    public void InitSlots()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot(null));
        }
    }

    public void SwapSlotContents(int fromIndex, int toIndex)
    {
        if (fromIndex != toIndex)
        {
            InventorySlot fromSlot = slots[fromIndex];
            InventorySlot toSlot = slots[toIndex];
            ItemInstance fromSlotItem = fromSlot.itemInstance;
            ItemInstance toSlotItem = toSlot.itemInstance;
            bool fromSlotEmpty = fromSlot.IsEmpty();
            bool toSlotEmpty = toSlot.IsEmpty();

            fromSlot.itemInstance = toSlotItem;
            toSlot.itemInstance = fromSlotItem;
            fromSlot.emptySlot = toSlotEmpty;
            toSlot.emptySlot = fromSlotEmpty;
        }
    }

    public void UpdateDebugUI()
    {
        if (inventoryDebugUI == null)
        {
            Debug.LogError("[InventorySystem] No TextMeshPro component linked.");
            return;
        }

        int selectedIndex = quickBar.GetSelectedSlotIndex() - 1;

        string inventoryDisplay = $"Weight: {GetCurrentWeight()}/{maxWeight}\n";
        inventoryDisplay += $"Slots: {GetFreeSlots()}/{maxSlots}\n";

        for (int i = 0; i < slots.Count; i++)
        {
            string slotDisplay;
            if (slots[i].IsEmpty())
            {
                slotDisplay = $"{i + 1}: empty"; // Display 'empty' for empty slots
            }
            else
            {
                slotDisplay = $"{i + 1}: {slots[i].itemInstance.ItemData.itemName} (x{slots[i].GetQuantity()})";
            }

            // Highlight the selected index
            if (i == selectedIndex)
            {
                slotDisplay = $"   <color=red>{slotDisplay}</color>"; // Change color to red
            }
            inventoryDisplay += $"{slotDisplay}\n";
        }

        inventoryDebugUI.text = inventoryDisplay; // Assuming inventoryDebugUI is a TextMeshPro component
    }

    public int GetFreeSlots()
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                count++;
            }
        }
        return count;
    }

    public bool WillRemovalFreeASlot(ItemInstance item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty())
            {
                if (slot.itemInstance.ItemData == item.ItemData && slot.GetQuantity() == item.Quantity)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanAddItemOnSlot(ItemInstance item, int slotIndex)
    {
        // TODO.
        return false;
    }

    public bool CanAddItem(ItemInstance item)
    {
        // First check is to check if weight would be too much!
        if (WillBeOverWeight(item.ItemData.weight * item.Quantity))
        {
            Debug.Log("[InventorySystem] Cannot add item: Over max weight capacity!");
            return false;
        }

        // Second check is to check if ItemInstance is already present in some Inventory Slots, if so just add
        foreach (InventorySlot slot in slots)
        {
            if (slot.CanAdd(item))
            {
                return true; // There's a slot that can take more of this item
            }
        }

        // Last check if there is a free Inventory Slot
        if (slots.Count < maxSlots)
        {
            return true; // There's room for a new slot
        }

        // No room for a new slot
        Debug.Log("[InventorySystem] Inventory full: No free slots available");
        return false;
    }

    public bool IsThereASlotWithItem(ItemData item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemInstance.ItemData == item)
            {
                return true;
            }
        }
        return false;
    }

    public bool TryAddItem(ItemInstance item, bool hasTargetSlot = false, int targetSlotIndex = -1)
    {
        // Ensure targetSlotIndex is mandatory only if hasTargetSlot is true
        if (hasTargetSlot && targetSlotIndex < 0)
        {
            Debug.LogError("[InventorySystem] Add operation aborted: targetSlotIndex must be specified if hasTargetSlot is true.");
            return false;
        }

        if (!CanAddItem(item))
        {
            Debug.Log("[InventorySystem] Add operation aborted: Cannot add item.");
            return false; // Ensure we do not proceed if we can't add the item
        }

        // First, try to add the item to an existing slot that already contains the same item.
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];
            if (!slot.IsEmpty() && slot.CanAdd(item))
            {
                slot.AddItem(item);
                if (hasTargetSlot)
                {
                    //SwapSlotContents(i, targetSlotIndex);
                }
                Debug.Log($"[InventorySystem] Added {item.Quantity} {item.ItemData.itemName}(s) to existing slot.");
                UpdateUI();
                return true;
            }
        }

        // If no suitable existing slot is found and there is room for a new slot, add a new slot
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];
            if (slot.CanAdd(item))
            {
                slot.AddItem(item);
                if (hasTargetSlot)
                {
                    SwapSlotContents(i, targetSlotIndex);
                }
                Debug.Log($"[InventorySystem] Added {item.Quantity} {item.ItemData.itemName}(s) to existing slot at index {i}.");
                UpdateUI();
                return true;
            }
        }


        // If there's no room for a new slot, log that the inventory is full
        Debug.Log("[InventorySystem] Inventory full: No free slots available.");
        return false;
    }

    public bool WillBeOverWeight(float additionalWeight)
    {
        return GetCurrentWeight() + additionalWeight > maxWeight;
    }

    public float GetCurrentWeight()
    {
        float currentWeight = 0;
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.itemInstance != null && slot.itemInstance.ItemData != null)
            {
                currentWeight += slot.GetWeight();
            }
        }
        return currentWeight;
    }

    public bool TryRemoveItem(ItemInstance item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty())
            {
                if (slot.itemInstance.ItemData == item.ItemData)
                {
                    if (slot.GetQuantity() >= item.Quantity)
                    {
                        slot.RemoveItem(item.Quantity);
                        UpdateUI();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsItemAvailable(ItemInstance item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty())
            {
                if (slot.itemInstance.ItemData == item.ItemData && slot.GetQuantity() >= item.Quantity)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void UpdateUI()
    {
        ui.UpdateUI();
        quickBarUI.UpdateUI();
    }

    public InventorySlot GetInventorySlotAtIndex(int index)
    {
        return slots[index];
    }
}
