using TMPro;
using UnityEngine;

public class InventoryUI : BaseInventoryUI
{
    public GameObject weightUI;

    private InputHandler inputHandler;
    protected override bool ItemsAreDraggable => true;

    protected override void Start()
    {
        base.Start();
        inputHandler = gm.GetPlayerGO().GetComponent<InputHandler>();
    }

    protected override void InitSlots()
    {
        numSlots = inventory.maxSlots;
        PopulateSlots(numSlots, SlotType.Inventory);
    }

    private void Update()
    {
        CheckInventoryModeInput();
    }

    private void CheckInventoryModeInput()
    {
        if (inputHandler.WasInventoryModePressedThisFrame())
        {
            gm.SetInventoryMode(!gm.IsInInventoryMode());
            UpdateActiveStatus();

            if (gm.IsInInventoryMode())
            {
                UpdateSlots();
            }
        }
    }

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
