using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData), true)]
public class GiveItemDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemData itemData = (ItemData)target;

        if (GUILayout.Button("Give x1"))
        {
            InventorySystem inventory = GameManager.Instance.GetPlayerInventory();
            inventory.TryAddItem(new ItemInstance(itemData));
        }

        if (GUILayout.Button("Remove x1"))
        {
            InventorySystem inventory = GameManager.Instance.GetPlayerInventory();
            inventory.TryRemoveItem(new ItemInstance(itemData));
        }

        if (GUILayout.Button("Give x10"))
        {
            InventorySystem inventory = GameManager.Instance.GetPlayerInventory();
            for (int i = 0; i<10; i++)
            {
                inventory.TryAddItem(new ItemInstance(itemData));
            }
        }
    }
}
