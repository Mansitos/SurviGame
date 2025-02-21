using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main UI GO Components Refs")]
    public GameObject inventoryUIGO;
    public GameObject quickBarUIGO;

    // Script Components
    private InventoryUI inventoryUI;
    private QuickBarUI quickBarUI;

    void Awake()
    {
        inventoryUI = inventoryUIGO.GetComponent<InventoryUI>();
        quickBarUI = quickBarUIGO.GetComponent<QuickBarUI>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public QuickBarUI GetQuickBarUI()
    {
        return quickBarUI;
    }

    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }
}
