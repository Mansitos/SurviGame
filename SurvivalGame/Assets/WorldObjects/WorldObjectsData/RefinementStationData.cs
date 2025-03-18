using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RefinementStation", menuName = "Game/WorldObjects/RefinementStation")]
public class RefinementStationData : BuildingData
{
    // Variables
    [SerializeField] public List<RefinementBlueprint> possibleRefinements = new List<RefinementBlueprint>();
}
