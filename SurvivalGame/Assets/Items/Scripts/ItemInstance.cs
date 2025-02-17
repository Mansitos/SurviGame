using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;

    public ItemInstance(ItemData itemData, int quantity = 1)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public bool RemoveQuantity(int amount)
    {
        if (quantity >= amount)
        {
            quantity -= amount;
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
