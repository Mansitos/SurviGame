using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/BuildingData")]
public class BuildingData : WorldObjectData
{
    [SerializeField] public int xdimension = 1;
    [SerializeField] public int zdimension = 1;
    [SerializeField] public BuildingBlueprint blueprint;

}
