using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    protected new GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
        nameText = nameTextGO.GetComponent<TextMeshProUGUI>();
        InitSlots();
        this.gameObject.SetActive(false);
    }

    protected override void InitSlots()
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

        processingStation.OnStartProcessing += UpdateUI;
        this.gameObject.SetActive(true);
        gm.GetUIManager().GetInventoryUI().SetActive(true);
        UpdateUI();
    }

    public override void UpdateUI()
    {
        UpdateSlots();
        UpdateProcessingIcon();
        nameText.text = GetLinkedProcessingStation().name;
    }

    private void UpdateProcessingIcon()
    {
        processingUIIcon.SetActive(processingStation.isProcessing);
    }

    protected override void UpdateSlots()
    {
        input.ClearSlot(destroyChild: true);
        fuel.ClearSlot(destroyChild: true);
        output.ClearSlot(destroyChild: true);

        ItemInstance storedInput = processingStation.storedInput;
        ItemInstance storedFuel = processingStation.storedFuel;
        ItemInstance storedOutput = processingStation.storedOutput;

        if (processingStation.HasStoredInput())
        {
            GameObject itemIconObjectInput = CreateItemIcon(storedInput);
            itemIconObjectInput.transform.SetParent(input.transform, false);
            input.SetDisplayedItem(itemIconObjectInput, storedInput, draggable: true);
        }
        if (processingStation.HasStoredFuel())
        {
            GameObject itemIconObjectFuel = CreateItemIcon(storedFuel);
            itemIconObjectFuel.transform.SetParent(fuel.transform, false);
            fuel.SetDisplayedItem(itemIconObjectFuel, storedFuel, draggable: true);
        }
        if (processingStation.HasStoredOutput())
        {
            GameObject itemIconObjectOutput = CreateItemIcon(storedOutput);
            itemIconObjectOutput.transform.SetParent(output.transform, false);
            output.SetDisplayedItem(itemIconObjectOutput, storedOutput, draggable: true);
        }
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
                        processingStation.storedInput = null;
                        break;
                    case 1:
                        processingStation.storedFuel = null;
                        break;
                    case 2:
                        processingStation.storedOutput = null;
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
                Debug.Log("add fuel");
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
}
