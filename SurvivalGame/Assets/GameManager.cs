using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Main Game Components Refs")]
    public GameObject mainCamera;
    public GameObject terrainGridSystem;
    public GameObject player;
    public GameObject inventoryUIGO;
    public GameObject quickBarUIGO;

    // Components
    private CameraManager mainCameraManager;
    private GridManager terrainGridManager;
    private BuildingPlacer buildingPlacer;
    private PlayerQuickBar playerQuickBar;
    private PlayerTileSelection playerTileSelection;
    private PlayerAnimationsHandler playerAnimationsHandler;
    private InventorySystem inventorySystem;

    // UIs
    private InventoryUI inventoryUI;
    private QuickBarUI quickBarUI;

    // Game modes
    private bool isBuildMode = false;
    private bool isInventoryMode = false;
    private bool isNormalMode = true;

    private void Awake()
    {
        mainCameraManager = GetComponent<CameraManager>();
        terrainGridManager = terrainGridSystem.GetComponent<GridManager>();
        buildingPlacer = terrainGridSystem.GetComponent<BuildingPlacer>();
        playerQuickBar = player.GetComponent<PlayerQuickBar>();
        playerTileSelection = player.GetComponent<PlayerTileSelection>();
        playerAnimationsHandler = player.GetComponent<PlayerAnimationsHandler>();
        inventorySystem = player.GetComponent<InventorySystem>();
        inventoryUI = inventoryUIGO.GetComponent<InventoryUI>();
        quickBarUI = quickBarUIGO.GetComponent <QuickBarUI>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
    }

    public void UpdateIsInNormalMode()
    {
        if (isBuildMode || isInventoryMode)
        {
            Debug.Log("not normal mode!");
            isNormalMode = false;
        }
        else
        {
            isNormalMode = true;
        }
    }

    public bool IsInNormalMode()
    {
        UpdateIsInNormalMode();
        return isNormalMode;
    }

    public QuickBarUI GetQuickBarUI()
    {
        return quickBarUI;
    }

    public InventoryUI GetInventoryUI()
    {
        return inventoryUI;
    }

    public bool IsInBuildMode()
    {
        return isBuildMode;
    }

    public bool IsInInventoryMode()
    {
        return isInventoryMode;
    }

    public void SetBuildMode(bool flag)
    {
        isBuildMode = flag;
    }

    public void SetInventoryMode(bool flag)
    {
        isInventoryMode = flag;
    }

    public PlayerTileSelection GetPlayerTileSelection()
    {
        return playerTileSelection;
    }

    public PlayerQuickBar GetPlayerQuickBar()
    {
        return playerQuickBar;
    }

    public CameraManager GetMainCameraManager()
    {
        return mainCameraManager;
    }

    public GridManager GetTerrainGridManager()
    {
        return terrainGridManager;
    }

    public BuildingPlacer GetBuildingPlacer()
    {
        return buildingPlacer;
    }

    public GameObject GetPlayerGO()
    {
        return player;
    }

    public GameObject GetMainCameraGO()
    {
        return mainCamera;
    }

    public GameObject GetTerrainGridSystemGO()
    {
        return terrainGridSystem;
    }

    public InventorySystem GetInventorySystem()
    {
        return inventorySystem;
    }

    public PlayerAnimationsHandler GetPlayerAnimationHandler()
    {
        return playerAnimationsHandler;
    }
}
