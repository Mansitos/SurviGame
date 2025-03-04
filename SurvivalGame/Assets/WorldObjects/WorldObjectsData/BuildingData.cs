using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Game/WorldObjects/Building")]
public class BuildingData : WorldObjectData
{
    [SerializeField] public int xdimension = 1;
    [SerializeField] public int zdimension = 1;
    [SerializeField] public BuildingBlueprint blueprint;
    [SerializeField] public Sprite uiIcon;

}
