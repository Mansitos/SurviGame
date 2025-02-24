using UnityEngine;

[DisallowMultipleComponent]
public class WorldObject : MonoBehaviour
{
    public string objectName = "Default WorldObject Name";
    protected GameManager gm;
    protected GridManager gridManager;

    public System.Type GetWorldObjectType()
    {
        return this.GetType();
    }

    protected virtual void Start()
    {
        gm = GameManager.Instance;
        gridManager = gm.GetTerrainGridManager();
    }

}
