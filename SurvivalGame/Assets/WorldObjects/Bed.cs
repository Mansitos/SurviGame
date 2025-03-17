using UnityEngine;

public class Bed : Building<BuildingData>, IUsableBuilding
{
    protected override void Start()
    {
        base.Start();
    }

    override public bool InteractWithWorldObject()
    {
        return Use();
    }

    public bool Use()
    {
        Debug.Log("BED used to sleep....");
        gm.GetGameTimeManager().HandleEndDay(fromSleep: true);
        return true;
    }
}
