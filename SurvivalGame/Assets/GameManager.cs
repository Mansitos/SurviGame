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

    // Game modes
    public bool isBuildMode = false;

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

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
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
}
