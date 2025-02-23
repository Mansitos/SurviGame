using UnityEngine.UI;
using UnityEngine;

public class QuickBarUI : BaseInventoryUI
{
    private int quickBarSize = 10;
    protected override bool ItemsAreDraggable => false;

    protected override void Start()
    {
        numSlots = quickBarSize;
        GameManager.Instance.GetPlayerQuickBar().OnChangedQuickBarSelection += HighlightSelectedSlot;
        base.Start();
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
        GameObject selectedSlot = uiSlots[index];
        Debug.Log(selectedSlot);
        selectedSlot.GetComponent<Image>().color = Color.red;
    }
}
