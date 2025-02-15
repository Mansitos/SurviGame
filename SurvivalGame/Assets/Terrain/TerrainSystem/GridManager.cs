using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{

    [Header("Grid Manager Variables")]
    public static GridManager Instance;

    [Tooltip("Ground (terrain) tilemap")]
    public Tilemap groundTilemap;
    [Tooltip("Building grid overlay tilemap")]
    public Tilemap gridOverlayTilemap;
    [Tooltip("Default grid overlay tile (not occupied)")]
    public TileBase defaultTile;
    [Tooltip("Tile to hilight tile is free to build")]
    public TileBase freeSelectedTile;
    [Tooltip("The tile to show when a selected tile is occupied by a building")]
    public TileBase occupiedTile;   
    [Tooltip("If True, occupied requiredTiles are highlighted (run-time change not supported jet).")]
    public bool showOccupiedMode = false;
    [Tooltip("If True -> debug logs.")]
    public bool verboseLogging = true;

    private Dictionary<Vector3Int, GameObject> occupiedTiles = new Dictionary<Vector3Int, GameObject>(); // Data Structure to store placed buildings
    private int deletionRadiusCheck = 6; // Should be at least >= the maximum building size that can happen
    private GameManager gm;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        gm = GameManager.Instance;
    }

    public Vector3Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(worldPosition);
        gridPos.z = 0;
        return gridPos;
    }

    public Vector3 GridToWorld(Vector3Int gridPosition)
    {
        return groundTilemap.CellToWorld(gridPosition);
    }

    public List<Vector3Int> GetPlayerOccupiedTiles()
    {
        Vector3 playerPos = gm.getPlayer().transform.position;
        Vector3Int playerOccupiedTile = WorldToGrid(playerPos);
        playerOccupiedTile.z = 0; // TODO: A sort of temp fix

        List<Vector3Int> surroundingTiles = new List<Vector3Int>();

        // Loop through a 3x3 grid centered on the player
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int tile = new Vector3Int(playerOccupiedTile.x + x, playerOccupiedTile.y + y, playerOccupiedTile.z);
                surroundingTiles.Add(tile);
            }
        }
        return surroundingTiles;
    }

    // Checks if all requiredTiles in a specified area are free for building placement, optionally including the player's current tile.
    public bool CanPlaceBuilding(Vector3Int origin, int width, int height, int rotation, bool checkAgainstPlayer = false)
    {
        List<Vector3Int> playerOccupiedTiles = GetPlayerOccupiedTiles();
        List<Vector3Int> tilesToCheck = GetBuildingRequiredTiles(origin, width, height, rotation);
        foreach (var pos in tilesToCheck)
        {
            if (occupiedTiles.ContainsKey(pos))
            {
                return false; // At least one tile is occupied by a building
            }

            if (checkAgainstPlayer)
            {
                foreach (Vector3Int playerOccupiedTile in playerOccupiedTiles) {
                    if (pos == playerOccupiedTile)
                    {
                        return false; // At least one tile is occupied by player
                    }
                }
            }
        }
        return true;
    }

    // Marks multiple requiredTiles as occupied by a building.
    public void PlaceObjectOnTiles(Vector3Int origin, int width, int height, GameObject obj, int rotation)
    {
        List<Vector3Int> tilesToOccupy = GetBuildingRequiredTiles(origin, width, height, rotation);
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

    // Removes all occupied requiredTiles within a square radius if they belong to the same object.
    public void RemoveObjectFromTiles(Vector3Int origin, bool destroyObjectOnTile=false)
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
                    Debug.Log(tilePosition);
                }
            }
        }

        // Log the number of requiredTiles being removed and their positions
        if (verboseLogging)
        {
            Debug.Log($"Removing {tilesToRemove.Count} requiredTiles: {string.Join(", ", tilesToRemove)}");
        }

        // Remove the collected requiredTiles and revert their tile if in show occupied mode.
        foreach (var tile in tilesToRemove)
        {
            occupiedTiles.Remove(tile);
            if (showOccupiedMode)
            {
                gridOverlayTilemap.SetTile(tile, defaultTile);
            }
        }

        if (destroyObjectOnTile)
        {
            Destroy(targetObject);
        }
    }

    // Gets all grid requiredTiles occupied by a building of a given size.
    // ATTENTION: This assumes a certain order: the origin is always the top left of the set of requiredTiles.
    private List<Vector3Int> GetBuildingRequiredTiles(Vector3Int origin, int width, int height, int rotation)
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

        List<Vector3Int> requiredTiles = new List<Vector3Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // The y-axis is subtracted to move downward from the origin.
                Vector3Int tile = new Vector3Int(origin.x + x + xoffset, origin.y - y + zoffset, origin.z);
                requiredTiles.Add(tile);
            }
        }
        return requiredTiles;
    }

    // Gets the object occupying a specific grid position.
    public GameObject GetObjectOnTile(Vector3Int gridPosition)
    {
        return occupiedTiles.TryGetValue(gridPosition, out GameObject obj) ? obj : null;
    }

    // Retrieves all objects within a square radius centered on a specific grid position.
    // center should be provided in grid coordinates
    public List<GameObject> GetObjectsOnSquaredArea(Vector3Int center, int radius)
    {
        List<GameObject> foundObjects = new List<GameObject>();
        center.z = 0; // For safety

        // Iterate through a square region from (x-radius, y-radius) to (x+radius, y+radius)
        for (int x = center.x - radius; x <= center.x + radius; x++)
        {
            for (int y = center.y - radius; y <= center.y + radius; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, center.z);
                GameObject obj = GetObjectOnTile(tilePosition);
                if (obj != null)
                {
                    foundObjects.Add(obj);
                }
            }
        }

        if (verboseLogging)
        {
            Debug.Log("[GridManager] Found " + foundObjects.Count + " objects on squared area...");
        }

        return foundObjects;
    }

    public List<GameObject> GetAllObjectsOnTilesByRadius(Vector3 center, float radius)
    {
        // Convert world position to grid coordinates and fix z to 0 for 2D operations
        Vector3Int centerGrid = WorldToGrid(center);
        centerGrid.z = 0;

        List<GameObject> candidates = GetObjectsOnSquaredArea(centerGrid, (int)Mathf.Ceil(radius) + 2); // +2 for safety
        List<GameObject> withinRadius = new List<GameObject>();

        foreach (var candidate in candidates)
        {
            Vector3 candidatePos = candidate.transform.position;
            Vector3Int candidateGridPos = WorldToGrid(candidatePos);  // Convert candidate position to grid coordinates

            // Calculate 2D Euclidean distance in grid space
            float distance = Vector2.Distance(new Vector2(centerGrid.x, centerGrid.y), new Vector2(candidateGridPos.x, candidateGridPos.y));

            if (distance <= radius)
            {
                withinRadius.Add(candidate);
            }
        }

        Debug.Log("[GridManager] Total objects within radius: " + withinRadius.Count);
        return withinRadius;
    }


    // Filters a list of game objects by type, returning only those which match the specified type.
    public List<GameObject> FilterGameObjectsByType(List<GameObject> gameObjects, System.Type type)
    {
        List<GameObject> filteredObjects = new List<GameObject>();
        foreach (GameObject obj in gameObjects)
        {
            if (type.IsInstanceOfType(obj.GetComponent(type)))
            {
                filteredObjects.Add(obj);
            }
        }
        return filteredObjects;
    }

    // Retrieves all objects of a certain type within a square radius centered on a specific grid position.
    public List<GameObject> GetAllObjectsOfTypeOnTilesByRadius(Vector3 center, float radius, System.Type type)
    {
        List<GameObject> objectsInRadius = GetAllObjectsOnTilesByRadius(center, radius);
        List<GameObject> objectsInRadiusOfRequiredType = FilterGameObjectsByType(objectsInRadius, type);

        return objectsInRadiusOfRequiredType;

    }

}
