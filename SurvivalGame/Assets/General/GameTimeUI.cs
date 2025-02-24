using UnityEngine;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayText;
    private GameTimeManager timeManager;

    void Start()
    {
        timeManager = GameManager.Instance.GetGameTimeManager();
        timeManager.OnTimeChanged += UpdateTimeUI;
        timeManager.OnDayEnded += UpdateDayUI;

        UpdateTimeUI(timeManager.GetCurrentHour(), timeManager.GetCurrentMinutes());
        UpdateDayUI(timeManager.GetCurrentDay());
    }

    private void UpdateTimeUI(int hour, int minute)
    {
        timeText.text = $"{hour:D2}:{minute:D2}";
    }

    private void UpdateDayUI(int day)
    {
        dayText.text = $"Day {day}";
    }
}
