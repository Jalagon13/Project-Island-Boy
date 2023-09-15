using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private GameObject _monster;
        [SerializeField] private int _maxSpawns;

        private float _baseSpawnChance = 35; // in percent
        private int _currentSpawns;

        private void OnEnable()
        {
            _currentSpawns = 0;

            StartCoroutine(SpawnMonsterTimer());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnMonsterTimer()
        {
            yield return new WaitForSeconds(1f);

            if (Random.Range(0, 100f) < CalcSpawnChance())
            {
                SpawnMob();
            }

            StartCoroutine(SpawnMonsterTimer());
        }
        private void SpawnMob()
        {
            Vector3 spawnPoint;

            do
            {
                spawnPoint = GetSpawnPosition();
            } while (!IsValidSpawnPosition(spawnPoint));

            Spawn(_monster, spawnPoint);
        }

        private void Spawn(GameObject monster, Vector3 spawnPos)
        {
            GameObject entity = Instantiate(monster, spawnPos, Quaternion.identity);

            entity.AddComponent<UndergroundAsset>();
            entity.GetComponent<UndergroundAsset>().RegisterAsset(() =>
            {
                _currentSpawns--;
            });

            _currentSpawns++;
        }

        private bool IsValidSpawnPosition(Vector3 pos)
        {
            //var posCheck = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
            //var colliders = Physics2D.OverlapCircleAll(posCheck, 0.25f);

            return !_pr.PlayerInRange(pos, 8)/* && colliders.Length <= 0*/;
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
            if (_currentSpawns >= _maxSpawns) return -1;

            float spawnRatio = _currentSpawns / _maxSpawns;
            float chanceAddOn = 3f;

            if (spawnRatio >= 0 && spawnRatio < 0.2f)
                return _baseSpawnChance + (chanceAddOn * 4);
            else if (spawnRatio >= 0.2f && spawnRatio < 0.4f)
                return _baseSpawnChance + (chanceAddOn * 3);
            else if (spawnRatio >= 0.4f && spawnRatio < 0.6)
                return _baseSpawnChance + (chanceAddOn * 2);
            else if (spawnRatio >= 0.6f && spawnRatio < 0.8)
                return _baseSpawnChance + (chanceAddOn * 1);
            else if (spawnRatio >= 0.8f && spawnRatio < 1)
                return _baseSpawnChance;

            return _baseSpawnChance;
        }
    }
}
