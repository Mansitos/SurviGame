using UnityEngine;

public enum WeaponType
{
    Sword,
    Spear,
    Bow
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Item/Weapon")]
public class WeaponData : ToolData, IUsableItem
{
    [SerializeField] private WeaponType weaponCategory;
    public WeaponType WeaponCategory => weaponCategory;

    new public bool PerformMainAction(GameManager gm)
    {
        Debug.Log("Weapon main action!");
        return true;
    }

    private void OnValidate()
    {
        itemType = ItemType.Weapon;
    }

    private void OnEnable()
    {
        itemType = ItemType.Weapon;
    }
}