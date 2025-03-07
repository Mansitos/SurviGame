using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Main Game GO Components Refs")]
    public GameObject mainCamera;
    public GameObject terrainGridSystem;
    public GameObject player;
    public GameObject timeManager;

    // Script Components
    private CameraManager mainCameraManager;
    private GridManager terrainGridManager;
    private BuildingPlacer buildingPlacer;
    private PlayerQuickBar playerQuickBar;
    private PlayerTileSelection playerTileSelection;
    private PlayerAnimationsHandler playerAnimationsHandler;
    private InventorySystem playerInventory;
    private GameTimeManager gameTimeManager;
    private InputHandler inputHandler;
    private PlayerStatus playerStatus;

    // Game modes
    public bool isBuildMode = false;
    public bool isInventoryMode = false;
    public bool isNormalMode = true;

    private void Awake()
    {
        mainCameraManager = GetComponent<CameraManager>();
        terrainGridManager = terrainGridSystem.GetComponent<GridManager>();
        buildingPlacer = terrainGridSystem.GetComponent<BuildingPlacer>();
        playerQuickBar = player.GetComponent<PlayerQuickBar>();
        playerTileSelection = player.GetComponent<PlayerTileSelection>();
        playerAnimationsHandler = player.GetComponent<PlayerAnimationsHandler>();
        playerInventory = player.GetComponent<InventorySystem>();
        gameTimeManager = timeManager.GetComponent<GameTimeManager>();
        inputHandler = player.GetComponent<InputHandler>();
        playerStatus = player.GetComponent<PlayerStatus>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Update()
    {
    }

    public void UpdateIsInNormalMode()
    {
        if (isBuildMode || isInventoryMode)
        {
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
        UpdateIsInNormalMode();
    }

    public void SetInventoryMode(bool flag)
    {
        isInventoryMode = flag;
        UpdateIsInNormalMode();
    }

    public PlayerStatus GetPlayerStatus()
    {
        return playerStatus;
    }

    public InputHandler GetInputHandler()
    {
        return inputHandler;
    }

    public GameTimeManager GetGameTimeManager()
    {
        return gameTimeManager;
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

    public InventorySystem GetPlayerInventory()
    {
        return playerInventory;
    }

    public PlayerAnimationsHandler GetPlayerAnimationHandler()
    {
        return playerAnimationsHandler;
    }
}
