using UnityEngine;

public class ChestUI : MonoBehaviour
{
    [SerializeField] public GameObject chestUI;
    [SerializeField] public GameObject inventoryUI;

    private InventoryUI chest;
    private InventoryUI inventory;

    private bool isActive;

    public void Start()
    {
        chest = chestUI.GetComponent<InventoryUI>();
        inventory = inventoryUI.GetComponent<InventoryUI>();
        SetActive(false);
    }

    public void UpdateUI()
    {
        chest.InitSlots();
        chest.UpdateUI();
        inventory.UpdateUI();
    }

    public virtual void SetActive(bool flag)
    {
        chest.SetActive(flag, skipUpdate: true);
        inventory.SetActive(flag, skipUpdate: true);
        isActive = flag;

        if (flag == true)
        {
            UpdateUI();
        }
    }

    public GameObject GetLinkedChest()
    {
        return chest.GetTargetInventoryGO();
    }

    public virtual bool IsActive() { return isActive; }

    public void LinkChest(GameObject chestGO)
    {
        chest.SetTargetInventoryGO(chestGO);
    }
}
