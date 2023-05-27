using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveFloor : MonoBehaviour
    {
        [SerializeField] private Vector2 _playerSpawnPosition;

        private GameObject _player;
        private CaveLevels _caveLevels;
        private Vector2 _backPoint;

        public CaveLevels CaveLevels { get { return _caveLevels; } }
        public Vector2 BackPoint { set { _backPoint = value; } }

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _caveLevels = transform.parent.transform.GetComponent<CaveLevels>();
        }

        public void SpawnAtEntrance()
        {
            _player.transform.position = _playerSpawnPosition;
        }

        public void SpawnAtBackPoint()
        {
            _player.transform.position = _backPoint;
        }
    }
}
