using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTileSelection : MonoBehaviour
{
    [Tooltip("Reference to the Tilemap used for the input selection.")]
    [SerializeField] private Tilemap playerInputTilemap;

    [Tooltip("Tile to use for highlighting.")]
    [SerializeField] private TileBase highlightTile;

    [Tooltip("Logs information.")]
    [SerializeField] private bool debugMode = true;

    [Tooltip("Minimum selectable distance from the player.")]
    [SerializeField] private float minSelectableDistance = 1f;

    [Tooltip("Maximum selectable distance from the player.")]
    [SerializeField] private float maxSelectableDistance = 5f;

    private Vector3Int selectedTile = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
    private bool hasSelectedTile = false;

    private Camera mainCamera;
    private GameManager gm;
    private GridManager gridManager;
    private PlayerMovementInputHandler inputHandler;
    private Transform playerTransform; // Reference to the player's position
    private bool isSelectionVisible = true;

    private void Start()
    {
        gm = GameManager.Instance;
        mainCamera = gm.getMainCamera().GetComponent<Camera>();
        gridManager = GridManager.Instance;
        inputHandler = gm.getPlayer().GetComponent<PlayerMovementInputHandler>();
        playerTransform = gm.getPlayer().transform; // Get the player’s Transform

        // Ensure tilemap visibility starts correctly
        playerInputTilemap.gameObject.SetActive(isSelectionVisible);
    }

    private void Update()
    {
        UpdateHoveredTile();
    }

    private void Log(string message)
    {
        if (debugMode)
        {
            Debug.Log(message);
        }
    }

    private void UpdateHoveredTile()
    {
        Vector3Int gridPos = gridManager.WorldToGrid(GetMouseWorldPosition());

        if (hasSelectedTile)
        {
            if (selectedTile == gridPos)
            {
                if (!IsTileWithinSelectionRange(selectedTile))
                {
                    Log($"[PlayerTileSelection] From selected to out of range!");
                    RestorePreviousTile();
                }
                return;
            }
            else
            {
                if (!IsTileWithinSelectionRange(gridPos))
                {
                    Log($"[PlayerTileSelection] From selected to un-selected!");
                    RestorePreviousTile();
                }
                else
                {
                    Log($"[PlayerTileSelection] New selected from selected!");
                    RestorePreviousTile();
                    HighlightTile(gridPos);
                }
            }
        }
        else
        {
            if (!IsTileWithinSelectionRange(gridPos))
            {
                Log($"[PlayerTileSelection] Selection still outside!");
                return;
            }
            else
            {
                Log($"[PlayerTileSelection] From un-selected to selected!");
                HighlightTile(gridPos);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPos = inputHandler.mouseScreenPosition;
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        int groundLayerMask = gm.getTerrainGridSystem().GetComponent<BuildingPlacer>().groundLayer;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private bool IsTileWithinSelectionRange(Vector3Int tilePos)
    {
        float cellSize = gridManager.GetComponent<Grid>().cellSize.x; // Assumes square grid

        // Get the exact world position of the tile center (fixes offset issues) /2 since you need the center
        Vector3 tileWorldPos = gridManager.GridToWorld(tilePos) + new Vector3(cellSize/2, 0, cellSize/2);

        // Use player's world position, but align Y to avoid height issues
        Vector3 playerReferencePos = playerTransform.position;
        playerReferencePos.y = tileWorldPos.y; // Match Y-values for correct 2D distance calculation

        // Compute accurate 2D distance
        float distance = Vector3.Distance(playerReferencePos, tileWorldPos);

        return distance >= minSelectableDistance && distance <= maxSelectableDistance;
    }

    private void HighlightTile(Vector3Int gridPos)
    {
        // Set the hovered tile to the highlight tile
        playerInputTilemap.SetTile(gridPos, highlightTile);
        selectedTile = gridPos;
        hasSelectedTile = true;
        Log($"[PlayerTileSelection] New selected tile: {GetHoveredTilePosition()}");
    }

    private void RestorePreviousTile()
    {
        // Clear the previously hovered tile (set it to null)
        playerInputTilemap.SetTile(selectedTile, null);
        selectedTile = new Vector3Int(int.MinValue, int.MinValue, int.MinValue); // Properly reset selection
        hasSelectedTile = false;
    }

    public Vector3Int? GetHoveredTilePosition()
    {
        return hasSelectedTile ? selectedTile : null;
    }

    // Public method to control selection visibility by enabling/disabling the tilemap
    public void SetSelectionVisibility(bool value)
    {
        if (isSelectionVisible == value) return; // No need to update if it's the same

        isSelectionVisible = value;
        playerInputTilemap.gameObject.SetActive(isSelectionVisible);
    }


}
