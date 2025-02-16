using UnityEngine;

public enum BuildingType
{
    CraftStation,
    Decoration,
    ResourceCrafter
}

[DisallowMultipleComponent]
public abstract class Building : WorldObject
{
    public int xdimension = 1;
    public int zdimension = 1;
    public BuildingBlueprint blueprint;

    public void Start()
    {
    }

    public void Build(GameManager gm)
    {
        // Apply blueprint cost
        blueprint.Build(gm.GetInventorySystem());
        gm.isBuildMode = true;
        gm.GetBuildingPlacer().buildingPrefab = this.gameObject;
    }

    public bool CanBuild(GameManager gm)
    {
        return blueprint.CanBuild(gm.GetInventorySystem());
    }
}