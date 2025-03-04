using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProcessingFuelRequirement
{
    public ItemData item;
    public int quantity;
}

[CreateAssetMenu(fileName = "ProcessingStation", menuName = "Game/WorldObjects/ProcessingStation")]
public class ProcessingStationData : BuildingData
{
    // Variables
    [SerializeField] public List<ProcessingBlueprint> possibleBlueprints = new List<ProcessingBlueprint>();
    [SerializeField] public List<ProcessingFuelRequirement> validProcessingFuelRequirements;
    [SerializeField] public float processingTime;
}
