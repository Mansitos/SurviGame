using UnityEngine;

public class EndOfDayUI : MonoBehaviour
{

    private GameTimeManager gameTimeManager;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
        gameTimeManager = gm.GetGameTimeManager();
    }

    void Update()
    {
    }

    public void OnNextDayButtonPressed()
    {
        gameTimeManager.TriggerNextDay();
    }

    public void SetActive(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
