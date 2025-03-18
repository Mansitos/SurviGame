using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RefinementStationUI : BaseInventoryUI, IStationUI
{
    [SerializeField] public GameObject inputSlot;
    [SerializeField] public GameObject outputSlot;
    [SerializeField] public GameObject attachedStation;
    [SerializeField] public GameObject nameTextGO;
    [SerializeField] public GameObject refinementUIIcon;

    private RefinementStation refinementStation;
    private InventoryUISlot input;
    private InventoryUISlot output;
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

        input.SetSlotType(SlotType.RefinementStation);
        output.SetSlotType(SlotType.RefinementStation);

        input.SetIndex(0);
        output.SetIndex(2);

        slots.Add(input);
        slots.Add(output);
    }

    public RefinementStation GetLinkedProcessingStation()
    {
        return refinementStation;
    }

    public void LinkStation(GameObject newStation)
    {
        attachedStation = newStation;
        refinementStation = newStation.GetComponent<RefinementStation>();

        input.SetLinkedInventorySlot(refinementStation.storedInput);
        output.SetLinkedInventorySlot(refinementStation.storedOutput);

        refinementStation.OnStartRefining += UpdateUI;

        UIManager.Instance.SetRefinementStationTabActive(true);
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
                        refinementStation.storedInput.ClearSlot();
                        break;
                    case 2:
                        refinementStation.storedOutput.ClearSlot();
                        break;
                }
            }
        }
        UpdateUI();
    }

    public bool VerifyCanAddToSlot(ItemData itemData, int index)
    {
        switch (index)
        {
            case 0:
                return this.GetLinkedProcessingStation().IsValidInput(itemData);
            case 2:
                return this.GetLinkedProcessingStation().IsValidInput(itemData);
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
        }
        UpdateUI();
    }

    public void UnLinkStation()
    {
        if (this.GetLinkedProcessingStation() != null)
        {
            refinementStation.OnStartRefining -= UpdateUI;
            input.ClearSlot(destroyChild: true);
            output.ClearSlot(destroyChild: true);
            attachedStation = null;
            refinementStation = null;
            UpdateUI();
        }
    }

    // --- Update/Redraw UI Methods ---

    public override void UpdateUI()
    {
        if (isActive)
        {
            UpdateSlots();
            UpdateProcessingIcon();
            nameText.text = GetLinkedProcessingStation().worldObjectData.objectName;
        }
    }

    protected override void UpdateSlots()
    {
        input.ClearSlot(destroyChild: true);
        output.ClearSlot(destroyChild: true);

        InventorySlot storedInput = refinementStation.storedInput;
        InventorySlot storedOutput = refinementStation.storedOutput;

        if (refinementStation.HasStoredInput())
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

        if (refinementStation.HasStoredOutput())
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
        refinementUIIcon.SetActive(refinementStation.IsRefining());
    }

}
