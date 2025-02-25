using TMPro;
using UnityEngine;

public class InventoryUI : BaseInventoryUI
{
    public GameObject weightUI;

    protected override bool ItemsAreDraggable => true;

    protected override void Start()
    {
        base.Start();
        SetActive(false);
    }

    protected override void InitSlots()
    {
        numSlots = inventory.maxSlots;
        PopulateSlots(numSlots, SlotType.Inventory);
    }

    // --- Update/Redraw UI Methods ---

    public override void UpdateUI()
    {
        UpdateSlots();
        UpdateWeightText();
    }

    private void UpdateWeightText()
    {
        weightUI.GetComponent<TextMeshProUGUI>().text = "Weight: " + inventory.GetCurrentWeight() + "/" + inventory.maxWeight;
    }

}
