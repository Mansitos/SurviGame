using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProcessingStation : Building<ProcessingStationData>
{
    // Internal status
    public InventorySlot storedFuel;
    public InventorySlot storedInput;
    public InventorySlot storedOutput;
    public ProcessingBlueprint ItemCraftBlueprintUnderProcess = null;
    public bool isProcessing;

    // Events
    public event Action OnStartProcessing;

    protected override void Start()
    {
        base.Start();
        storedFuel = new InventorySlot(null);
        storedInput = new InventorySlot(null);
        storedOutput = new InventorySlot(null);
    }

    void Update()
    {
        CheckStartProcessingConditions(); //TODO: in future, for optimization, only when thigs added
    }

    public bool IsProcessing()
    {
        return isProcessing;
    }

    public bool HasStoredFuel()
    {
        return !storedFuel.IsEmpty();
    }

    public bool HasStoredInput()
    {
        return !storedInput.IsEmpty();
    }

    public bool HasStoredOutput()
    {
        return !storedOutput.IsEmpty();
    }

    // TODO: refactor to use IsValidFuel()
    private (bool, int) HasAvailableValidFuel()
    {
        if (!HasStoredFuel())
        {
            return (false, 0);
        }
        else
        {
            foreach (var requiredFuel in worldObjectData.validProcessingFuelRequirements) {
                if (requiredFuel.item == storedFuel.itemInstance.ItemData)
                {
                    if (requiredFuel.quantity <= storedFuel.itemInstance.Quantity)
                    {
                        return (true, requiredFuel.quantity);
                    }
                    else
                    {
                        Debug.Log("Fuel was matching but not enough qnty.");
                    }
                }
            }
            return (false, 0);
        }
    }

    public bool IsValidFuel(ItemData targetData)
    {
        foreach (var requiredFuel in worldObjectData.validProcessingFuelRequirements)
        {
            if (requiredFuel.item == targetData)
            {
                return true;
            }
        }
        return false;
    }

    // TODO: refactor and use where needed to clean code
    public bool IsValidInput(ItemData targetData)
    {
        foreach (var craft in worldObjectData.possibleBlueprints)
        {
            if (craft.itemInput == targetData)
            {
                return true;
            }
        }
        return false;
    }

    // TODO: implement and refactor who does this check
    public bool IsValidOutput()
    {
        return false;
    }

    private void CheckStartProcessingConditions()
    {
        // Check if a new processing step coroutine should be started
        if (!IsProcessing() && HasStoredFuel() && HasStoredInput())
        {
            // Find if there's a craft that can be processed
            foreach (var craft in worldObjectData.possibleBlueprints)
            {
                // If the stored input data matches the one required by a craft and the quantity is also ok, proceed.
                if (craft.itemInput == storedInput.itemInstance.ItemData && storedInput.itemInstance.Quantity >= craft.itemInputQuantity)
                {
                    // If the output slot is free or filled with the matching item Output, proceed
                    if (!HasStoredOutput() || craft.itemOutput == storedOutput.itemInstance.ItemData)
                    {
                        var(hasValidFuel, fuelAmountToConsume) = HasAvailableValidFuel();
                        if (hasValidFuel)
                        {
                            Debug.Log("Starting processing conditions met! Starting!");
                            ItemCraftBlueprintUnderProcess = craft;
                            StartCoroutine(ProcessCoroutine(fuelAmountToConsume));
                            break; // Break as soon as we find a valid craft to process
                        }

                    }
                    else
                    {
                        Debug.Log("All craft condition are met except for present output being different from expected one");
                    }
                }
            }

            Debug.Log("No matching craft");
        }

        Debug.Log("Basic process condition un-met");
    }

    //TODO: da rivedere, servirà come metodo da chiamare quando aggiungo quantità da UI? forse useless now?
    //TODO: SAME METHOD AS PRODUCING STATION, REFACTOR
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

    // Code replication between this and AddInput: refactor needed!
    public bool AddFuel(ItemInstance item)
    {
        if (IsValidFuel(item.ItemData))
        {
            if (!HasStoredFuel())
            {
                storedFuel = new InventorySlot(null);
                storedFuel.AddItem(item);
                Debug.Log("Valid fuel, input was none");
                return true;
            }
            else
            {
                if (storedFuel.itemInstance.ItemData == item.ItemData)
                {
                    storedFuel.itemInstance.AddQuantity(item.Quantity);
                    Debug.Log("Valid fuel, input was present, adding");
                    return true;
                }
                else
                {
                    Debug.Log("Valid fuel, but input was present and different");
                    return false;
                }

            }
        }
        else
        {
            Debug.Log("Invalid fuel. no crafts needs it");
            return false;
        }
    }

    private void RemoveProcessingRequirement(int inputAmountToConsume, int fuelAmountToConsume)
    {
        storedInput.RemoveItem(inputAmountToConsume);
        storedFuel.RemoveItem(fuelAmountToConsume);

        OnStartProcessing?.Invoke();
    }

    override public bool InteractWithWorldObject()
    {
        UIManager.Instance.GetProcessingStationUI().LinkStation(this.gameObject);
        return true;
    }

    public IEnumerator ProcessCoroutine(int fuelAmountToConsume)
    {
        isProcessing = true;

        // Consume input and fuel
        int inputAmountToConsume = ItemCraftBlueprintUnderProcess.itemInputQuantity;
        RemoveProcessingRequirement(inputAmountToConsume, fuelAmountToConsume);

        // Process time simulation
        yield return new WaitForSeconds(worldObjectData.processingTime);

        // Generate output
        if (storedOutput.IsEmpty())
        {
            storedOutput = new InventorySlot(null);
            storedOutput.AddItem(new ItemInstance(ItemCraftBlueprintUnderProcess.itemOutput, ItemCraftBlueprintUnderProcess.itemOutputQuantity));
            Debug.Log("Processing completed. Output added from null.");
        }
        else
        {
            storedOutput.itemInstance.AddQuantity(ItemCraftBlueprintUnderProcess.itemOutputQuantity);
            Debug.Log("Processing completed. Output added to already present one.");
        }

        // Ensure flag and status is reset when processing is done
        ItemCraftBlueprintUnderProcess = null;
        isProcessing = false;
        OnStartProcessing?.Invoke();
    }
}