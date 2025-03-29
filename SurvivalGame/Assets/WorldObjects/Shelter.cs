public class Shelter : Building<BuildingData>, IUsableBuilding
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
        gm.GetGameTimeManager().HandleEndDay(fromSleep: true);
        return true;
    }
}
