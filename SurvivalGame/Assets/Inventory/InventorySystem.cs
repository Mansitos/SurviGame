using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int maxSlots = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public float maxWeight = 100.0f;

    private GameManager gm;

    public static event Action OnInventoryUpdated;

    private void Awake()
    {
        InitSlots();
    }
    
    void Start()
    {
        gm = GameManager.Instance;
    }

    public void Update()
    {
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

        UpdateUI();
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

    public void UpdateUI()
    {
        OnInventoryUpdated?.Invoke();
    }

    public InventorySlot GetInventorySlotAtIndex(int index)
    {
        return slots[index];
    }

    public List<InventorySlot> GetSlotsWithItemType(ItemType type)
    {
        List<InventorySlot> slotsWithFood = new List<InventorySlot>();

        foreach(InventorySlot slot in slots)
        {
            if (!slot.IsEmpty())
            {
                if (slot.itemInstance.ItemData.IsItemOfType(type))
                {
                    slotsWithFood.Add(slot);
                }
            }
        }

        return slotsWithFood;
    }
}
