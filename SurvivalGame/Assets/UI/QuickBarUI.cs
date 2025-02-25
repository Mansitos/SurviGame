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
        SetActive(true);
    }

    protected override void InitSlots()
    {
        PopulateSlots(numSlots, SlotType.QuickBar);
    }

    private void Update()
    {
        //TODO: dummy for now, in future event called when OnChanged
        HighlightSelectedSlot(GameManager.Instance.GetPlayerQuickBar().selectedIndex);
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

    // --- Update/Redraw UI Methods ---

}
