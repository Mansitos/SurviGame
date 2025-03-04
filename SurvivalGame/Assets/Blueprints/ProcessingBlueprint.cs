using UnityEngine;

[CreateAssetMenu(fileName = "NewProcessingBlueprint", menuName = "Game/Blueprint/ProcessingBlueprint")]
public class ProcessingBlueprint : ScriptableObject
{
    [SerializeField] public ItemData itemOutput;
    public int itemOutputQuantity;
    [SerializeField] public ItemData itemInput;
    public int itemInputQuantity;
}
