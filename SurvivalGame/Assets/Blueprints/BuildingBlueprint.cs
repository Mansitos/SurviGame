using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingBlueprint", menuName = "Game/Blueprint/BuildingBlueprint")]
public class BuildingBlueprint : Blueprint
{
    public bool CanBuild(InventorySystem inventory)
    {
        // First simple check is to check whenever the required items are available
        foreach (ItemRequirement requirement in requirements)
        {
            if (!inventory.IsItemAvailable(new ItemInstance(requirement.item, requirement.quantity)))
            {
                Debug.Log($"[BuildingBlueprint] Can't build because {requirement.item.itemName} with quantity {requirement.quantity} is not available");
                return false;
            }
        }
        return true;

    }

    public bool Build(InventorySystem inventory)
    {
        if (CanBuild(inventory))
        {
            foreach (ItemRequirement requirement in requirements)
            {
                inventory.TryRemoveItem(new ItemInstance(requirement.item, requirement.quantity));
            }

            // Logic for actual build

            return true;
        }
        else
        {
            return false;
        }
    }
}
