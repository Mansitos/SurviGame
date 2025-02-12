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
            InventorySystem inventory = GameManager.Instance.getInventorySystem();
            bool result = inventory.TryAddItem(new ItemInstance(itemData));
            string resultMessage = result ? "Added to inventory" : "Cannot add to inventory";
            Debug.Log(resultMessage);
        }

        if (GUILayout.Button("Remove x1"))
        {
            InventorySystem inventory = GameManager.Instance.getInventorySystem();
            bool result = inventory.TryRemoveItem(new ItemInstance(itemData));
            string resultMessage = result ? "Removed from inventory" : "Cannot remove from inventory";
            Debug.Log(resultMessage);
        }
    }
}
