using UnityEngine;

public class Resource : Item
{
    public override ItemType GetItemType()
    {
        return ItemType.Resource;
    }

    public override bool PerformMainAction()
    {
        Debug.Log("Resource has no main action!");
        return true;
    }
}
