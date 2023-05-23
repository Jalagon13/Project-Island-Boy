using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveLevel : MonoBehaviour
    {
        private int _index;
        private Vector2 _spawnPosition;
        private Vector2 _entranceSpawnPosition;
        private GameObject _player;

        public int Index { get { return _index; } }
        public Vector2 SpawnPosition { get { return _spawnPosition; } set { _spawnPosition = value; } }
        public Vector2 EntranceSpawnPosition { get { return _entranceSpawnPosition; } set { _entranceSpawnPosition = value; } }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Initialize(int index)
        {
            _index = index;
        }

        public void SpawnPlayer(Vector2 pos)
        {
            _player.transform.position = pos;
        }
    }
}
