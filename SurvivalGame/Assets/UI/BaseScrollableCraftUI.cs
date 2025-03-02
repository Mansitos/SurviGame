using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScrollableCraftUI : MonoBehaviour
{
    [SerializeField] protected GameObject verticalGrid;
    [SerializeField] protected GameObject slotUIPrefab;
    [SerializeField] protected List<CraftBlueprint> craftBlueprints = new List<CraftBlueprint>();

    protected GameManager gm;
    protected InventorySystem inventory;
    protected int numSlots;
    protected bool isActive;

    protected virtual void Start()
    {
        PopulateSlots();
        SetActive(false);
    }

    protected virtual void Update()
    {
        UpdatedUI();
    }

    abstract protected void PopulateSlots();

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
