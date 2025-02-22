public class InventoryUI : BaseInventoryUI
{
    private PlayerMovementInputHandler inputHandler;
    protected override bool ItemsAreDraggable => true;

    protected override void Start()
    {
        base.Start();
        inputHandler = gm.GetPlayerGO().GetComponent<PlayerMovementInputHandler>();
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
}
