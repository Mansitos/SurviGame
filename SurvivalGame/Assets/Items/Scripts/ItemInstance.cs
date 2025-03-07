using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;

    public ItemInstance(ItemData itemData, int quantity = 1)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }

    public ItemInstance Clone()
    {
        ItemInstance newItem = new ItemInstance(this.itemData, this.quantity);
        return newItem;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public bool RemoveQuantity(int amount)
    {
        if (quantity >= amount)
        {
            quantity -= amount;
            return true;
        }
        return false;
    }

    public bool PerformMainAction(GameManager gameManager)
    {
        if (ItemData is IUsableItem usableItem)
        {
            return usableItem.PerformMainAction(gameManager);
        }
        {
            Debug.LogWarning("[ItemInstance] Cannot perform main action on a non-IUsableItem!");
            return false;
        }
    }

    public bool PerformSecondaryAction(GameManager gameManager)
    {
        Debug.Log("Perform secondary action");
        Vector3Int? selectedTile = gameManager.GetPlayerTileSelection().GetHoveredTilePosition();
        if (selectedTile != null)
        {
            GameObject targetObject = gameManager.GetTerrainGridManager().GetObjectOnTile(selectedTile.Value);
            if (targetObject != null)
            {
                IBuildable targetBuilding = targetObject.GetComponent<IBuildable>();
                if (targetBuilding != null)
                {
                    targetBuilding.InteractWithBuilding();
                }
                else
                {
                    Debug.Log("Object is not a building object");
                }
            }
            else
            {
                Debug.Log("Empty tile for the 2nd action");
            }
        }
        else
        {
            Debug.Log("Cannot perform 2nd action since no tile selected");
        }

        return true;
    }
}
