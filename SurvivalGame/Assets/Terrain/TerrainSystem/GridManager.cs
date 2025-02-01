using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Tilemap groundTilemap;

    private Dictionary<Vector3Int, GameObject> occupiedTiles = new Dictionary<Vector3Int, GameObject>();

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

    public bool IsTileOccupied(Vector3Int gridPosition)
    {
        return occupiedTiles.ContainsKey(gridPosition);
    }

    public bool CanPlaceBuilding(Vector3Int gridPosition)
    {
        return !IsTileOccupied(gridPosition);
    }

    public void PlaceObjectOnTile(Vector3Int gridPosition, GameObject obj)
    {
        if (!IsTileOccupied(gridPosition))
        {
            occupiedTiles[gridPosition] = obj;
        }
    }

    public void RemoveObjectFromTile(Vector3Int gridPosition)
    {
        if (occupiedTiles.ContainsKey(gridPosition))
        {
            occupiedTiles.Remove(gridPosition);
        }
    }

    public GameObject GetObjectOnTile(Vector3Int gridPosition)
    {
        return occupiedTiles.TryGetValue(gridPosition, out GameObject obj) ? obj : null;
    }
}
