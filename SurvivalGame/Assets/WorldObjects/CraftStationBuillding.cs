public enum CraftStationType
{
    None,
    CraftStation,
    Anvil,
    WoodStation,
    Kitchen,
    CampFire
}

public class CraftStationBuillding : Building
{
    public CraftStationType type;
    //private BuildingType buildingType = BuildingType.CraftStation;

    public bool IsCraftStationOfType(CraftStationType type)
    {
        return this.type == type;
    }
}
