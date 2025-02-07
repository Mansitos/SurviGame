using UnityEngine;

public enum WeaponType
{
    Sword,
    Spear,
    Bow
}

public class Weapon : Tool
{
    [SerializeField] private WeaponType weaponCategory;

    public WeaponType WeaponCategory => weaponCategory;

    public override ItemType GetItemType()
    {
        return ItemType.Weapon;
    }

    public override bool PerformMainAction()
    {
        Debug.Log("ATTACK");
        return true;
    }
}