using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 2;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public float maxWeight = 100.0f;

    public TextMeshProUGUI inventoryDebugUI;

    public void Update()
    {
        UpdateDebugUI();
    }


    public void UpdateDebugUI()
    {
        if (inventoryDebugUI == null)
        {
            Debug.LogError("[InventorySystem] No TextMeshPro component linked.");
            return;
        }

        string inventoryDisplay = $"Weight: {GetCurrentWeight()}/{maxWeight}\n";
        inventoryDisplay += $"Slots: {slots.Count}/{maxSlots}\n";

        Dictionary<string, int> itemQuantities = new Dictionary<string, int>();
        foreach (InventorySlot slot in slots)
        {
            string itemName = slot.itemInstance.ItemData.itemName;
            if (itemQuantities.ContainsKey(itemName))
            {
                itemQuantities[itemName] += slot.GetQuantity();
            }
            else
            {
                itemQuantities.Add(itemName, slot.GetQuantity());
            }
        }

        foreach (var item in itemQuantities)
        {
            inventoryDisplay += $"{item.Key}: {item.Value}\n";
        }

        inventoryDebugUI.text = inventoryDisplay; // Set the text of the TextMeshPro component
    }

    public int GetFreeSlots()
    {
        return maxSlots - slots.Count;
    }

    public bool willRemovalFreeASlot(ItemInstance item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemInstance.ItemData == item.ItemData && slot.GetQuantity() == item.Quantity)
            {
                return true;
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

        foreach (InventorySlot slot in slots)
        {
            if (slot.CanAdd(item))
            {
                slot.AddItem(item.Quantity);
                Debug.Log($"[InventorySystem] Added {item.Quantity} {item.ItemData.itemName}(s) to existing slot.");
                return true;
            }
        }

        // If no existing slot can hold the item, add a new slot
        slots.Add(new InventorySlot(item));
        Debug.Log($"[InventorySystem] Added {item.Quantity} {item.ItemData.itemName}(s) to new slot.");
        return true;
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
            currentWeight += slot.GetWeight();
        }
        return currentWeight;
    }

    public bool TryRemoveItem(ItemInstance item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemInstance.ItemData == item.ItemData)
            {
                if (slot.GetQuantity() >= item.Quantity)
                {
                    slot.RemoveItem(item.Quantity);
                    if (slot.GetQuantity() == 0) slots.Remove(slot);
                    return true;
                }
            }
        }
        return false;
    }

    public bool isItemAvailable(ItemInstance item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemInstance.ItemData == item.ItemData && slot.GetQuantity() >= item.Quantity)
            {
                return true;
            }
        }
        return false;
    }
}
