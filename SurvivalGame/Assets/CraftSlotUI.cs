using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CraftBlueprint;

public class CraftSlotUI : BaseHorizontalBlueprintUISlot
{
    [SerializeField] GameObject inventoryUISlotCounterPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UpdateSlotUI()
    {
        ItemCraftBlueprint itemBlueprint = blueprint as ItemCraftBlueprint;

        if (itemBlueprint.CanCraft(GameManager.Instance.GetInventorySystem()))
        {
            inventoryUISlotGO.GetComponent<Image>().color = baseColor;
        }
        else
        {
            inventoryUISlotGO.GetComponent<Image>().color = Color.red;
        }
    }

    protected override void InitUI() {

        ItemCraftBlueprint itemBlueprint = blueprint as ItemCraftBlueprint;

        // Init text
        nameText.text = itemBlueprint.outputItem.itemName + " x" + itemBlueprint.outputQuantity;
        requirementsText.text = "";
        foreach (ItemRequirement req in blueprint.requirements)
        {
            requirementsText.text += "x" + req.quantity + " " + req.item.name + "\n";
        }

        // Init icon
        GameObject icon = UIUtils.CreateItemIcon(new ItemInstance(itemBlueprint.outputItem, itemBlueprint.outputQuantity), inventoryUISlotCounterPrefab, inventoryUISlotGO);
        icon.transform.SetParent(inventoryUISlotGO.transform, false);
        inventoryUISlotGO.GetComponent<InventoryUISlot>().SetDisplayedItem(icon, null, draggable: false);

        UpdateSlotUI();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        ItemCraftBlueprint itemBlueprint = blueprint as ItemCraftBlueprint;

        Debug.Log($"Inventory Slot Clicked: {itemBlueprint.outputItem.name}");
        bool result = itemBlueprint.Craft(inventory);

        if (result)
        {
            Debug.Log("crafted!");
        }
        else
        {
            Debug.Log("Can't craft!");
        }
    }
}
