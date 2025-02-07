using UnityEngine;

public enum ToolType
{
    Pickaxe,
    Axe,
    Shovel
}

public class Tool : Item
{
    [SerializeField] private ToolType toolCategory;

    public ToolType ToolCategory => toolCategory;

    public override ItemType GetItemType()
    {
        return ItemType.Tool;
    }

    public override bool PerformMainAction()
    {
        Debug.Log("TOOL MAIN ACTION");
        return true;
    }
}