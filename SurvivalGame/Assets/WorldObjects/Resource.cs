using UnityEngine;

[DisallowMultipleComponent]
public class Resource : WorldObject<ResourceObjectData>
{
    protected override void Start()
    {
        base.Start();
        OccupyTile();
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

    public void Collect()
    {
        SpawnItems();
        Vector3Int gridPos = gridManager.WorldToGrid(this.transform.position);
        gridManager.RemoveObjectFromTiles(gridPos, true);
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
    }

}
