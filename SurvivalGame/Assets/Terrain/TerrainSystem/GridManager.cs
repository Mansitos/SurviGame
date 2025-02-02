using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{

    [Header("Grid Manager Variables")]
    public static GridManager Instance;

    public Tilemap groundTilemap;
    public Tilemap gridOverlayTilemap;
    public TileBase defaultTile;    // The default tile (not occupied)
    public TileBase freeSelectedTile;       // The free selected tile
    public TileBase occupiedSelectedTile;   // The tile to show when a selected tile is occupied by a building
    public TileBase occupiedTile;   // The tile to show a occupied building in showOccupiedMode

    [Tooltip("If True, occupied tiles are highlighted (run-time change not supported jet).")]
    public bool showOccupiedMode = false;  // Toggle to show occupied tiles or not

    [Tooltip("If True -> debug logs.")]
    public bool verbodeLogging = true;

    // Data Structure to store placed buildings
    private Dictionary<Vector3Int, GameObject> occupiedTiles = new Dictionary<Vector3Int, GameObject>();
    private int deletionRadiusCheck = 6; // Should be at least >= the maximum building size that can happen

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector3Int WorldToGrid(Vector3 worldPosition)
    {
        return groundTilemap.WorldToCell(worldPosition);
    }

    public Vector3 GridToWorld(Vector3Int gridPosition)
    {
        return groundTilemap.CellToWorld(gridPosition);
    }

    // Checks if all tiles in a specified area are free for building placement.
    public bool CanPlaceBuilding(Vector3Int origin, int width, int height, int rotation)
    {
        List<Vector3Int> tilesToCheck = GetBuildingTilesInArea(origin, width, height, rotation);
        foreach (var pos in tilesToCheck)
        {
            if (occupiedTiles.ContainsKey(pos))
                return false; // At least one tile is occupied
        }
        return true; // All required tiles are free
    }

    // Marks multiple tiles as occupied by a building.
    public void PlaceObjectOnTiles(Vector3Int origin, int width, int height, GameObject obj, int rotation)
    {
        List<Vector3Int> tilesToOccupy = GetBuildingTilesInArea(origin, width, height, rotation);
        foreach (var pos in tilesToOccupy)
        {
            occupiedTiles[pos] = obj;

            // If in show occupied mode, change the tile to indicate occupation.
            if (showOccupiedMode)
            {
                gridOverlayTilemap.SetTile(pos, occupiedTile);
            }
        }
    }

    // Removes the occupied tile at the origin when a building is removed.
    public void RemoveObjectFromTile(Vector3Int origin)
    {
        if (occupiedTiles.ContainsKey(origin))
        {
            occupiedTiles.Remove(origin);

            // If in show occupied mode, revert the tile back to default.
            if (showOccupiedMode)
            {
                gridOverlayTilemap.SetTile(origin, defaultTile);
            }
        }
    }

    /// Removes all occupied tiles within a square radius if they belong to the same object.
    public void RemoveObjectFromTiles(Vector3Int origin)
    {
        GameObject targetObject = GetObjectOnTile(origin);
        if (targetObject == null)
        {
            return; // If the origin tile is not occupied, exit early.
        }

        List<Vector3Int> tilesToRemove = new List<Vector3Int>();

        // Iterate through a square region from (x-radius, y-radius) to (x+radius, y+radius)
        for (int x = origin.x - deletionRadiusCheck; x <= origin.x + deletionRadiusCheck; x++)
        {
            for (int y = origin.y - deletionRadiusCheck; y <= origin.y + deletionRadiusCheck; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, origin.z);

                // Use GetObjectOnTile to check occupancy
                if (GetObjectOnTile(tilePosition) == targetObject)
                {
                    tilesToRemove.Add(tilePosition);
                }
            }
        }

        // Log the number of tiles being removed and their positions
        if (verbodeLogging)
        {
            Debug.Log($"Removing {tilesToRemove.Count} tiles: {string.Join(", ", tilesToRemove)}");
        }

        // Remove the collected tiles and revert their tile if in show occupied mode.
        foreach (var tile in tilesToRemove)
        {
            occupiedTiles.Remove(tile);
            if (showOccupiedMode)
            {
                gridOverlayTilemap.SetTile(tile, defaultTile);
            }
        }
    }

    // Gets all grid tiles occupied by a building of a given size.
    // ATTENTION: This assumes a certain order: the origin is always the top left of the set of tiles.
    private List<Vector3Int> GetBuildingTilesInArea(Vector3Int origin, int width, int height, int rotation)
    {
        int xoffset = 0;
        int zoffset = 0;

        if (rotation == 90)
        {
            xoffset = - (width) +1;
        }

        if (rotation == 180)
        {
            zoffset = (height) - 1;
            xoffset = -(width) + 1;
        }

        if (rotation == 270)
        {
            zoffset = (height) - 1;
        }

        List<Vector3Int> tiles = new List<Vector3Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // The y-axis is subtracted to move downward from the origin.
                Vector3Int tile = new Vector3Int(origin.x + x + xoffset, origin.y - y + zoffset, origin.z);
                tiles.Add(tile);

                if (verbodeLogging)
                {
                    Debug.Log($"[GetBuildingTilesInArea] Added Tile: {tile}");
                }
            }
        }
        return tiles;
    }

    // Gets the object occupying a specific grid position.
    public GameObject GetObjectOnTile(Vector3Int gridPosition)
    {
        return occupiedTiles.TryGetValue(gridPosition, out GameObject obj) ? obj : null;
    }
}
