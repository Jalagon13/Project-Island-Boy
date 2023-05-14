using UnityEngine;

namespace IslandBoy
{
    public enum ToolType
    {
        Ax,
        Pickaxe,
        Sword
    }

    [CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
    public class ToolObject : ItemObject
    {
        [field: SerializeField] public ToolType ToolType { get; set; }
    }
}
