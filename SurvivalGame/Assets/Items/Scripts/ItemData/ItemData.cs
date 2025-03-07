using UnityEngine;

public enum ItemType
{
    Item,
    Tool,
    Resource,
    Food,
    Weapon,
    Consumable
}

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public float weight;
    public string itemDescription;

    public GameObject worldPrefab; // Prefab for when dropped into the world
    public Sprite uiIcon; // Icon for inventory UI

    public ItemType GetItemType()
    {
        return itemType;
    }

    public bool IsItemOfType(ItemType type)
    {
        return GetItemType() == type;
    }
}
