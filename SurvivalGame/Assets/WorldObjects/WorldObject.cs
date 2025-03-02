using UnityEngine;

[DisallowMultipleComponent]
public class WorldObject<T> : MonoBehaviour where T : WorldObjectData
{
    [SerializeField] public T worldObjectData;
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
