using UnityEngine;

public enum ResourceObjectType
{
    Rock,
    Tree
}

[DisallowMultipleComponent]
public class ResourceObject : WorldObject
{
    [SerializeField] public ResourceObjectType resourceObjectType;

    private GameManager gm;
    private GridManager gridManager;

    public void Start()
    {
        gm = GameManager.Instance;
        gridManager = gm.getTerrainGridManager();
        OccupyTile();
    }

    public void OccupyTile()
    {
        Vector3Int gridPos = gridManager.WorldToGrid(this.transform.position);
        if (gridManager.CanPlaceBuilding(gridPos, 1, 1, 0)){
            gridManager.PlaceObjectOnTiles(gridPos, 1, 1, this.gameObject, 0);
        }
        else
        {
            Debug.Log("Can't place resource object since tile occupied! destroying");
            Destroy(this.gameObject);
        }
    }

    public bool IsResourceObjectOfType(ResourceObjectType type)
    {
        return resourceObjectType == type;
    }
}