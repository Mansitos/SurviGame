using UnityEngine;

[CreateAssetMenu(fileName = "NewProcessingItemBlueprint", menuName = "Game/ProcessingItemBlueprint")]
public class ProcessingItemCraftBlueprint : ScriptableObject
{
    [SerializeField] public ItemData itemOutput;
    public int itemOutputQuantity;
    [SerializeField] public ItemData itemInput;
    public int itemInputQuantity;
}
