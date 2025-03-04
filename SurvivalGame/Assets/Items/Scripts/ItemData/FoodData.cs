using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Game/Item/Food")]
public class FoodData : UsableData, IUsableItem
{
    [SerializeField] private int hungerValue;
    [SerializeField] private int thirstValue;

    public int HungerValue => hungerValue;
    public int ThirstValue => thirstValue;

    new public bool PerformMainAction(GameManager gm)
    {
        Debug.Log("EAT!");
        return true;
    }

    private void OnValidate()
    {
        itemType = ItemType.Food;
    }

    private void OnEnable()
    {
        itemType = ItemType.Food;
    }
}
