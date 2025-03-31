using UnityEngine;

public enum BuildingType
{
    CraftStation,
    Decoration,
    ResourceCrafter
}

public interface IBuildable
{
    void Build(GameManager gm);
    bool CanBuild(GameManager gm);
    bool IsWaterBuilding();
}

[DisallowMultipleComponent]
public class Building<T> : WorldObject<T> , IBuildable, IInteractableWO where T : BuildingData
{
    protected override void Start()
    {
        base.Start();
    }

    public void Build(GameManager gm)
    {
        // Apply blueprint cost
        worldObjectData.blueprint.Build(gm.GetPlayerInventory());
        gm.SetBuildMode(true);
        gm.GetBuildingPlacer().SetBuilding(this.gameObject);
    }

    public bool CanBuild(GameManager gm)
    {
        return worldObjectData.blueprint.CanBuild(gm.GetPlayerInventory());
    }

    public virtual bool InteractWithWorldObject()
    {
        Debug.Log("Interacted with building!");
        return true;
    }

    public bool IsWaterBuilding()
    {
        return worldObjectData.waterBuilding;
    }
}