using UnityEngine;

[CreateAssetMenu(fileName = "NewItemCraftBlueprint", menuName = "Game/ItemCraftBlueprint")]
public class ItemCraftBlueprint : CraftBlueprint
{
    public ItemData outputItem;
    public int outputQuantity;

    public bool CanCraft(InventorySystem inventory)
    {
        // First simple check is to check whenever the required items are available
        foreach (ItemRequirement requirement in requirements)
        {
            if (!inventory.isItemAvailable(new ItemInstance(requirement.item, requirement.quantity)))
            {
                Debug.Log($"[ItemCraftBlueprint] Can't craft item because {requirement.item.itemName} with quantity {requirement.quantity} is not available");
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

        Debug.Log("[ItemCraftBlueprint] Can't craft item because no free slot available after using the required items");
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
