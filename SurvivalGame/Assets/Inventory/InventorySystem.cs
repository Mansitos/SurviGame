using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public float maxWeight = 100.0f;

    public TextMeshProUGUI inventoryDebugUI;

    private void Awake()
    {
        InitSlots();
    }
    void Start()
    {

    }

    public void Update()
    {
        UpdateDebugUI();
    }

    public void InitSlots()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot(null));
        }
    }

    public void UpdateDebugUI()
    {
        if (inventoryDebugUI == null)
        {
            Debug.LogError("[InventorySystem] No TextMeshPro component linked.");
            return;
        }

        string inventoryDisplay = $"Weight: {GetCurrentWeight()}/{maxWeight}\n";
        inventoryDisplay += $"Slots: {GetFreeSlots()}/{maxSlots}\n";

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                inventoryDisplay += $"{i + 1}: empty\n"; // Display 'empty' for empty slots
            }
            else
            {
                inventoryDisplay += $"{i + 1}: {slots[i].itemInstance.ItemData.itemName} (x{slots[i].GetQuantity()})\n";
            }
        }

        inventoryDebugUI.text = inventoryDisplay; // Set the text of the TextMeshPro component
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

    public bool TryAddItem(ItemInstance item)
    {
        if (!CanAddItem(item))
        {
            Debug.Log("[InventorySystem] Add operation aborted: Cannot add item.");
            return false; // Ensure we do not proceed if we can't add the item
        }

        // First, try to add the item to an existing slot that already contains the same item.
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.CanAdd(item))
            {
                slot.AddItem(item);
                Debug.Log($"[InventorySystem] Added {item.Quantity} {item.ItemData.itemName}(s) to existing slot.");
                return true;
            }
        }

        // If no suitable existing slot is found and there is room for a new slot, add a new slot
        foreach (InventorySlot slot in slots)
        {
            if (slot.CanAdd(item))
            {
                slot.AddItem(item);
                Debug.Log($"[InventorySystem] Added {item.Quantity} {item.ItemData.itemName}(s) to existing slot.");
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

    private float GetCurrentWeight()
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
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool isItemAvailable(ItemInstance item)
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
}
