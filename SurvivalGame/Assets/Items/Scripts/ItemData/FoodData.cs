using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Game/Item/Food")]
public class FoodData : UsableData, IUsableItem
{
    [SerializeField] private int hungerValue;
    [SerializeField] private int thirstValue;
    [SerializeField] private int energyValue;

    public int HungerValue => hungerValue;
    public int ThirstValue => thirstValue;
    public int EnergyValue => energyValue;

    new public bool PerformMainAction(GameManager gm)
    {
        InventorySystem inventory = gm.GetInventorySystem();
        inventory.TryRemoveItem(new ItemInstance(this, 1));
        PlayerStatus playerStatus = gm.GetPlayerStatus();
        playerStatus.AddFood(hungerValue);
        playerStatus.AddThirst(thirstValue);
        playerStatus.AddEnergy(energyValue);
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
