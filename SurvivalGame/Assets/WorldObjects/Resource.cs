using UnityEngine;

[DisallowMultipleComponent]
public class Resource : WorldObject<ResourceObjectData>, IInteractableWO
{
    private ResourceObjectData data;
    [SerializeField] public ItemInstance storedProduction;
    [SerializeField] public GameObject producedItemGO;

    protected override void Start()
    {
        base.Start();
        OccupyTile();
        data = worldObjectData;
        if (data.doesProduceItem)
        {
            storedProduction = new ItemInstance(data.productionItem, RollChance(data.initProductionChance) ? 1 : 0);
            UpdateProductionObjectVisibility();
            GameManager.Instance.GetGameTimeManager().OnDayEnded += ExecuteProductionStep;
        }
    }

    public void OccupyTile()
    {
        Vector3Int gridPos = gridManager.WorldToGrid(this.transform.position);
        if (gridManager.CanPlaceBuilding(gridPos, 1, 1, 0, checkAgainstPlayer: false))
        {
            gridManager.PlaceObjectOnTiles(gridPos, 1, 1, this.gameObject, 0);
        }
        else
        {
            Debug.Log("[ResourceObject] Can't place resource object since tile occupied! Destroying.");
            Destroy(this.gameObject);
        }
    }

    public bool IsResourceObjectOfType(ResourceObjectType type)
    {
        return worldObjectData.resourceObjectType == type;
    }

    public float GetCollectionDuration()
    {
        return worldObjectData.collectionTime;
    }

    public bool PlayerHasEnoughEnergyToCollect()
    {
        return gm.GetPlayerStatus().GetEnergy() > worldObjectData.GetCollectionCost();
    }

    public void Collect()
    {
        SpawnItems();
        Vector3Int gridPos = gridManager.WorldToGrid(this.transform.position);
        gridManager.RemoveObjectFromTiles(gridPos, true);
        gm.GetPlayerStatus().ReduceEnergy(worldObjectData.GetCollectionCost());
        Destroy(this.gameObject);
    }

    private void SpawnItems()
    {
        foreach (var spawnableItem in worldObjectData.spawnableItems)
        {
            if (spawnableItem.itemData == null) continue; // Ensure item data exists

            // Determine number of items to spawn (between min and max)
            int amount = Random.Range(spawnableItem.minAmount, spawnableItem.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                // Call the extracted method for spawning the item
                SpawnItem(spawnableItem.itemData);
            }
        }

        SpawnProducedItems();
    }

    private void SpawnProducedItems()
    {
        // Spawn the produced item
        if (data.doesProduceItem && storedProduction.Quantity > 0)
        {
            for (int i = 0; i < storedProduction.Quantity; i++)
            {
                // Call the same method using the produced item's data
                SpawnItem(storedProduction.ItemData);
            }
        }

        storedProduction.RemoveQuantity(storedProduction.Quantity);
        UpdateProductionObjectVisibility();
    }

    private void SpawnItem(ItemData itemData)
    {
        // Generate a random position within the spawn radius
        Vector3 randomOffset = Random.insideUnitCircle * ResourceObjectData.spawnRadius;
        Vector3 spawnPosition = new Vector3(transform.position.x + randomOffset.x, 0.25f, transform.position.z + randomOffset.y);

        // Create an ItemInstance with a hardcoded quantity of 1
        ItemInstance spawnedItemInstance = new ItemInstance(itemData, 1);

        // Spawn the item in the world using DroppedItem
        DroppedItem.Spawn(spawnedItemInstance, spawnPosition);
    }


    private bool RollChance(float chance)
    {
        return Random.value < chance;
    }
    
    // Called at end of day (TODO: link via event?)
    private void ExecuteProductionStep(int dayIndex)
    {
        if (storedProduction.Quantity < data.maxProductionQuantity)
        {
            if (RollChance(data.endOfDayProductionChance))
            {
                storedProduction.AddQuantity(1);
            }
        }
        else
        {
            if (RollChance(data.endOfDayRemoveChanceIfMaxedOut))
            {
                storedProduction.RemoveQuantity(1);
            }
        }

        UpdateProductionObjectVisibility();
    }

    private void UpdateProductionObjectVisibility()
    {
        if (data.doesProduceItem)
        {
            bool flag = storedProduction.Quantity > 0;
            producedItemGO.SetActive(flag);
        }
    }

    public virtual bool InteractWithWorldObject()
    {
        if (data.doesProduceItem)
        {
            if (storedProduction != null && storedProduction.Quantity > 0)
            {
                SpawnProducedItems();
            }
            else
            {
                Debug.Log("nothing to harvest.");
            }
        }
        else
        {
            Debug.Log("This is not a producer resource.");
        }
        return true;
    }
}
