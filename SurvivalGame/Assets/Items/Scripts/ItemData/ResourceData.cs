using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Game/Item/Resource")]
public class ResourceData : ItemData
{
    private void OnValidate()
    {
        itemType = ItemType.Resource;
    }

    private void OnEnable()
    {
        itemType = ItemType.Resource;
    }
}
