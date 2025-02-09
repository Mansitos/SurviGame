using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private ItemInstance itemInstance;

    // Factory method to create a dropped item
    public static DroppedItem Spawn(ItemInstance itemInstance, Vector3 spawnPosition)
    {
        if (itemInstance.ItemData.worldPrefab == null)
        {
            Debug.LogError($"[DroppedItem] Missing prefab for {itemInstance.ItemData.itemName}.");
            return null;
        }

        // Instantiate the item in the world
        GameObject droppedItemGO = Instantiate(itemInstance.ItemData.worldPrefab, spawnPosition, Quaternion.identity);

        // Ensure it has a DroppedItem component
        DroppedItem droppedItem = droppedItemGO.GetComponent<DroppedItem>();
        if (droppedItem == null)
        {
            droppedItem = droppedItemGO.AddComponent<DroppedItem>();
        }

        // Initialize it with the item instance
        droppedItem.Initialize(itemInstance);

        return droppedItem;
    }

    public void Initialize(ItemInstance item)
    {
        itemInstance = item;
    }

    public ItemInstance GetItemInstance()
    {
        return itemInstance;
    }
}
