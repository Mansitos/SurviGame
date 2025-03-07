using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemBlueprint))]
public class ItemCraftBlueprintDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemBlueprint blueprint = (ItemBlueprint)target;

        if (GUILayout.Button("Craft"))
        {
            InventorySystem inventory = GameManager.Instance.GetPlayerInventory();

            blueprint.Craft(inventory);

        }
    }
}
