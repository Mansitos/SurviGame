using UnityEngine;

public class PlayerQuickBar : MonoBehaviour
{
    [SerializeField] GameObject selectedItem = null;
    public Item selectedItemScript = null;

    private void Start()
    {
        updateSelectedItemScript();
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
        updateSelectedItemScript();
    }

    public void setSelectedItem(GameObject item)
    {
        //TODO: checks?
        selectedItem = item;
        updateSelectedItemScript();
    }
}
