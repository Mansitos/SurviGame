using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool HasStoredFuel()
    {
        Debug.Log(storedFuel);
        return (storedFuel != null);
    }

    private bool HasStoredInput()
    {
        return (storedInput != null);
    }

    private bool HasStoredOutput()
    {
        Debug.Log(storedOutput);
        return (storedOutput != null);
    }

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
    public bool AddProcessingRequirement(ItemInstance item)
    {
        foreach (var craft in possibleCrafts)
        {
            if (craft.itemInput == item.ItemData)
            {
                if (storedInput == null)
                {
                    storedInput = item;
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
        }
        Debug.Log("Invalid input. no crafts needs it");
        return false;
    }

    private void RemoveProcessingRequirement(int inputAmountToConsume, int fuelAmountToConsume)
    {
        Debug.Log("Removing fuel:" + fuelAmountToConsume);
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
    }
}