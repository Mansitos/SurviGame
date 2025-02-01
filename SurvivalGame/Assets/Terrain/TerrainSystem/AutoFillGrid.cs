using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoFillGrid : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase gridTile;
    public int dimToFill;

    private void Start()
    {
        FillGrid();
    }

    private void FillGrid()
    {
        for (int x = -dimToFill; x < dimToFill; x++)
        {
            for (int y = -dimToFill; y < dimToFill; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePos, gridTile);
            }
        }
    }
}
