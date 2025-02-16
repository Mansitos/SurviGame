using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemCraftBlueprint))]
public class ItemCraftBlueprintDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemCraftBlueprint blueprint = (ItemCraftBlueprint)target;

        if (GUILayout.Button("Craft"))
        {
            InventorySystem inventory = GameManager.Instance.GetInventorySystem();

            blueprint.Craft(inventory);

        }
    }
}
