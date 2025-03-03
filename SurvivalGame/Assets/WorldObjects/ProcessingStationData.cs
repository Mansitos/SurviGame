using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProcessingFuelRequirement
{
    public ItemData item;
    public int quantity;
}

[CreateAssetMenu(fileName = "ProcessingStationData", menuName = "Scriptable Objects/ProcessingStationData")]
public class ProcessingStationData : BuildingData
{
    // Variables
    public List<ProcessingItemCraftBlueprint> possibleCrafts = new List<ProcessingItemCraftBlueprint>();
    [SerializeField] public List<ProcessingFuelRequirement> validProcessingFuelRequirements;
    public float processingTime;
}
