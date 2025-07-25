using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Blueprint;

public class BuildingBlueprintSlotUI : BaseHorizontalBlueprintUISlot
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void UpdateSlotUI()
    {
        BuildingBlueprint buildingBlueprint = blueprint as BuildingBlueprint;
        if (buildingBlueprint.CanBuild(GameManager.Instance.GetPlayerInventory()))
        {
            inventoryUISlotGO.GetComponent<Image>().color = baseColor;
        }
        else
        {
            inventoryUISlotGO.GetComponent<Image>().color = Color.red;
        }
    }

    protected override void InitUI()
    {
        BuildingData data = linkedGO.GetComponent<WorldObjectBase>().GetWorldObjectData() as BuildingData;

        // Init text
        nameText.text = data.objectName;
        requirementsText.text = "";
        foreach (ItemRequirement req in blueprint.requirements)
        {
            requirementsText.text += "x" + req.quantity + " " + req.item.name + "\n";
        }

        // Init icon
        GameObject icon = UIUtils.CreateIcon(data.uiIcon, -1, inventoryUISlotCounterPrefab, inventoryUISlotGO);
        icon.transform.SetParent(inventoryUISlotGO.transform, false);
        inventoryUISlotGO.GetComponent<InventoryUISlot>().SetDisplayedItem(icon, null, draggable: false);

        inventoryUISlotGO.GetComponent<Image>().raycastTarget = false;
        icon.GetComponent<Image>().raycastTarget = false;

        UpdateSlotUI();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        IBuildable building = linkedGO.GetComponent<IBuildable>();

        Debug.Log($"Building blueprint:");
        if (building.CanBuild(GameManager.Instance))
        {
            building.Build(GameManager.Instance);
        }
    }
}
