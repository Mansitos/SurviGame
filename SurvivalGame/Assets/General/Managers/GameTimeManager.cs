using UnityEngine;
using System;
using System.Collections.Generic;

public class GameTimeManager : MonoBehaviour
{
    public int startHour = 8;  // Start of day (8:00 AM)
    public int endHour = 22;   // End of day (10:00 PM)
    public float realTimeDayDuration = 600f; // Full day duration in seconds (10 min IRL)

    private int currentDay = 1;
    private int currentHour;
    private int currentMinute;
    private float timePerSlot;
    private float timer = 0;

    private GameManager gm;
    private EndOfDayUI ui;

    public event Action<int, int> OnTimeChanged;  // Event for UI updates
    public event Action<int> OnDayEnded;  // Event when day ends

    void Start()
    {
        ResetDay();
        gm = GameManager.Instance;
        ui = UIManager.Instance.GetEndOfDayUI();
    }

    void Update()
    {
        timer += Time.deltaTime;

        while (timer >= timePerSlot)
        {
            timer -= timePerSlot;
            AdvanceTime();
        }
    }

    private void ResetDay()
    {
        currentHour = startHour;
        currentMinute = 0;
        timer = 0f;

        // Calculate real time duration for each 10-minute slot
        int totalGameMinutes = (endHour - startHour) * 60; // Total in-game minutes in a day
        int totalSlots = totalGameMinutes / 10; // Number of 10-minute slots
        timePerSlot = realTimeDayDuration / totalSlots; // Time per slot in real-world seconds

        OnTimeChanged?.Invoke(currentHour, currentMinute);
    }

    private void AdvanceTime()
    {
        currentMinute += 10;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
        }

        if (currentHour >= endHour)
        {
            HandleEndDay(fromSleep:false);
        }
        else
        {
            OnTimeChanged?.Invoke(currentHour, currentMinute);
        }
    }

    private void EndDay()
    {
        currentDay++;
        OnDayEnded?.Invoke(currentDay);
        ResetDay();
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    public int GetCurrentHour()
    {
        return currentHour;
    }

    public int GetCurrentMinutes()
    {
        return currentMinute;
    }

    public string GetCurrentTimeFormatted()
    {
        return $"{currentHour:D2}:{currentMinute:D2}";
    }

    public void TriggerNextDay()
    {
        EndDay();
        ui.SetActive(false);
        Time.timeScale = 1.0f; // un-pause game
    }

    public void HandleEndDay(bool fromSleep)
    {
        HandleWorldUpdate();
        ui.SetActive(true);
        Time.timeScale = 0f; // pause game

        if (fromSleep == false)
        {
            Debug.Log("Applying did not sleep penalty for next day");
        }
        else
        {
            Debug.Log("Removing did not sleep penalty for next day");
        }

        gm.GetPlayerStatus().SetDidNotSleepPenalty(!fromSleep);
        gm.GetPlayerStatus().Sleep();
    }

    private void HandleWorldUpdate()
    {
        FoodRottingLogic();
        Debug.Log("World updated at end day");
    }

    private void FoodRottingLogic()
    {
        InventorySystem[] inventories = FindObjectsByType<InventorySystem>(FindObjectsSortMode.None);

        foreach (InventorySystem inventory in inventories)
        {
            List<InventorySlot> slotsWithFood = inventory.GetSlotsWithItemType(ItemType.Food);

            foreach (InventorySlot slot in slotsWithFood)
            {
                FoodData data = slot.itemInstance.ItemData as FoodData;
                data.Rotten(slot.itemInstance);

                if (slot.itemInstance.Quantity <= 0)
                {
                    slot.ClearSlot();
                }
            }

            inventory.UpdateUI();
        }
    }
}
