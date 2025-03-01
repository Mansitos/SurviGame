using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] protected GameObject grid;
    [SerializeField] protected GameObject craftingSlotUIPrefab;
    [SerializeField] protected List<ItemCraftBlueprint> craftBlueprints = new List<ItemCraftBlueprint>();

    protected GameManager gm;
    protected InventorySystem inventory;
    protected int numSlots;
    protected bool isActive;

    void Start()
    {
        PopulateSlots();
        SetActive(false);
    }

    void Update()
    {
        UpdatedUI();
    }

    protected void PopulateSlots()
    {
        foreach (ItemCraftBlueprint craftBlueprint in craftBlueprints)
        {
            GameObject slot = Instantiate(craftingSlotUIPrefab, grid.transform);
            slot.GetComponent<CraftSlotUI>().SetBluePrint(craftBlueprint);
        }
    }

    public void UpdatedUI()
    {
        foreach (Transform child in transform)
        {
            CraftSlotUI slot = child.GetComponent<CraftSlotUI>();
            if (slot != null)
            {
                slot.UpdateSlotUI();
            }

        }

    }

    public bool IsActive()
    {
        return isActive;
    }

    // TODO: Move on UI Utils?
    public virtual void SetActive(bool flag)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(flag);
        }

        if (flag == true)
        {
            //UpdateUI();
        }

        isActive = flag;
    }
}
