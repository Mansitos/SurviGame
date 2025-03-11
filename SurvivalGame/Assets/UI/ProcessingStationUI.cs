using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProcessingStationUI : BaseInventoryUI
{
    [SerializeField] public GameObject inputSlot;
    [SerializeField] public GameObject outputSlot;
    [SerializeField] public GameObject fuelSlot;
    [SerializeField] public GameObject attachedStation;
    [SerializeField] public GameObject nameTextGO;
    [SerializeField] public GameObject processingUIIcon;

    private ProcessingStation processingStation;
    private InventoryUISlot input;
    private InventoryUISlot output;
    private InventoryUISlot fuel;
    private TextMeshProUGUI nameText;

    private List<InventoryUISlot> slots = new List<InventoryUISlot>();

    protected override void Start()
    {
        gm = GameManager.Instance;
        nameText = nameTextGO.GetComponent<TextMeshProUGUI>();
        InitSlots();
        SetActive(false);
    }

    public override void InitSlots()
    {
        input = inputSlot.GetComponent<InventoryUISlot>();
        output = outputSlot.GetComponent<InventoryUISlot>();
        fuel = fuelSlot.GetComponent<InventoryUISlot>();

        input.SetSlotType(SlotType.ProcessingStation);
        output.SetSlotType(SlotType.ProcessingStation);
        fuel.SetSlotType(SlotType.ProcessingStation);

        input.SetIndex(0);
        fuel.SetIndex(1);
        output.SetIndex(2);

        slots.Add(input);
        slots.Add(fuel);
        slots.Add(output);
    }

    public ProcessingStation GetLinkedProcessingStation()
    {
        return processingStation;
    }

    public void LinkStation(GameObject newStation)
    {
        attachedStation = newStation;
        processingStation = newStation.GetComponent<ProcessingStation>();

        input.SetLinkedInventorySlot(processingStation.storedInput);
        output.SetLinkedInventorySlot(processingStation.storedOutput);
        fuel.SetLinkedInventorySlot(processingStation.storedFuel);

        processingStation.OnStartProcessing += UpdateUI;

        UIManager.Instance.SetProcessingStationTabActive(true);
    }

    public void RemoveItemFromIndexSlot(int index)
    {
        foreach (InventoryUISlot slot in slots)
        {
            if (slot.GetIndex() == index)
            {
                slot.ClearSlot(destroyChild: true);
                switch (index)
                {
                    case 0:
                        processingStation.storedInput.ClearSlot();
                        break;
                    case 1:
                        processingStation.storedFuel.ClearSlot();
                        break;
                    case 2:
                        processingStation.storedOutput.ClearSlot();
                        break;
                }
            }
        }
        UpdateUI();
    }

    public bool VerifyCanAddToSlot(ItemData itemData,  int index)
    {
        switch (index)
        {
            case 0:
                return this.GetLinkedProcessingStation().IsValidInput(itemData);
            case 1:
                return this.GetLinkedProcessingStation().IsValidFuel(itemData);
            case 2:
                Debug.Log("Cannot add items to the output slot!");
                return false;
        }

        return false;
    }

    public void AddToSlot(ItemInstance item, int index)
    {
        switch (index)
        {
            case 0:
                this.GetLinkedProcessingStation().AddProcessingInputRequirement(item);
                break;
            case 1:
                this.GetLinkedProcessingStation().AddFuel(item);
                break;
            case 2:
                Debug.Log("This should never be called since cannot add items to the output slot!");
                break;
        }
        UpdateUI();
    }

    public void UnLinkStation()
    {
        processingStation.OnStartProcessing -= UpdateUI;
        attachedStation = null;
        processingStation = null;
        this.gameObject.SetActive(false);
    }

    // --- Update/Redraw UI Methods ---

    public override void UpdateUI()
    {
        UpdateSlots();
        UpdateProcessingIcon();
        nameText.text = GetLinkedProcessingStation().worldObjectData.objectName;
    }

    protected override void UpdateSlots()
    {
        input.ClearSlot(destroyChild: true);
        fuel.ClearSlot(destroyChild: true);
        output.ClearSlot(destroyChild: true);

        InventorySlot storedInput = processingStation.storedInput;
        InventorySlot storedFuel = processingStation.storedFuel;
        InventorySlot storedOutput = processingStation.storedOutput;

        if (processingStation.HasStoredInput())
        {
            GameObject itemIconObjectInput = UIUtils.CreateItemIcon(storedInput.itemInstance, inventoryUISlotCounterPrefab, grid);
            input.SetDisplayedItem(itemIconObjectInput, storedInput, draggable: true);

            // TODO refactor
            //input.SetDisplayedItemIcon(storedInput, inventoryUISlotCounterPrefab, ItemsAreDraggable);
        }
        else
        {
            input.ClearSlot(destroyChild: true);
        }

        if (processingStation.HasStoredFuel())
        {
            GameObject itemIconObjectFuel = UIUtils.CreateItemIcon(storedFuel.itemInstance, inventoryUISlotCounterPrefab, grid);
            fuel.SetDisplayedItem(itemIconObjectFuel, storedFuel, draggable: true);
            
            // TODO refactor
            //fuel.SetDisplayedItemIcon(storedFuel, inventoryUISlotCounterPrefab, ItemsAreDraggable);
        }
        else
        {
            fuel.ClearSlot(destroyChild: true);
        }

        if (processingStation.HasStoredOutput())
        {
            GameObject itemIconObjectOutput = UIUtils.CreateItemIcon(storedOutput.itemInstance, inventoryUISlotCounterPrefab, grid);
            output.SetDisplayedItem(itemIconObjectOutput, storedOutput, draggable: true);
            
            // TODO refactor
            //output.SetDisplayedItemIcon(storedOutput, inventoryUISlotCounterPrefab, ItemsAreDraggable);

        }
        else
        {
            output.ClearSlot(destroyChild: true);
        }
    }

    private void UpdateProcessingIcon()
    {
        processingUIIcon.SetActive(processingStation.isProcessing);
    }

}
