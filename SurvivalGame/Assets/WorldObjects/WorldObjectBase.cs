using UnityEngine;

[DisallowMultipleComponent]
public class WorldObjectBase : MonoBehaviour
{
    public virtual WorldObjectData GetWorldObjectData() => null;
}