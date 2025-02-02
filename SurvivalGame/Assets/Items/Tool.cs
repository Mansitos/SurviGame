using UnityEngine;

public enum ToolType
{
    Pickaxe,
    Axe,
    Shovel
}

public abstract class Tool : Item
{
    [SerializeField] private ToolType toolCategory;

    public ToolType ToolCategory => toolCategory;

    public override ItemType GetItemType()
    {
        return ItemType.Tool;
    }
}