using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInventoryUISlot : MonoBehaviour
{
    public Vector2 offset = new Vector2(30f, -30f);
    public GameObject inventorySlotGO;
    public InventorySlot originInventorySlot;

    private InventorySlot inventorySlot;
    private InventoryUISlot inventoryUISlot;

    private CanvasGroup canvas;

    private void OnEnable()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventorySlot = new InventorySlot(null);
        inventoryUISlot = inventorySlotGO.GetComponent<InventoryUISlot>();
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            if (!inventorySlot.IsEmpty())
            {
                canvas.alpha = 1;
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                transform.position = mousePosition + offset;
            }
            else
            {
                canvas.alpha = 0;
            }
        }
    }

    public InventoryUISlot GetInventoryUISlot()
    {
        return inventoryUISlot;
    }

    public InventorySlot GetInventorySlot()
    {
        return inventorySlot;
    }

    public GameObject GetInventorySlotGO()
    {
        return inventorySlotGO;
    }

    public void Clear(bool interruptedClear = false)
    {
        if (interruptedClear) 
        {
            if (!inventorySlot.IsEmpty()) // Closed while still carrying items
            {
                Debug.Log("Mouse right inventory force closed. restoring items to origin");
                originInventorySlot.AddItem(inventorySlot.itemInstance);
            }
        }
        inventorySlot.ClearSlot();
        inventoryUISlot.ClearSlot(destroyChild: true);
        originInventorySlot = null;
    }

    public void SetOriginReferenceSlot(InventorySlot slot)
    {
        originInventorySlot = slot;
    }
}
