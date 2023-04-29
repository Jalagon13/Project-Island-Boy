using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Player Reference", menuName = "Player Reference")]
    public class PlayerReference : ScriptableObject
    {
        private const float INTERACT_RANGE = 2f;
        private Vector2 _playerPosition;

        public Vector2 PlayerPosition { get { return _playerPosition; } }

        public void UpdatePlayerPositionReference(Vector2 position)
        {
            _playerPosition = position;
        }

        public bool PlayerInRange(Vector3 posToCheck)
        {
            return Vector2.Distance(posToCheck, _playerPosition) < INTERACT_RANGE;
        }
    }
}
