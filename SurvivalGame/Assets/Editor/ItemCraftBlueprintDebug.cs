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
            InventorySystem inventory = GameManager.Instance.getInventorySystem();

            bool canCraft = blueprint.CanCraft(inventory);
            bool result = false;
            if (canCraft)
            {
                result = blueprint.Craft(inventory);
            }

            string resultMessage = canCraft ? (result ? "Crafting successful!" : "Crafting failed!") : "Cannot craft due to insufficient resources or space.";
            Debug.Log(resultMessage);
        }
    }
}
