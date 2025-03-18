using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopupManager : MonoBehaviour
{
    [Header("Popup Settings")]
    public GameObject itemPopupPrefab; // Assign your ItemPopup prefab here
    public Transform popupContainer;   // Assign the PopupContainer (with Vertical Layout Group) here

    // Dictionary to track active popups keyed by the item data
    private Dictionary<ItemData, ItemPopup> activePopups = new Dictionary<ItemData, ItemPopup>();

    private void OnEnable()
    {
        DroppedItem.OnItemCollected += ShowItemPopup;
    }

    private void OnDisable()
    {
        DroppedItem.OnItemCollected -= ShowItemPopup;
    }

    public void ShowItemPopup(ItemInstance item)
    {
        // Check if a popup for this item's type already exists
        if (activePopups.TryGetValue(item.ItemData, out ItemPopup existingPopup))
        {
            // Update the existing popup: increment count and reset its fade timer
            existingPopup.IncrementPopup();
        }
        else
        {
            // Instantiate a new popup as a child of the container so that the Vertical Layout Group positions it automatically
            GameObject popupInstance = Instantiate(itemPopupPrefab, popupContainer);
            popupInstance.transform.SetAsLastSibling(); // Ensure it appears at the bottom
            ItemPopup popup = popupInstance.GetComponent<ItemPopup>();
            if (popup != null)
            {
                popup.ShowPopup(item);
                // Add the popup to our dictionary
                activePopups[item.ItemData] = popup;
                // When the popup is finished (destroyed), remove it from the dictionary.
                popup.OnPopupDestroyed += () => activePopups.Remove(item.ItemData);
            }
        }
    }
}
