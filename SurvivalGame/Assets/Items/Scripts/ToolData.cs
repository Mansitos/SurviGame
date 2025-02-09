using UnityEngine;

public enum ToolType
{
    Pickaxe,
    Axe,
    Shovel
}

[CreateAssetMenu(fileName = "NewTool", menuName = "Game/Tool")]
public class ToolData : ItemData, IUsableItem
{
    public ToolType toolCategory;
    public ResourceObjectType resourceObjectType;

    public bool PerformMainAction(GameManager gm)
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
                        Debug.Log("Mismatch tool and resource type");
                    }
                }
                else
                {
                    Debug.Log("Object is not a resource object");
                }
            }
            else
            {
                Debug.Log("Empty tile for the tool");
            }
        }
        else
        {
            Debug.Log("Cannot perform tool action since no tile selected");
        }

        return true;
    }

    private void OnValidate()
    {
        itemType = ItemType.Tool;
    }

    private void OnEnable()
    {
        itemType = ItemType.Tool;
    }
}
