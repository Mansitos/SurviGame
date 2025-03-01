using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryUI : MonoBehaviour
{
    [SerializeField] protected GameObject grid;
    [SerializeField] protected GameObject inventoryUISlotPrefab;
    [SerializeField] protected GameObject inventoryUISlotCounterPrefab;
    protected virtual bool ItemsAreDraggable => true; // default
    [SerializeField] protected List<GameObject> uiSlots = new List<GameObject>();

    protected GameManager gm;
    protected InventorySystem inventory;
    protected int numSlots;
    protected bool isActive;

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        inventory = gm.GetInventorySystem();
        InitSlots();
        UpdateUI();
    }

    protected abstract void InitSlots(); // Each UI type initializes differently

    protected void PopulateSlots(int numToPopulate, SlotType slotType)
    {
        for (int i = 0; i < numToPopulate; i++)
        {
            GameObject slot = Instantiate(inventoryUISlotPrefab, grid.transform);
            slot.GetComponent<InventoryUISlot>().SetIndex(i);
            slot.GetComponent<InventoryUISlot>().SetSlotType(slotType);
            uiSlots.Add(slot);
        }
    }

    public virtual void SetActive(bool flag)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(flag);
        }

        if (flag == true)
        {
            UpdateUI();
        }

        isActive = flag;
    }

    public virtual bool IsActive() { return isActive; }

    // --- Update/Redraw UI Methods ---

    public virtual void UpdateUI()
    {
        UpdateSlots();
    }

    protected virtual void UpdateSlots()
    {
        List<InventorySlot> inventorySlots = inventory.GetInventorySlots();

        for (int i = 0; i < numSlots; i++)
        {
            InventorySlot inventorySlot = inventorySlots[i];
            InventoryUISlot UISlot = uiSlots[i].GetComponent<InventoryUISlot>();

            // Clear any previous item display
            UISlot.ClearSlot(destroyChild: true);

            if (!inventorySlot.IsEmpty())
            {
                GameObject itemIconObject = UIUtils.CreateItemIcon(inventorySlot.itemInstance, inventoryUISlotCounterPrefab, grid);
                itemIconObject.transform.SetParent(UISlot.transform, false);
                UISlot.SetDisplayedItem(itemIconObject, inventorySlot.itemInstance, draggable: ItemsAreDraggable);
            }
        }
    }

}
