using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _monster;

        public void TestSpawnMob()
        {
            Instantiate(_monster, _spawnPoint.position, Quaternion.identity);
        }
    }
}
