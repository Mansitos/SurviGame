using UnityEngine;
using System.Collections.Generic;

public enum ResourceObjectType
{
    Rock,
    Tree
}

[System.Serializable]
public class SpawnableItem
{
    public GameObject itemPrefab;
    public int minAmount = 1;
    public int maxAmount = 3;
}

[DisallowMultipleComponent]
public class ResourceObject : WorldObject
{
    [SerializeField] public ResourceObjectType resourceObjectType;
    [SerializeField] private float collectionTime = 5.0f;
    [SerializeField] private float spawnRadius = 2.0f;
    [SerializeField] private List<SpawnableItem> spawnableItems;

    private GameManager gm;
    private GridManager gridManager;

    private void Start()
    {
        gm = GameManager.Instance;
        gridManager = gm.getTerrainGridManager();
        OccupyTile();
    }

    public void OccupyTile()
    {
        Vector3Int gridPos = gridManager.WorldToGrid(this.transform.position);
        if (gridManager.CanPlaceBuilding(gridPos, 1, 1, 0))
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
        return resourceObjectType == type;
    }

    public float GetCollectionDuration()
    {
        return collectionTime;
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
        if (spawnableItems == null || spawnableItems.Count == 0)
        {
            Debug.LogWarning("No items assigned to spawn!");
            return;
        }

        foreach (var spawnableItem in spawnableItems)
        {
            if (spawnableItem.itemPrefab == null) continue;

            // Determine number of items to spawn (between min and max)
            int amount = Random.Range(spawnableItem.minAmount, spawnableItem.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                // Generate a random position within the spawn radius
                Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPosition = new Vector3(transform.position.x + randomOffset.x, 0.25f, transform.position.z + randomOffset.y);

                // Instantiate the item
                Instantiate(spawnableItem.itemPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
