using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Player Reference", menuName = "Player Reference")]
    public class PlayerReference : ScriptableObject
    {
        private const float INTERACT_RANGE = 2.25f;
        private Vector2 _playerPosition;
        private Vector2 _mousePosition;
        private Inventory _playerInventory;
        private InventorySlot _selectedSlot;

        /// <summary>
        /// Setting PlayerPositionReference does NOT change the actual position of the player.
        /// It just changes THIS local Vector2 called _playerPosition that can be referenced ONLY to get the player position.
        /// Exact same thing with MousePositionReference.
        /// Both PlayerPositionReference and MousePositionReference are set in PlayerStateMachine update function.
        /// </summary>
        public Vector2 Position { get { return _playerPosition; } set { _playerPosition = value; } }
        public Vector2 MousePosition { get { return _mousePosition; } set { _mousePosition = value; } }

        // This is only SET in the Inventory Script do NOT SET this anywhere else. Only get a refernce for it.
        public Inventory Inventory { get { return _playerInventory; } set { _playerInventory = value; } }

        // This is only SET in HotbarControl Script do NOT SET THIS anywhere else. Only get a reference for it.
        public InventorySlot SelectedSlot { get { return _selectedSlot; } set { _selectedSlot = value; } }

        public bool PlayerInRange(Vector3 posToCheck)
        {
            return Vector2.Distance(posToCheck, _playerPosition) < INTERACT_RANGE;
        }
    }
}
