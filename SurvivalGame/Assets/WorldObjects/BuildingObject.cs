using UnityEngine;

[DisallowMultipleComponent]
public class Building : WorldObject
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
        blueprint.Build(gm.getInventorySystem());
        gm.isBuildMode = true;
        gm.getBuildingPlacer().buildingPrefab = this.gameObject;
    }

    public bool CanBuild(GameManager gm)
    {
        return blueprint.CanBuild(gm.getInventorySystem());
    }
}