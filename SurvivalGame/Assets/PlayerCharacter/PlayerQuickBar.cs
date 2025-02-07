using UnityEngine;

public class PlayerQuickBar : MonoBehaviour
{
    [SerializeField] GameObject selectedItem = null;
    public Item selectedItemScript = null;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        updateStep();
    }

    private void updateSelectedItemScript()
    {
        if (selectedItem != null)
        {
            selectedItemScript = selectedItem.GetComponent<Item>();
        }
    }

    private void Update()
    {
        updateStep();
    }

    private void updateStep()
    {
        updateSelectedItemScript();
        updateTileSelectionGridVisibility();
    }

    public void setSelectedItem(GameObject item)
    {
        //TODO: checks if corret type of item?
        selectedItem = item;
        updateSelectedItemScript();
    }

    public void updateTileSelectionGridVisibility()
    {
        if (selectedItemScript.IsItemOfType(ItemType.Tool)){
            gm.getPlayerTileSelection().SetSelectionVisibility(true);
        }
        else
        {
            gm.getPlayerTileSelection().SetSelectionVisibility(false);
        }
    }
}
