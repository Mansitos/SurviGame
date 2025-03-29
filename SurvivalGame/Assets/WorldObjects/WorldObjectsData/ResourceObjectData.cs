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
    [SerializeField] public float collectionCostPerSecond = 1.0f;
    [SerializeField] public List<SpawnableItem> spawnableItems;

    [Header("Item Production Variables")]
    [SerializeField] public bool doesProduceItems = false;
    [SerializeField] public ItemData productionItem;
    [SerializeField] public int maxProductionQuantity = 2;
    [SerializeField] public float endOfDayProductionChance = 0.33f;
    [SerializeField] public float endOfDayRemoveChanceIfMaxedOut = 0.05f;
    [SerializeField] public float initProductionChance = 0.33f;
    [SerializeField] public float productionCollectionTime = 2.0f;
    [SerializeField] public float produdctionCollectionCostPerSecond = 1.5f;

    public float GetCollectionCost()
    {
        return collectionCostPerSecond * collectionTime;
    }

    public float GetProductionCollectionCost()
    {
        return produdctionCollectionCostPerSecond * collectionTime;
    }
}
