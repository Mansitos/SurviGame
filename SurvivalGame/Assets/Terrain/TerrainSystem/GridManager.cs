using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Tilemap groundTilemap;

    private Dictionary<Vector3Int, GameObject> occupiedTiles = new Dictionary<Vector3Int, GameObject>();
    private int deletionRadiusCheck = 5; // Should be at least >= the maximum building size that can happen

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
    public bool CanPlaceBuilding(Vector3Int origin, int width, int height)
    {
        List<Vector3Int> occupiedTiles = GetTilesInArea(origin, width, height);
        foreach (var pos in occupiedTiles)
        {
            if (this.occupiedTiles.ContainsKey(pos))
                return false; // At least one tile is occupied
        }
        return true; // All required tiles are free
    }

    // Marks multiple tiles as occupied by a building.
    public void PlaceObjectOnTiles(Vector3Int origin, int width, int height, GameObject obj)
    {
        List<Vector3Int> occupiedTiles = GetTilesInArea(origin, width, height);
        foreach (var pos in occupiedTiles)
        {
            this.occupiedTiles[pos] = obj;
        }
    }

    // Removes the occupied tile at the origin when a building is removed.
    public void RemoveObjectFromTile(Vector3Int origin)
    {
        if (this.occupiedTiles.ContainsKey(origin))
        {
            this.occupiedTiles.Remove(origin);
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

        // Iterate through a square region from (x-radius, z-radius) to (x+radius, z+radius)
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
        Debug.Log($"Removing {tilesToRemove.Count} tiles: {string.Join(", ", tilesToRemove)}");

        // Remove the collected tiles
        foreach (var tile in tilesToRemove)
        {
            occupiedTiles.Remove(tile);
        }
    }


    // Gets all grid tiles occupied by a building of a given size.
    // ATTENTION: This assumes a certain order: the origin is always the on top left of the set of tiles.
    private List<Vector3Int> GetTilesInArea(Vector3Int origin, int width, int height)
    {
        List<Vector3Int> tiles = new List<Vector3Int>();
        if (width > 0 && height > 0)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int tile = new Vector3Int(origin.x + x, origin.y - y, origin.z);
                    tiles.Add(tile);

                    Debug.Log($"[GetTilesInArea] Added Tile: {tile}");
                }
            }
        }

        if (width < 0 && height > 0) // TODO: wrong!!
        {
            for (int x = 0; x < Mathf.Abs(width); x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int tile = new Vector3Int(origin.x - x-1, origin.y + y-1, origin.z);
                    tiles.Add(tile);

                    Debug.Log($"[GetTilesInArea] Added Tile: {tile}");
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
