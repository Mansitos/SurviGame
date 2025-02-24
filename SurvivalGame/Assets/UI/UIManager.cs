using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main UI GO Components Refs")]
    public GameObject inventoryUIGO;
    public GameObject quickBarUIGO;
    public GameObject processingStationUIGO;

    // Script Components
    private InventoryUI inventoryUI;
    private QuickBarUI quickBarUI;
    private ProcessingStationUI processingStationUI;

    void Awake()
    {
        inventoryUI = inventoryUIGO.GetComponent<InventoryUI>();
        quickBarUI = quickBarUIGO.GetComponent<QuickBarUI>();
        processingStationUI = processingStationUIGO.GetComponent<ProcessingStationUI>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Update()
    {
        if (GameManager.Instance.GetInputHandler().WasEscInventoryPressedThisFrame())
        {
            CloseOpenInventoryTabs();
            Debug.Log("CLOSEEE");
        }
    }

    public QuickBarUI GetQuickBarUI()
    {
        return quickBarUI;
    }

    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }

    public ProcessingStationUI GetProcessingStationUI()
    {
        return processingStationUI;
    }

    public void CloseOpenInventoryTabs()
    {
        GameManager.Instance.SetInventoryMode(false);
        inventoryUI.SetActive(false);
        processingStationUIGO.SetActive(false);
    }
}
