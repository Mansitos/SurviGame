using UnityEngine;

public class BuildingUI : BaseScrollableCraftUI
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void PopulateSlots()
    {
        foreach (BuildingBlueprint buildingBlueprint in craftBlueprints)
        {
            GameObject slot = Instantiate(slotUIPrefab, verticalGrid.transform);
            slot.GetComponent<BuildingBlueprintSlotUI>().SetBluePrint(buildingBlueprint);
        }
    }
}
