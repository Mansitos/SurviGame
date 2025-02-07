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

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected string itemName;
    [SerializeField] protected float weight;

    public string Name => itemName;
    public float Weight => weight;

    // Returns the class type of the item (must be overridden by subclasses)
    public abstract ItemType GetItemType();
    
    // Returns true if main action can be performed
    public abstract bool PerformMainAction(GameManager gm);

    // Checks if this item is of a specific ItemType
    public bool IsItemOfType(ItemType type)
    {
        return GetItemType() == type;
    }
}
