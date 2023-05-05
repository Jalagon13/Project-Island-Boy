using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Player Reference", menuName = "Player Reference")]
    public class PlayerReference : ScriptableObject
    {
        private const float INTERACT_RANGE = 2f;
        private Vector2 _playerPosition;
        private Vector2 _mousePosition;

        /// <summary>
        /// Setting PlayerPositionReference does NOT change the actual position of the player.
        /// It just changes THIS local Vector2 called _playerPosition that can be referenced ONLY to get the player position.
        /// Exact same thing with MousePositionReference.
        /// Both PlayerPositionReference and MousePositionReference are set in PlayerStateMachine update function.
        /// </summary>
        public Vector2 PlayerPositionReference { get { return _playerPosition; } set { _playerPosition = value; } }
        public Vector2 MousePositionReference { get { return _mousePosition; } set { _mousePosition = value; } }

        public bool PlayerInRange(Vector3 posToCheck)
        {
            return Vector2.Distance(posToCheck, _playerPosition) < INTERACT_RANGE;
        }
    }
}
