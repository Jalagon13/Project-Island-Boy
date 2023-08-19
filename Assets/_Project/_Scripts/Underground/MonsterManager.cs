using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GameObject _monster;
        [SerializeField] private TilemapReferences _tmr;

        private float _baseSpawnChance = 10; // in percent
        private int _maxSpawns = 5;
        private int _currentSpawns;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            TryToSpawnMob();
            StartCoroutine(Start());
        }

        private void TryToSpawnMob()
        {
            if(Random.Range(0, 100f) < CalcSpawnChance() && _currentSpawns < _maxSpawns)
            {
                Debug.Log("Spawn Mob");

                Vector3 spawnPoint;

                do
                {
                    spawnPoint = GetSpawnPosition();
                } while (!IsValidSpawnPosition(spawnPoint));

                Instantiate(_monster, spawnPoint, Quaternion.identity);
                _currentSpawns++;
            }
        }

        private bool IsValidSpawnPosition(Vector3 pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.25f);

            if (colliders.Length > 0)
                return false;

            return true;
        }

        private Vector3 GetSpawnPosition()
        {
            var grid = AstarPath.active.data.gridGraph;
            GraphNode randomNode;
            do
            {
                randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
            } while (!randomNode.Walkable);

            return (Vector3)randomNode.position;
        }

        private float CalcSpawnChance()
        {
            float spawnRatio = _currentSpawns / _maxSpawns;

            if (spawnRatio >= 0 && spawnRatio < 0.2f)
                return _baseSpawnChance + 2;
            else if (spawnRatio >= 0.2f && spawnRatio < 0.4f)
                return _baseSpawnChance + 1.5f;
            else if (spawnRatio >= 0.4f && spawnRatio < 0.6)
                return _baseSpawnChance + 1f;
            else if (spawnRatio >= 0.6f && spawnRatio < 0.8)
                return _baseSpawnChance + 0.5f;
            else if (spawnRatio >= 0.8f && spawnRatio < 1)
                return _baseSpawnChance;

            return _baseSpawnChance;
        }

        public void TestSpawnMob()
        {
            Debug.Log("Spawn Mob");

            Vector3 spawnPoint;

            do
            {
                spawnPoint = GetSpawnPosition();
            } while (!IsValidSpawnPosition(spawnPoint));

            Instantiate(_monster, spawnPoint, Quaternion.identity);
            _currentSpawns++;
        }
    }
}
