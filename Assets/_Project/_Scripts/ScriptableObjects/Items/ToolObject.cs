using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
    public class ToolObject : ItemObject
    {
        [Header("Tool Parameters")]
        [SerializeField] private int _maxDurability;
        [SerializeField] private float _coolDown = 0.8f;

        private int _durabilityReference;

        public float MaxDurability { get { return _maxDurability; } }
        public float CoolDown { get { return _coolDown; } }

        /// This is not a set in stone Durability for all Tools of this type but like a 
        /// durability temp variable to store this information in the inventory slot.
        public int DurabilityReference { get { return _durabilityReference; } 
            set 
            {
                _durabilityReference = value > _maxDurability ? _maxDurability : value; 
            } 
        }
    }
}
