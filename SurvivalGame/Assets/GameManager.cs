using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Main Game Components Refs")]
    public GameObject mainCamera;
    public GameObject terrainGridSystem;
    public GameObject player;

    private CameraManager mainCameraManager;
    private GridManager terrainGridManager;
    private BuildingPlacer buildingPlacer;
    private PlayerQuickBar playerQuickBar;
    private PlayerTileSelection playerTileSelection;
    private PlayerAnimationsHandler playerAnimationsHandler;
    private InventorySystem inventorySystem;

    // Game modes
    public bool isBuildMode = false;
    public bool isNormalMode = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void Awake()
    {
        mainCameraManager = GetComponent<CameraManager>();
        terrainGridManager = terrainGridSystem.GetComponent<GridManager>();
        buildingPlacer = terrainGridSystem.GetComponent<BuildingPlacer>();
        playerQuickBar = player.GetComponent<PlayerQuickBar>();
        playerTileSelection = player.GetComponent<PlayerTileSelection>();
        playerAnimationsHandler = player.GetComponent<PlayerAnimationsHandler>();
        inventorySystem = player.GetComponent <InventorySystem>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        UpdateIsInNormalMode();
    }

    public void UpdateIsInNormalMode()
    {
        if (isBuildMode)
        {
            isNormalMode = false;
        }
        else
        {
            isNormalMode = true;
        }
    }

    public bool IsInBuildMode()
    {
        return isBuildMode;
    }

    public void SetBuildMode(bool flag)
    {
        isBuildMode = flag;

    }

    public PlayerTileSelection getPlayerTileSelection()
    {
        return playerTileSelection;
    }

    public PlayerQuickBar getPlayerQickBar()
    {
        return playerQuickBar;
    }

    public CameraManager getMainCameraManager()
    {
        return mainCameraManager;
    }

    public GridManager getTerrainGridManager()
    {
        return terrainGridManager;
    }

    public BuildingPlacer getBuildingPlacer()
    {
        return buildingPlacer;
    }

    public GameObject getPlayer()
    {
        return player;
    }

    public GameObject getMainCamera()
    {
        return mainCamera;
    }

    public GameObject getTerrainGridSystem()
    {
        return terrainGridSystem;
    }

    public InventorySystem getInventorySystem()
    {
        return inventorySystem;
    }

    public PlayerAnimationsHandler getPlayerAnimationHandler()
    {
        return playerAnimationsHandler;
    }
}
