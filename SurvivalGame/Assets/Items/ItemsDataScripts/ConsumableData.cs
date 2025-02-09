using UnityEngine;

public abstract class ConsumableData : ItemData, IUsableItem
{
    public bool PerformMainAction(GameManager gm)
    {
        Debug.Log("Consumable main aciton");
        return true;
    }
}
