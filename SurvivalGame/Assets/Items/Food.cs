using UnityEngine;

public class Food : Consumable
{
    [SerializeField] private int hungerValue;
    [SerializeField] private int thirstValue;

    public int HungerValue => hungerValue;
    public int ThirstValue => thirstValue;

    public override ItemType GetItemType()
    {
        return ItemType.Food;
    }

    public override bool PerformMainAction()
    {
        Debug.Log("EAT!");
        return true;
    }
}
