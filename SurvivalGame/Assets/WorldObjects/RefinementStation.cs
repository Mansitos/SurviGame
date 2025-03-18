using UnityEngine;
using System;

public class RefinementStation : Building<RefinementStationData>
{
    // Internal status
    public InventorySlot storedInput;
    public InventorySlot storedOutput;
    public RefinementBlueprint RefinementBlueprintUnderProcess = null;
    public bool isRefining;
    public int remainingRefiningDays;

    // Events
    public event Action OnStartRefining;

    protected override void Start()
    {
        base.Start();
        storedInput = new InventorySlot(null);
        storedOutput = new InventorySlot(null);
    }

    void Update()
    {
        if (!IsRefining())
        {
            CheckStartRefinementConditions(); //TODO: in future, for optimization, only when thigs added
        }
    }

    public bool IsRefining()
    {
        return isRefining;
    }

    public bool HasStoredInput()
    {
        return !storedInput.IsEmpty();
    }

    public bool HasStoredOutput()
    {
        return !storedOutput.IsEmpty();
    }

    public bool IsValidInput(ItemData targetData)
    {
        foreach (var craft in worldObjectData.possibleRefinements)
        {
            if (craft.itemInput == targetData)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckStartRefinementConditions()
    {
        // Check if a new refining procedure should be started
        if (!IsRefining() && HasStoredInput())
        {
            // Find if there's a craft that can be processed
            foreach (var craft in worldObjectData.possibleRefinements)
            {
                // If the stored input data matches the one required by a craft proceed.
                if (craft.itemInput == storedInput.itemInstance.ItemData)
                {
                    // If the output slot is free or filled with the matching item Output, proceed
                    if (!HasStoredOutput() || craft.itemOutput == storedOutput.itemInstance.ItemData)
                    {
                        isRefining = true;
                        remainingRefiningDays = craft.requiredDaysToRefine;
                        return;
                    }
                    else
                    {
                        Debug.Log("All craft condition are met except for present output being different from expected one");
                    }
                }
            }

            Debug.Log("No matching refinement craft");
        }

        Debug.Log("Basic process condition un-met");
    }

    //TODO: SAME METHOD AS PROCESSING STATION, REFACTOR
    public bool AddProcessingInputRequirement(ItemInstance item)
    {
        if (IsValidInput(item.ItemData))
        {
            if (!HasStoredInput())
            {
                storedInput = new InventorySlot(null);
                storedInput.AddItem(item);
                Debug.Log("Valid input, input was none");
                return true;
            }
            else
            {
                if (storedInput.itemInstance.ItemData == item.ItemData)
                {
                    storedInput.itemInstance.AddQuantity(item.Quantity);
                    Debug.Log("Valid input, input was present, adding");
                    return true;
                }
                else
                {
                    Debug.Log("Valid input, but input was present and different");
                    return false;
                }

            }
        }
        else
        {
            Debug.Log("Invalid input. no crafts needs it");
            return false;
        }
    }

    private void RemoveRefinementRequirement(int inputAmountToConsume)
    {
        storedInput.RemoveItem(inputAmountToConsume);
    }

    override public bool InteractWithWorldObject()
    {
        UIManager.Instance.GetRefinementStationUI().LinkStation(this.gameObject);
        return true;
    }

    private void CheckEndRefinementConditions()
    {
        if (remainingRefiningDays == 0)
        {
            // GIVE OUTPUT AND RESET STATUS
        }
    }

    public void ProcessingStep()
    {
        remainingRefiningDays -= 1;
        Debug.Log("Processing step done!");
    }


}
