using UnityEngine;

public class ProcessingStationUI : BaseInventoryUI
{
    [SerializeField] public GameObject inputSlot;
    [SerializeField] public GameObject outputSlot;
    [SerializeField] public GameObject fuelSlot;
    [SerializeField] public GameObject attachedStation;

    private ProcessingStation processingStation;
    private InventoryUISlot input;
    private InventoryUISlot output;
    private InventoryUISlot fuel;

    protected new GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
        input = inputSlot.GetComponent<InventoryUISlot>();
        output = outputSlot.GetComponent<InventoryUISlot>();
        fuel = fuelSlot.GetComponent<InventoryUISlot>();
        input.SetSlotType(SlotType.ProcessingStation);
        output.SetSlotType(SlotType.ProcessingStation);
        fuel.SetSlotType (SlotType.ProcessingStation);
        LinkStation(attachedStation);
    }

    protected override void InitSlots()
    {
        return;
    }

    void Update()
    {
        
    }

    public void LinkStation(GameObject newStation)
    {
        attachedStation = newStation;
        processingStation = newStation.GetComponent<ProcessingStation>();

        ItemInstance storedFuel = processingStation.storedFuel;
        ItemInstance storedInput = processingStation.storedInput;
        ItemInstance storedOutput = processingStation.storedOutput;

        GameObject itemIconObjectFuel = CreateItemIcon(storedFuel);
        itemIconObjectFuel.transform.SetParent(fuel.transform, false);
        fuel.SetDisplayedItem(itemIconObjectFuel, storedFuel, draggable: true);

        GameObject itemIconObjectInput = CreateItemIcon(storedInput);
        itemIconObjectInput.transform.SetParent(input.transform, false);
        input.SetDisplayedItem(itemIconObjectInput, storedInput, draggable: true);

        GameObject itemIconObjectOutput = CreateItemIcon(storedOutput);
        itemIconObjectOutput.transform.SetParent(output.transform, false);
        output.SetDisplayedItem(itemIconObjectOutput, storedOutput, draggable: true);

    }

    public void UnLinkStation()
    {
        Debug.Log("Implement");
    }
}
