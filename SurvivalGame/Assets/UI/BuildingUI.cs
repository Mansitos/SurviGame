using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : BaseScrollableCraftUI
{

    [SerializeField] protected List<GameObject> buildingsGO = new List<GameObject>();

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
        foreach (GameObject building in buildingsGO)
        {
            BuildingData data = building.GetComponent<WorldObjectBase>().GetWorldObjectData() as BuildingData;
            BuildingBlueprint buildingBlueprint = data.blueprint;
            GameObject slot = Instantiate(slotUIPrefab, verticalGrid.transform);
            slot.GetComponent<BuildingBlueprintSlotUI>().SetBluePrint(buildingBlueprint, skipUpdateUI: true);
            slot.GetComponent<BuildingBlueprintSlotUI>().SetLinkedGameObject(building);
        }
    }
}
