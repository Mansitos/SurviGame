using UnityEngine;

public class Bed : Building, IUsableBuilding
{
    protected override void Start()
    {
        base.Start();
    }

    override public bool InteractWithBuilding()
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
