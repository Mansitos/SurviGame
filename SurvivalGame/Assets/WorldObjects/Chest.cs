using UnityEngine;
using System;

public class Chest : Building<BuildingData>, IUsableBuilding
{
    private InventorySystem inventory;
    public static event Action<GameObject> OnChestOpen;

    protected override void Start()
    {
        inventory = this.gameObject.GetComponent<InventorySystem>();
        base.Start();
    }

    override public bool InteractWithWorldObject()
    {
        return Use();
    }

    public InventorySystem GetInventory()
    {
        return inventory;
    }

    public bool Use()
    {
        Debug.Log("Opened chest");
        OnChestOpen?.Invoke(this.gameObject);
        return true;
    }
}
