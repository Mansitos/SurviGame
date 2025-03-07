using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Game/Item/Food")]
public class FoodData : UsableData, IUsableItem
{
    [SerializeField] private int hungerValue;
    [SerializeField] private int thirstValue;
    [SerializeField] private int energyValue;
    [SerializeField] private float rottenChance = 0.5f;

    public int HungerValue => hungerValue;
    public int ThirstValue => thirstValue;
    public int EnergyValue => energyValue;
    public float RottenChance => rottenChance;

    new public bool PerformMainAction(GameManager gm)
    {
        InventorySystem inventory = gm.GetPlayerInventory();
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

    public int Rotten(ItemInstance item)
    {
        int actualQuantity = item.Quantity;
        int rottenCount = 0;

        for (int i = 0; i < actualQuantity; i++)
        {
            if (Random.value < rottenChance)
            {
                rottenCount++;
            }
        }

        int afterQuantity = actualQuantity - rottenCount;
        item.RemoveQuantity(rottenCount);
        return afterQuantity;
    }
}
