using UnityEngine;


public interface IInteractableWO
{
    bool InteractWithWorldObject();
}

[DisallowMultipleComponent]
public class WorldObject<T> : WorldObjectBase where T : WorldObjectData
{
    [SerializeField] public T worldObjectData;
    protected GameManager gm;
    protected GridManager gridManager;

    public override WorldObjectData GetWorldObjectData() => worldObjectData;

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
