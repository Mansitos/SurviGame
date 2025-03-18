using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main UI GO Components Refs")]
    public GameObject inventoryUIGO;
    public GameObject quickBarUIGO;
    public GameObject processingStationUIGO;
    public GameObject refinementStationUIGO;
    public GameObject endOfDayUIGO;
    public GameObject craftingUIGO;
    public GameObject buildingUIGO;
    public GameObject chestUIGO;
    public GameObject itemsTooltipUIGO;
    public GameObject mouseInventoryUISlotGO;
    public GameObject itemPopupUIGO;

    // Script Components
    private InventoryUI inventoryUI;
    private QuickBarUI quickBarUI;
    private ProcessingStationUI processingStationUI;
    private RefinementStationUI refinementStationUI;
    private EndOfDayUI endOfDayUI;
    private CraftingUI craftingUI;
    private BuildingUI buildingUI;
    private ChestUI chestUI;
    private ItemsTooltipUI itemsTooltipUI;
    private GameManager gm;
    private MouseInventoryUISlot mouseInventoryUISlot;
    private ItemPopupManager itemPopupUI;

    public GameObject inventoryUISlotCounterPrefab;


    private void OnEnable()
    {
        InputHandler.OnInventoryKeyPressedEvent += OnInventoryKeyPressed;
        InputHandler.OnEscKeyPressedEvent += OnEscKeyPressed;
        InputHandler.OnCraftingKeyPressedEvent += OnCraftingKeyPressed;
        InputHandler.OnBuildingKeyPressedEvent += OnBuildingKeyPressed;
        Chest.OnChestOpen += OneChestOpened;
    }

    void Awake()
    {
        inventoryUI = inventoryUIGO.GetComponent<InventoryUI>();
        quickBarUI = quickBarUIGO.GetComponent<QuickBarUI>();
        processingStationUI = processingStationUIGO.GetComponent<ProcessingStationUI>();
        endOfDayUI = endOfDayUIGO.GetComponent<EndOfDayUI>();
        craftingUI = craftingUIGO.GetComponent<CraftingUI>();
        buildingUI = buildingUIGO.GetComponent<BuildingUI>();
        chestUI = chestUIGO.GetComponent<ChestUI>();
        itemsTooltipUI = itemsTooltipUIGO.GetComponent<ItemsTooltipUI>();
        mouseInventoryUISlot = mouseInventoryUISlotGO.GetComponent<MouseInventoryUISlot>();
        itemPopupUI = itemPopupUIGO.GetComponent<ItemPopupManager>();
        refinementStationUI = refinementStationUIGO.GetComponent<RefinementStationUI>();

        gm = GameManager.Instance;

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public RefinementStationUI GetRefinementStationUI()
    {
        return refinementStationUI;
    }

    public ItemPopupManager GetItemPopupManager()
    {
        return itemPopupUI;
    }

    public MouseInventoryUISlot GetMouseInventorySlot()
    {
        return mouseInventoryUISlot;
    }

    public ItemsTooltipUI GetItemsTooltipUI()
    {
        return itemsTooltipUI;
    }

    public ChestUI GetChestUI()
    {
        return chestUI;
    }

    public CraftingUI GetCraftingUI()
    {
        return craftingUI;
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

    public EndOfDayUI GetEndOfDayUI()
    {
        return endOfDayUI;
    }

    public void CloseOpenInventoryTabs()
    {
        gm.SetInventoryMode(false);
        inventoryUI.SetActive(false);
        CloseProcessingStationUI();
        CloseRefinementStationUI();
        craftingUI.SetActive(false);
        buildingUI.SetActive(false);
        chestUI.SetActive(false);
        itemsTooltipUI.HideTooltip();
        mouseInventoryUISlot.Clear(interruptedClear: true);

        quickBarUI.SetActive(true);
    }

    private void CloseProcessingStationUI()
    {
        processingStationUI.SetActive(false);
        processingStationUI.UnLinkStation();
    }

    private void CloseRefinementStationUI()
    {
        refinementStationUI.SetActive(false);
        refinementStationUI.UnLinkStation();
    }

    private bool CanCloseInventory()
    {
        return !refinementStationUI.IsActive() && !processingStationUI.IsActive() && !craftingUI.IsActive() && !buildingUI.IsActive() && !chestUI.IsActive();
    }

    public void SetProcessingStationTabActive(bool flag)
    {
        gm.SetInventoryMode(true);
        processingStationUI.SetActive(flag);
        inventoryUI.SetActive(flag);

        quickBarUI.SetActive(false);
    }

    public void SetRefinementStationTabActive(bool flag)
    {
        gm.SetInventoryMode(true);
        refinementStationUI.SetActive(flag);
        inventoryUI.SetActive(flag);

        quickBarUI.SetActive(false);
        refinementStationUI.UpdateUI();
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

    public void OnBuildingKeyPressed()
    {
        bool newflag = !buildingUI.IsActive();
        if (newflag == true && !gm.IsInNormalMode())
        {
            return;
        }
        gm.SetInventoryMode(newflag); // TODO: not properly inventory mode... refactor modes?
        buildingUI.SetActive(newflag);

        quickBarUI.SetActive(!newflag);
    }

    public void OnCraftingKeyPressed()
    {
        bool newflag = !craftingUI.IsActive();
        if (newflag == true && !gm.IsInNormalMode())
        {
            return;
        }
        gm.SetInventoryMode(newflag);
        craftingUI.SetActive(newflag);
        inventoryUI.SetActive(newflag);

        quickBarUI.SetActive(!newflag);
    }

    public void OneChestOpened(GameObject chest)
    {
        chestUI.LinkChest(chest);
        gm.SetInventoryMode(true);
        chestUI.SetActive(true);
        quickBarUI.SetActive(false);
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
