using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveFloor : MonoBehaviour
    {
        [SerializeField] private Vector2 _playerSpawnPosition;

        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            _player.transform.position = _playerSpawnPosition;
        }
    }
}
