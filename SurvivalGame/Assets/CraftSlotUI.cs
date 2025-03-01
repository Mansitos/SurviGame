using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static CraftBlueprint;

public class CraftSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ItemCraftBlueprint blueprint;
    [SerializeField] GameObject inventoryUISlotGO;
    [SerializeField] GameObject nameTextGO;
    [SerializeField] GameObject requirementsTextGO;
    [SerializeField] GameObject inventoryUISlotCounterPrefab;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI requirementsText;
    private InventorySystem inventory;
    private Color baseColor;

    void Awake()
    {
        inventory = GameManager.Instance.GetInventorySystem();
        nameText = nameTextGO.GetComponent<TextMeshProUGUI>();
        requirementsText = requirementsTextGO.GetComponent<TextMeshProUGUI>();
        baseColor = inventoryUISlotGO.GetComponent<Image>().color;
    }

    public void UpdateSlotUI()
    {
        if (blueprint.CanCraft(GameManager.Instance.GetInventorySystem()))
        {
            inventoryUISlotGO.GetComponent<Image>().color = baseColor;
        }
        else
        {
            inventoryUISlotGO.GetComponent<Image>().color = Color.red;
        }
    }

    private void InitUI() {

        // Init text
        nameText.text = blueprint.outputItem.itemName + " x" + blueprint.outputQuantity;
        requirementsText.text = "";
        foreach (ItemRequirement req in blueprint.requirements)
        {
            requirementsText.text += "x" + req.quantity + " " + req.item.name + "\n";
        }

        // Init icon
        GameObject icon = UIUtils.CreateItemIcon(new ItemInstance(blueprint.outputItem, blueprint.outputQuantity), inventoryUISlotCounterPrefab, inventoryUISlotGO);
        icon.transform.SetParent(inventoryUISlotGO.transform, false);
        inventoryUISlotGO.GetComponent<InventoryUISlot>().SetDisplayedItem(icon, null, draggable: false);

        UpdateSlotUI();
    }

    public void SetBluePrint(ItemCraftBlueprint blueprint)
    {
        this.blueprint = blueprint;
        InitUI();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Inventory Slot Clicked: {blueprint.outputItem.name}");
        bool result = blueprint.Craft(inventory);

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
