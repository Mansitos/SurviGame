using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemInstance itemInstance;

    public InventorySlot(ItemInstance itemInstance)
    {
        this.itemInstance = itemInstance;
    }

    public bool CanAdd(ItemInstance newItem)
    {
        return itemInstance.ItemData == newItem.ItemData;
    }

    public void AddItem(int amount)
    {
        itemInstance.AddQuantity(amount);
    }

    public void RemoveItem(int amount)
    {
        itemInstance.RemoveQuantity(amount);
    }

    public float GetWeight()
    {
        return itemInstance.ItemData.weight * itemInstance.Quantity;
    }

    public int GetQuantity()
    {
        return itemInstance.Quantity;
    }
}
