using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : BaseScrollableCraftUI
{
    [SerializeField] protected List<Blueprint> craftBlueprints = new List<Blueprint>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void PopulateSlots()
    {
        foreach (ItemBlueprint craftBlueprint in craftBlueprints)
        {
            GameObject slot = Instantiate(slotUIPrefab, verticalGrid.transform);
            slot.GetComponent<CraftSlotUI>().SetBluePrint(craftBlueprint);
        }
    }
}
