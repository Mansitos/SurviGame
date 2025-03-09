using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInventoryUISlot : MonoBehaviour
{
    public Vector2 offset = new Vector2(30f, -30f);
    public GameObject slot;
    public InventoryUISlot referenceUISlot;

    private CanvasGroup canvas;

    private void OnEnable()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            if (slot.GetComponent<InventoryUISlot>().IsDisplayingAnItemIcon())
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

    public void Clear(bool interruptedClear = false)
    {
        if (interruptedClear)
        {
            Debug.LogWarning("IMPLEMENT: add back to referenceUiSlot and then clear");
        }
        slot.GetComponent<InventoryUISlot>().ClearSlot(destroyChild: true);
        referenceUISlot = null;
    }

    public void SetOriginReferenceSlot(InventoryUISlot inventoryUISlot)
    {
        referenceUISlot = inventoryUISlot;
    }
}
