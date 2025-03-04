using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Blueprint;

public class BuildingBlueprintSlotUI : BaseHorizontalBlueprintUISlot
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void UpdateSlotUI()
    {
        Debug.Log("asd");
    }

    protected override void InitUI()
    {
        Debug.Log("asd");
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("asd");
    }
}
