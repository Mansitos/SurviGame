using UnityEngine;

[DisallowMultipleComponent]
public class Resource : WorldObject<ResourceObjectData>
{
    [SerializeField] private bool doesProduceItem = false;
    [SerializeField] private ItemData productionItem;
    [SerializeField] private int maxProductionQuantity = 2;
    [SerializeField] private float endOfDayProductionChance = 0.33f;
    [SerializeField] private float endOfDayRemoveChanceIfMaxedOut = 0.05f;
    [SerializeField] private ItemInstance storedProduction;
    [SerializeField] private GameObject producedItemGO;
    private float initProductionChance = 0.33f;

    protected override void Start()
    {
        base.Start();
        OccupyTile();
        if (doesProduceItem)
        {
            storedProduction = new ItemInstance(productionItem, RollChance(initProductionChance) ? 1 : 0);
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
                // Generate a random position within the spawn radius
                Vector3 randomOffset = Random.insideUnitCircle * ResourceObjectData.spawnRadius;
                Vector3 spawnPosition = new Vector3(transform.position.x + randomOffset.x, 0.25f, transform.position.z + randomOffset.y);

                // Create an ItemInstance to spawn
                ItemInstance spawnedItemInstance = new ItemInstance(spawnableItem.itemData, 1); // Hardcoded quantity 1 is an strong assumption in inventory system, keep like this.

                // Spawn the item in the world using DroppedItem
                DroppedItem.Spawn(spawnedItemInstance, spawnPosition);
            }
        }

        // Spawn the produced item
        if (doesProduceItem)
        {
            if (storedProduction.Quantity > 0)
            {
                for (int i = 0; i < storedProduction.Quantity; i++)
                {
                    Vector3 randomOffset = Random.insideUnitCircle * ResourceObjectData.spawnRadius;
                    Vector3 spawnPosition = new Vector3(transform.position.x + randomOffset.x, 0.25f, transform.position.z + randomOffset.y);
                    ItemInstance spawnedItemInstance = new ItemInstance(storedProduction.ItemData, 1);

                    DroppedItem.Spawn(spawnedItemInstance, spawnPosition);
                }
            }
        }
    }

    private bool RollChance(float chance)
    {
        return Random.value < chance;
    }
    
    // Called at end of day (TODO: link via event?)
    private void ExecuteProductionStep(int dayIndex)
    {
        if (storedProduction.Quantity < maxProductionQuantity)
        {
            if (RollChance(endOfDayProductionChance))
            {
                storedProduction.AddQuantity(1);
            }
        }
        else
        {
            if (RollChance(endOfDayRemoveChanceIfMaxedOut))
            {
                storedProduction.RemoveQuantity(1);
            }
        }

        UpdateProductionObjectVisibility();
    }

    private void UpdateProductionObjectVisibility()
    {
        if (doesProduceItem)
        {
            bool flag = storedProduction.Quantity > 0;
            producedItemGO.SetActive(flag);
        }
    }
}
