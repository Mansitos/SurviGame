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
    private GameManager gm;

    private void OnEnable()
    {
        InputHandler.OnInventoryKeyPressedEvent += OnInventoryKeyPressed;
        InputHandler.OnEscKeyPressedEvent += OnEscKeyPressed;
    }

    void Awake()
    {
        inventoryUI = inventoryUIGO.GetComponent<InventoryUI>();
        quickBarUI = quickBarUIGO.GetComponent<QuickBarUI>();
        processingStationUI = processingStationUIGO.GetComponent<ProcessingStationUI>();

        gm = GameManager.Instance;

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

    public ProcessingStationUI GetProcessingStationUI()
    {
        return processingStationUI;
    }

    public void CloseOpenInventoryTabs()
    {
        gm.SetInventoryMode(false);
        inventoryUI.SetActive(false);
        processingStationUI.SetActive(false);
    }

    private bool CanCloseInventory()
    {
        return !processingStationUI.IsActive();
    }

    public void SetProcessingStationTabActive(bool flag)
    {
        gm.SetInventoryMode(true);
        processingStationUI.SetActive(flag);
        inventoryUI.SetActive(flag);
    }

    public void OnInventoryKeyPressed()
    {
        bool newflag = !gm.IsInInventoryMode();

        if (newflag == false){ // handle closing
            if (CanCloseInventory()){
                gm.SetInventoryMode(newflag);
                inventoryUI.SetActive(newflag);
                quickBarUI.SetActive(!newflag);
            }
            else
            {
                Debug.Log("Can't close inventory now! required in some open tabs!");
            }
        }
        else // Handle opening
        {
            gm.SetInventoryMode(newflag);
            inventoryUI.SetActive(newflag);
            quickBarUI.SetActive(!newflag);
        }
    }

    public void OnEscKeyPressed()
    {
        CloseOpenInventoryTabs();

        if (gm.IsInNormalMode())
        {
            quickBarUI.SetActive(true);
        }
    }
}
