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
    [SerializeField] private string itemName;
    [SerializeField] private float weight;

    public string Name => itemName;
    public float Weight => weight;

    // Returns the class type of the item (must be overridden by subclasses)
    public abstract ItemType GetItemType();

    // Check conditions for main action and perform it if possible
    public bool TryMainAction()
    {
        if (true)
        {
            return PerformMainAction();
        }
        else
        {
            return false;
        }
    }
    
    // Returns true if main action can be performed
    public abstract bool PerformMainAction();

    // Checks if this item is of a specific ItemType
    public bool IsItemOfType(ItemType type)
    {
        return GetItemType() == type;
    }
}
