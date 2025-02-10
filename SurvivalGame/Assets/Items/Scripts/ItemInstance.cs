using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData ItemData { get; private set; }
    public int Quantity { get; private set; }

    public ItemInstance(ItemData itemData, int quantity = 1)
    {
        ItemData = itemData;
        Quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        Quantity += amount;
    }

    public bool RemoveQuantity(int amount)
    {
        if (Quantity >= amount)
        {
            Quantity -= amount;
            return true;
        }
        return false;
    }

    public bool PerformMainAction(GameManager gameManager)
    {
        if (ItemData is IUsableItem usableItem)
        {
            return usableItem.PerformMainAction(gameManager);
        }
        {
            Debug.LogWarning("[ItemInstance] Cannot perform main action on a non-IUsableItem!");
            return false;
        }
    }
}
