using Mono.Cecil;
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
    [SerializeField] private ResourceObjectType resourceObjectType;

    public ToolType ToolCategory => toolCategory;
    public ResourceObjectType ResourceObjectType => resourceObjectType;


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
                ResourceObject targetResource = targetObject.GetComponent<ResourceObject>();
                if (targetResource != null)
                {
                    if (targetResource.IsResourceObjectOfType(resourceObjectType))
                    {
                        // Start collect process 
                        PlayerMovement player = gm.getPlayer().GetComponent<PlayerMovement>();
                        player.StartCollectingResource(targetObject, targetResource.GetCollectionDuration(), resourceObjectType);
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