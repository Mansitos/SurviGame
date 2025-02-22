using UnityEngine;

public class QuickBarUI : BaseInventoryUI
{
    private PlayerQuickBar quickBar;
    protected override bool ItemsAreDraggable => false;
    public int quickBarSize = 10;

    protected override void Start()
    {
        numSlots = quickBarSize;
        base.Start();
        quickBar = gm.GetPlayerQuickBar();
    }

    protected override void InitSlots()
    {
        PopulateSlots(numSlots, SlotType.QuickBar);
    }

    private void Update()
    {
        UpdateActiveStatus();
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
}
