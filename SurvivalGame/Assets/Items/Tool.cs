using UnityEngine;

public enum ToolType
{
    Pickaxe,
    Axe,
    Shovel
}

public class Tool : Item
{
    [SerializeField] private ToolType toolCategory;
    [SerializeField] private ResourceObjectType interactableType;

    public ToolType ToolCategory => toolCategory;
    public ResourceObjectType InteractableType => interactableType;


    public override ItemType GetItemType()
    {
        return ItemType.Tool;
    }

    public override bool PerformMainAction(GameManager gm)
    {
        Vector3Int? selectedTile = gm.getPlayerTileSelection().GetHoveredTilePosition();
        if (selectedTile != null)
        {
            GameObject targetObject = gm.getTerrainGridManager().GetObjectOnTile(selectedTile.Value);
            if (targetObject != null)
            {
                if (targetObject.GetComponent<ResourceObject>() != null)
                {
                    if (targetObject.GetComponent<ResourceObject>().IsResourceObjectOfType(interactableType))
                    {
                        Debug.Log("OK!");
                    }
                    else
                    {
                        Debug.Log("Mismatch tool and res type");
                    }
                }
                else
                {
                    Debug.Log("obj is not a resource obj");
                }
            }
            else
            {
                Debug.Log("empty tile for the tool");
            }
        }
        else
        {
            Debug.Log("Cannot perform tool main action since no tile selcted");
        }
        
        return true;
    }
}