using UnityEngine.UI;
using UnityEngine;

public class QuickBarUI : BaseInventoryUI
{
    private int quickBarSize = 10;
    protected override bool ItemsAreDraggable => false;

    private Color unselectedColor;

    protected override void Start()
    {
        numSlots = quickBarSize;
        GameManager.Instance.GetPlayerQuickBar().OnChangedQuickBarSelection += HighlightSelectedSlot;
        base.Start();

        unselectedColor = uiSlots[0].GetComponent<Image>().color;
    }

    protected override void InitSlots()
    {
        PopulateSlots(numSlots, SlotType.QuickBar);
    }

    private void Update()
    {
        UpdateActiveStatus();

        //TODO: dummy for now, in future event called when changed
        HighlightSelectedSlot(GameManager.Instance.GetPlayerQuickBar().selectedIndex);
    }

    protected override void UpdateActiveStatus()
    {
        bool newStatus = !gm.IsInInventoryMode();
        bool oldStatus = grid.activeSelf;

        grid.SetActive(newStatus);

        if (oldStatus != newStatus)
        {
            UpdateSlots();
        }
    }

    public void HighlightSelectedSlot(int index)
    {
        foreach (GameObject slot in uiSlots)
        {
            slot.GetComponent<Image>().color = unselectedColor;
        }

        GameObject selectedSlot = uiSlots[index];
        selectedSlot.GetComponent<Image>().color = Color.red;
    }
}
