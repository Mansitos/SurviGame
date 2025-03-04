public enum CraftStationType
{
    None,
    CraftStation,
    Anvil,
    WoodStation,
    Kitchen,
    CampFire
}

public class CraftStation : Building<BuildingData>
{
    public CraftStationType type;

    public bool IsCraftStationOfType(CraftStationType type)
    {
        return this.type == type;
    }
}
