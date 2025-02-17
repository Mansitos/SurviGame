using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingStation : Building
{
    public List<ProcessingItemCraftBlueprint> possibleCrafts = new List<ProcessingItemCraftBlueprint>();
    [SerializeField] public ItemInstance storedFuel;
    [SerializeField] public ItemInstance storedInput;
    [SerializeField] public ItemInstance storedOutput;
    public ProcessingItemCraftBlueprint actualProcess = null;
    public float processingTime;
    public bool isProcessing;

    void Update()
    {
        CheckStartProcessingConditions(); //TODO: in future, for optimization, only when thigs added
    }

    private void CheckStartProcessingConditions()
    {
        // Check if the coroutine should be running
        if (!isProcessing && storedFuel != null && storedFuel.Quantity > 0 && storedInput != null)
        {
            // Find if there's a craft that can be processed
            foreach (var craft in possibleCrafts)
            {
                if (craft.itemInput == storedInput.ItemData && storedInput.Quantity >= craft.itemInputQuantity)
                {
                    if (craft.itemOutput == storedOutput.ItemData)
                    {
                        Debug.Log("Starting processing conditions met! Starting!");
                        actualProcess = craft;
                        StartCoroutine(ProcessCoroutine());
                        isProcessing = true;
                        break; // Break as soon as we find a valid craft to process
                    }
                    else
                    {
                        Debug.Log("All craft condition are met except for present output being different from expected one");
                    }
  
                }
            }
        }
    }

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

    public void RemoveProcessingRequirement()
    {
        storedInput.RemoveQuantity(actualProcess.itemInputQuantity);
        storedFuel.RemoveQuantity(1);
    }

    public IEnumerator ProcessCoroutine()
    {
        isProcessing = true;

        // Consume input and fuel
        RemoveProcessingRequirement();

        // Process time simulation
        yield return new WaitForSeconds(processingTime);

        // Generate output
        if (storedOutput == null)
        {
            storedOutput = new ItemInstance(actualProcess.itemOutput, actualProcess.itemOutputQuantity);
            Debug.Log("Processing completed. Output added from null.");
        }
        else
        {
            storedOutput.AddQuantity(actualProcess.itemOutputQuantity);
            Debug.Log("Processing completed. Output added to already present one.");
        }


        // Ensure flag and status is reset when processing is done
        actualProcess = null;
        isProcessing = false;
    }
}