using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProcessingStation : Building
{
    [System.Serializable]
    public struct ProcessingFuelRequirement
    {
        public ItemData item;
        public int quantity;
    }

    // Variables
    public List<ProcessingItemCraftBlueprint> possibleCrafts = new List<ProcessingItemCraftBlueprint>();
    public List<ProcessingFuelRequirement> validProcessingFuelRequirements;
    public float processingTime;

    // Internal status
    public ItemInstance storedFuel;
    public ItemInstance storedInput;
    public ItemInstance storedOutput;
    public ProcessingItemCraftBlueprint ItemCraftBlueprintUnderProcess = null;
    public bool isProcessing;

    // Events
    public event Action OnStartProcessing;

    void Update()
    {
        ResetStateNulls();
        CheckStartProcessingConditions(); //TODO: in future, for optimization, only when thigs added
    }

    // TODO: i still not understand this unity/c# ... behaviour
    private void ResetStateNulls()
    {
        if (storedFuel != null && storedFuel.Quantity == 0)
        {
            storedFuel = null;
        }
        if (storedInput != null && storedInput.Quantity == 0)
        {
            storedInput = null;
        }
        if (storedOutput != null && storedOutput.Quantity == 0)
        {
            storedOutput = null;
        }
    }

    public bool IsProcessing()
    {
        return isProcessing;
    }

    public bool HasStoredFuel()
    {
        ResetStateNulls();
        return (storedFuel != null);
    }

    public bool HasStoredInput()
    {
        ResetStateNulls();
        return (storedInput != null);
    }

    public bool HasStoredOutput()
    {
        ResetStateNulls();
        return (storedOutput != null);
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
            foreach (var requiredFuel in validProcessingFuelRequirements) {
                if (requiredFuel.item == storedFuel.ItemData)
                {
                    if (requiredFuel.quantity <= storedFuel.Quantity)
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
        foreach (var requiredFuel in validProcessingFuelRequirements)
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
        foreach (var craft in possibleCrafts)
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
            foreach (var craft in possibleCrafts)
            {
                // If the stored input data matches the one required by a craft and the quantity is also ok, proceed.
                if (craft.itemInput == storedInput.ItemData && storedInput.Quantity >= craft.itemInputQuantity)
                {
                    // If the output slot is free or filled with the matching item Output, proceed
                    if (!HasStoredOutput() || craft.itemOutput == storedOutput.ItemData)
                    {
                        var(hasValidFuel, fuelAmountToConsume) = HasAvailableValidFuel();
                        if (hasValidFuel)
                        {
                            Debug.Log("Starting processing conditions met! Starting!");
                            ItemCraftBlueprintUnderProcess = craft;
                            StartCoroutine(ProcessCoroutine(fuelAmountToConsume));
                            OnStartProcessing?.Invoke();
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
    public bool AddProcessingInputRequirement(ItemInstance item)
    {
        if (IsValidInput(item.ItemData))
        {
            if (storedInput == null)
            {
                storedInput = item.Clone();
                Debug.Log("Valid input, input was none");
                return true;
            }
            else
            {
                if (storedInput.ItemData == item.ItemData)
                {
                    storedInput.AddQuantity(item.Quantity);
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
            if (storedFuel == null)
            {
                storedFuel = item.Clone();
                Debug.Log("Valid fuel, input was none");
                return true;
            }
            else
            {
                if (storedFuel.ItemData == item.ItemData)
                {
                    storedFuel.AddQuantity(item.Quantity);
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
        storedInput.RemoveQuantity(inputAmountToConsume);
        storedFuel.RemoveQuantity(fuelAmountToConsume);
    }

    public IEnumerator ProcessCoroutine(int fuelAmountToConsume)
    {
        isProcessing = true;

        // Consume input and fuel
        int inputAmountToConsume = ItemCraftBlueprintUnderProcess.itemInputQuantity;
        RemoveProcessingRequirement(inputAmountToConsume, fuelAmountToConsume);

        // Process time simulation
        yield return new WaitForSeconds(processingTime);

        // Generate output
        if (storedOutput == null)
        {
            storedOutput = new ItemInstance(ItemCraftBlueprintUnderProcess.itemOutput, ItemCraftBlueprintUnderProcess.itemOutputQuantity);
            Debug.Log("Processing completed. Output added from null.");
        }
        else
        {
            storedOutput.AddQuantity(ItemCraftBlueprintUnderProcess.itemOutputQuantity);
            Debug.Log("Processing completed. Output added to already present one.");
        }

        // Ensure flag and status is reset when processing is done
        ItemCraftBlueprintUnderProcess = null;
        isProcessing = false;
        OnStartProcessing?.Invoke();
    }
}