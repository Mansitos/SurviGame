using UnityEngine;

public class PlayerQuickBar : MonoBehaviour
{
    [SerializeField] private ItemData selectedItemData; // Assign in Inspector
    private ItemInstance selectedItemInstance; // Runtime item instance

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;

        // Create an instance at runtime if an ItemData is set
        if (selectedItemData != null)
        {
            selectedItemInstance = new ItemInstance(selectedItemData, 1);
        }

        updateStep();
    }

    private void Update()
    {
        updateStep();
    }

    private void updateStep()
    {
        // Ensure selectedItemInstance is updated when selectedItemData changes
        if (selectedItemData != null && (selectedItemInstance == null || selectedItemInstance.ItemData != selectedItemData))
        {
            selectedItemInstance = new ItemInstance(selectedItemData, 1);
        }

        updateTileSelectionGridVisibility();
    }

    public ItemInstance GetSelectedItemInstance()
    {
        return selectedItemInstance;
    }

    public void setSelectedItem(ItemInstance itemInstance)
    {
        //TODO: checks if corret type of item?
        selectedItemInstance = itemInstance;
        updateTileSelectionGridVisibility();
    }

    private void updateTileSelectionGridVisibility()
    {
        if (selectedItemInstance.ItemData != null && selectedItemInstance.ItemData.IsItemOfType(ItemType.Tool))
        {
            gm.getPlayerTileSelection().SetSelectionVisibility(true);
        }
        else
        {
            gm.getPlayerTileSelection().SetSelectionVisibility(false);
        }
    }
}
