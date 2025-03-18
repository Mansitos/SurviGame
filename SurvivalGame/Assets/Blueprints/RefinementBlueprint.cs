using UnityEngine;

[CreateAssetMenu(fileName = "NewRefinementBlueprint", menuName = "Game/Blueprint/RefinementBlueprint")]
public class RefinementBlueprint : ScriptableObject
{
    [SerializeField] public int requiredDaysToRefine;
    [SerializeField] public ItemData itemOutput;
    [SerializeField] public ItemData itemInput;
}
