public abstract class Consumable : Item
{

    public override ItemType GetItemType()
    {
        return ItemType.Consumable;
    }
}
