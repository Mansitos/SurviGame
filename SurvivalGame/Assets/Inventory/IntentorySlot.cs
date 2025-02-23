using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemInstance itemInstance = null;
    public bool emptySlot = true;

    // Constructor
    public InventorySlot(ItemInstance itemInstance)
    {
        this.itemInstance = itemInstance;
        this.emptySlot = itemInstance is null;
    }

    public bool IsEmpty()
    {
        return emptySlot;
    }

    public bool CanAdd(ItemInstance newItem)
    {
        if (IsEmpty()){
            return true;
        }
        else
        {
            return itemInstance.ItemData == newItem.ItemData;
        }

    }

    public void AddItem(ItemInstance item)
    {
        if (!IsEmpty())
        {
            itemInstance.AddQuantity(item.Quantity);
        }
        else
        {
            itemInstance = item.Clone(); // We want a different reference!
            emptySlot = false;
        }
    }

    public void RemoveItem(int amount)
    {
        itemInstance.RemoveQuantity(amount);

        if (itemInstance.Quantity <= 0)
        {
            itemInstance = null;
            emptySlot = true;
        }
    }

    public float GetWeight()
    {
        return IsEmpty() ? 0 : itemInstance.ItemData.weight * itemInstance.Quantity;
    }

    public int GetQuantity()
    {
        return IsEmpty() ? 0 : itemInstance.Quantity;
    }
}
