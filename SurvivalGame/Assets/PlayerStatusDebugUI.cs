using UnityEngine;
using TMPro;
using System.Collections;

public class DebugPlayerStatusUI : MonoBehaviour
{
    private PlayerStatus playerStatus; // Reference to PlayerStatus
    private TextMeshProUGUI statusText; // Reference to TMP UI Text

    private void Start()
    {
        playerStatus = GameManager.Instance.GetPlayerStatus();
        statusText = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateStatusUI());
    }

    private IEnumerator UpdateStatusUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f); // Update every second

            // Read Player Status
            float health = playerStatus.GetHealth();
            float maxHealth = playerStatus.GetMaxHealth();
            float food = playerStatus.GetFood();
            float maxFood = playerStatus.GetMaxFood();
            float thirst = playerStatus.GetThirst();
            float maxThirst = playerStatus.GetMaxThirst();
            float energy = playerStatus.GetEnergy();
            float maxEnergy = playerStatus.GetMaxEnergy();

            // Update UI text
            statusText.text = $"Health: {health}/{maxHealth}\n" +
                              $"Food: {food}/{maxFood}\n" +
                              $"Thirst: {thirst}/{maxThirst}\n" +
                              $"Energy: {energy}/{maxEnergy}";
        }
    }
}
