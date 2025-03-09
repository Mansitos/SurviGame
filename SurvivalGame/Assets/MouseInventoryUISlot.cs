using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInventoryUISlot : MonoBehaviour
{
    public Vector2 offset = new Vector2(30f, -30f);
    public GameObject slot;
    public InventoryUISlot referenceSlot;

    private CanvasGroup canvas;

    private void OnEnable()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            if (slot.GetComponent<InventoryUISlot>().hasDisplayedItem)
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

    public void SetReferenceUISlot(InventoryUISlot inventoryUISlot)
    {
        referenceSlot = inventoryUISlot;
    }
}
