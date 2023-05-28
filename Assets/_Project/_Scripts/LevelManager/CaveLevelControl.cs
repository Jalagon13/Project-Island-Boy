using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveLevelControl : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        public void SpawnPlayer()
        {
            _player.transform.position = _spawnPoint.position;
        }
    }
}
