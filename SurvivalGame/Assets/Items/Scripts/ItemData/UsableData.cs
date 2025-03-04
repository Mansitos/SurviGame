using UnityEngine;

public abstract class UsableData : ItemData, IUsableItem
{
    public bool PerformMainAction(GameManager gm)
    {
        Debug.Log("Use Data main aciton");
        return true;
    }
}
