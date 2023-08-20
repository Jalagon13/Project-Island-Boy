using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private int _maxSpawns = 30;
        [SerializeField] private GameObject _monster;
        [SerializeField] private TilemapReferences _tmr;

        private float _baseSpawnChance = 40; // in percent
        private int _currentSpawns;
        private List<GameObject> _mobList = new();

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            TryToSpawnMob();
            StartCoroutine(Start());
        }

        public void ResetMonsters() // hooked to UndergroundGeneration unityEvent;
        {
            foreach (GameObject mob in _mobList)
            {
                Destroy(mob);
            }

            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForEndOfFrame();

            _mobList = new();
            _currentSpawns = 0;

            for (int i = 0; i < 10; i++)
            {
                SpawnMob();
            }
        }

        private void TryToSpawnMob()
        {
            if(Random.Range(0, 100f) < CalcSpawnChance() && _currentSpawns < _maxSpawns)
            {
                SpawnMob();
            }
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
            _mobList.Add(entity);
            _currentSpawns++;

            entity.AddComponent<UndergroundAsset>();
            entity.GetComponent<UndergroundAsset>().RegisterAsset(() =>
            {
                _currentSpawns--;
                _mobList.Remove(entity);
            });
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
