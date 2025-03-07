using System.Collections.Generic;
using UnityEngine;

public enum ResourceObjectType
{
    Rock,
    Tree,
    PickUp
}

[System.Serializable]
public class SpawnableItem
{
    public ItemData itemData;
    public int minAmount = 1;
    public int maxAmount = 3;
}

[CreateAssetMenu(fileName = "ResourceObject", menuName = "Game/WorldObjects/ResourceObject")]
public class ResourceObjectData : WorldObjectData
{
    [SerializeField] public ResourceObjectType resourceObjectType;
    [SerializeField] public float collectionTime = 5.0f;
    static public float spawnRadius = 2.0f;
    static public float collectionCostPerSecond = 8.0f;
    [SerializeField] public List<SpawnableItem> spawnableItems;

    public float GetCollectionCost()
    {
        return collectionCostPerSecond * collectionTime;
    }
}
