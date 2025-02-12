using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftBlueprint", menuName = "Game/CraftBlueprint")]
public class CraftBlueprint : ScriptableObject
{
    [System.Serializable]
    public struct ItemRequirement
    {
        public ItemData item;
        public int quantity;
    }

    public List<ItemRequirement> requirements;
    public ItemData outputItem;
    public int outputQuantity;

    public bool CanCraft(InventorySystem inventory)
    {
        // First simple check is to check whenever the required items are available
        foreach (ItemRequirement requirement in requirements)
        {
            if (!inventory.isItemAvailable(new ItemInstance(requirement.item, requirement.quantity)))
            {
                Debug.Log($"[CraftBlueprint] Can't craft because {requirement.item.itemName} with quantity {requirement.quantity} is not available");
                return false;
            }
        }

        // Now i need to check if there is space for the output
        if (inventory.GetFreeSlots() > 0)
        {
            return true; // Easy case, a slot is empty
        }

        // Medium case, maybe there is a slot with already the output item
        if (inventory.IsThereASlotWithItem(outputItem))
        {
            return true;
        }

        // Hard case, i need to check if a slot would be freed after removal of required input
        foreach (ItemRequirement requirement in requirements)
        {
            if (inventory.willRemovalFreeASlot(new ItemInstance(requirement.item, requirement.quantity)))
            {
                return true;
            }
        }

        Debug.Log("[CraftBlueprint] Can't craft because no free slot available after using the required items");
        return false;
        
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
                Debug.LogError("[CraftBlueprint] This should never happen since add condition was checked earlier!");
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
