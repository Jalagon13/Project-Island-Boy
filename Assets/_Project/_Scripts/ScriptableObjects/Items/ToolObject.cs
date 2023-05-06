using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
    public class ToolObject : ItemObject
    {
        [Header("Tool Parameters")]
        [SerializeField] private int _maxDurability;
        [SerializeField] private float _coolDown = 0.8f;

        public float MaxDurability { get { return _maxDurability; } }
        public float CoolDown { get { return _coolDown; } }
    }
}
