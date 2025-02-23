using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewItemCraftBlueprint", menuName = "Game/ItemCraftBlueprint")]
public class ItemCraftBlueprint : CraftBlueprint
{
    public ItemData outputItem;
    public int outputQuantity;

    public CraftStationType requiredCraftStationType = CraftStationType.None;
    private float craftStationRequiredDistance = 3.5f;

    public bool CanCraft(InventorySystem inventory)
    {
        if (!IsCraftStationAvailable())
            return false;

        if (!HasRequiredItems(inventory))
            return false;

        if (WillCauseOverweight(inventory))
            return false;

        return HasSpaceForOutput(inventory);
    }

    private bool IsCraftStationAvailable()
    {
        bool craftStationAvailable = CheckRequiredCraftingStationAvailability();
        if (!craftStationAvailable)
        {
            Debug.Log("[ItemCraftBlueprint] Can't craft item because required craftStation is not found in range.");
        }
        return craftStationAvailable;
    }

    private bool HasRequiredItems(InventorySystem inventory)
    {
        foreach (ItemRequirement requirement in requirements)
        {
            if (!inventory.IsItemAvailable(new ItemInstance(requirement.item, requirement.quantity)))
            {
                Debug.Log($"[ItemCraftBlueprint] Can't craft item because {requirement.item.itemName} with quantity {requirement.quantity} is not available");
                return false;
            }
        }
        return true;
    }

    private bool HasSpaceForOutput(InventorySystem inventory)
    {
        if (inventory.GetFreeSlots() > 0)
            return true; // Easy case, a slot is empty

        if (inventory.IsThereASlotWithItem(outputItem))
            return true; // Medium case, slot with already the output item

        if (requirements.Any(req => inventory.WillRemovalFreeASlot(new ItemInstance(req.item, req.quantity))))
            return true; // Hard case, check if removal of required items frees a slot

        Debug.Log("[ItemCraftBlueprint] Can't craft item because no free slot available after using the required items");
        return false;
    }

    private bool WillCauseOverweight(InventorySystem inventory)
    {
        float addedWeight = outputItem.weight * outputQuantity;
        float removedWeight = requirements.Sum(req => req.item.weight * req.quantity);
        float deltaWeight = addedWeight - removedWeight;

        if (deltaWeight > 0 && inventory.WillBeOverWeight(deltaWeight))
        {
            Debug.Log("[ItemCraftBlueprint] Can't craft item since it would cause overweight");
            return true;
        }
        return false;
    }

    private bool CheckRequiredCraftingStationAvailability()
    {
        if (requiredCraftStationType != CraftStationType.None)
        {
            GameManager gm = GameManager.Instance;
            Vector3 playerPos = gm.GetPlayerGO().transform.position;
            GridManager gridManager = gm.GetTerrainGridManager();

            List<GameObject> craftStations = gridManager.GetAllObjectsOfTypeOnTilesByRadius(playerPos, craftStationRequiredDistance, typeof(CraftStationBuillding));
            foreach (GameObject station in craftStations)
            {
                if (station.GetComponent<CraftStationBuillding>().IsCraftStationOfType(requiredCraftStationType))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool Craft(InventorySystem inventory)
    {
        if (CanCraft(inventory))
        {
            foreach (ItemRequirement requirement in requirements)
            {
                inventory.TryRemoveItem(new ItemInstance(requirement.item, requirement.quantity));
            }

            bool esit = inventory.TryAddItem(new ItemInstance(outputItem, outputQuantity));
            if (esit == false)
            {
                Debug.LogError("[ItemCraftBlueprint] This should never happen since add condition was checked earlier!");
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
