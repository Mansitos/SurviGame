using TMPro;
using UnityEngine;

public class InventoryUI : BaseInventoryUI
{
    public GameObject weightUI;
    public SlotType slotType;

    protected override bool ItemsAreDraggable => true;

    protected override void Start()
    {
        base.Start();
        SetActive(false);
    }

    public override void InitSlots()
    {
        numSlots = inventory.maxSlots;
        PopulateSlots(numSlots, slotType);
    }

    // --- Update/Redraw UI Methods ---

    public override void UpdateUI()
    {
        if (targetInventoryGO != null)
        {
            UpdateSlots();
            UpdateWeightText();
        }

    }

    private void UpdateWeightText()
    {
        weightUI.GetComponent<TextMeshProUGUI>().text = "Weight: " + inventory.GetCurrentWeight() + "/" + inventory.maxWeight;
    }

}
