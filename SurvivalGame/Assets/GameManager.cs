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
