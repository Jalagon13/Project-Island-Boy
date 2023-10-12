using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Player Reference", menuName = "New Reference/Player Reference")]
    public class PlayerReference : ScriptableObject
    {
        private const float INTERACT_RANGE = 2.25f;
        private int _defense; // set to 0 in PlayerEntity
        private Vector2 _spawnPosition;
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
        public Vector2 SpawnPosition { get { return _spawnPosition; } }

        // This is only SET in the Inventory Script do NOT SET this anywhere else. Only get a refernce for it.
        public Inventory Inventory { get { return _playerInventory; } set { _playerInventory = value; } }

        // This is only SET in HotbarControl Script do NOT SET THIS anywhere else. Only get a reference for it.
        public InventorySlot SelectedSlot { get { return _selectedSlot; } set { _selectedSlot = value; } }

        public int Defense { get { return _defense; } set { _defense = value; } }

        public bool PlayerInRange(Vector3 posToCheck)
        {
            return Vector2.Distance(posToCheck, _playerPosition) < INTERACT_RANGE;
        }

        public bool PlayerInRange(Vector3 posToCheck, float customDistance)
        {
            return Vector2.Distance(posToCheck, _playerPosition) < customDistance;
        }

        public void SetSpawnPosition(Vector2 pos)
        {
            _spawnPosition = pos;
        }

        public void AddDefense(int val)
        {
            _defense += val;

            if(_defense < 0)
                _defense = 0;
        }
    }
}
