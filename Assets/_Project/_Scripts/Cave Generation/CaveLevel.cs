using UnityEngine;

namespace IslandBoy
{
    public class CaveLevel : MonoBehaviour
    {
        private GameObject _player;
        private Vector2 _backPoint;
        private Vector2 _entranceSpawnPoint;
        private int _index;

        public int Index { get { return _index; } }
        public Vector2 BackPoint { get { return _backPoint; } set { _backPoint = value; } }
        public Vector2 EntranceSpawnPoint { get { return _entranceSpawnPoint; } set {_entranceSpawnPoint = value; } }

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Initialize(int index)
        {
            _index = index;
        }

        public void PutPlayerAtEntrance()
        {
            _player.transform.position = _entranceSpawnPoint;
        }

        public void PutPlayerAtBackPoint()
        {
            _player.transform.position = _backPoint;
        }
    }
}
