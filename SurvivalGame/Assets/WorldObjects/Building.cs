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
}

[DisallowMultipleComponent]
public class Building<T> : WorldObject<T> , IBuildable where T : BuildingData
{
    protected override void Start()
    {
        base.Start();
    }

    public void Build(GameManager gm)
    {
        // Apply blueprint cost
        worldObjectData.blueprint.Build(gm.GetInventorySystem());
        gm.SetBuildMode(true);
        gm.GetBuildingPlacer().SetBuilding(this.gameObject);
    }

    public bool CanBuild(GameManager gm)
    {
        return worldObjectData.blueprint.CanBuild(gm.GetInventorySystem());
    }

    public virtual bool InteractWithBuilding()
    {
        Debug.Log("Interacted with building!");
        return true;
    }
}