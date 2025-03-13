using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ItemPopup : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup canvasGroup;         // For controlling alpha during fade
    public Image itemIcon;                  // The UI image to display the item icon
    public TextMeshProUGUI itemNameText;    // The UI text to display the item name and count

    [Header("Animation Settings")]
    public float fadeInDuration = 0.3f;
    public float displayDuration = 1.5f;
    public float fadeOutDuration = 0.3f;

    // Internal state
    private int count = 1;
    private ItemData itemData;              // The item data for this popup
    private Coroutine waitCoroutine;        // Reference to the wait coroutine

    // Event so the manager can know when this popup is finished
    public event Action OnPopupDestroyed;

    // Initializes and shows the popup for a given item.
    public void ShowPopup(ItemInstance item)
    {
        itemData = item.ItemData;
        count = 1;
        if (itemIcon != null)
            itemIcon.sprite = item.ItemData.uiIcon;
        if (itemNameText != null)
            itemNameText.text = $"{item.ItemData.itemName} x{count}";

        // Start fade-in animation
        StartCoroutine(AnimateFadeIn());
    }

    // Fade in coroutine
    private IEnumerator AnimateFadeIn()
    {
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        // After fade in, start the waiting period before fading out
        waitCoroutine = StartCoroutine(WaitAndFadeOut());
    }

    // Coroutine that waits and then fades out the popup
    private IEnumerator WaitAndFadeOut()
    {
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(AnimateFadeOut());
        OnPopupDestroyed?.Invoke();
        Destroy(gameObject);
    }

    // Fade out coroutine
    private IEnumerator AnimateFadeOut()
    {
        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    // Call this method when another item of the same type is collected.
    // It increments the count, updates the UI, and resets the fade-out timer.
    public void IncrementPopup()
    {
        count++;
        if (itemNameText != null)
        {
            itemNameText.text = $"{itemData.itemName} x{count}";
        }
        // Reset the fade-out timer by stopping the current wait coroutine and starting a new one.
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
        waitCoroutine = StartCoroutine(WaitAndFadeOut());
    }
}
