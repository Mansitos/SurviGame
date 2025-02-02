using UnityEngine;

[DisallowMultipleComponent]
public class WorldObject : MonoBehaviour
{
    public string objectName = "Default WorldObject Name";

    public System.Type GetWorldObjectType()
    {
        return this.GetType();
    }

}
