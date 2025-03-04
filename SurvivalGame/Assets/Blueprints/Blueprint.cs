using System.Collections.Generic;
using UnityEngine;

public abstract class Blueprint : ScriptableObject
{
    [System.Serializable]
    public struct ItemRequirement
    {
        public ItemData item;
        public int quantity;
    }

    public List<ItemRequirement> requirements;
}
