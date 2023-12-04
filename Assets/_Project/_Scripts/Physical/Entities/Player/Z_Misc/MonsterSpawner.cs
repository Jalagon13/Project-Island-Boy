using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private TilemapReference _wallTm;
        [SerializeField] private TilemapReference _floorTm;
        [SerializeField] private Entity _monsterPrefab;
        [SerializeField] private int _maxMonsterCount;
        [SerializeField] private float _minSpawnTimerSec;
        [SerializeField] private float _maxSpawnTimerSec;

        private void Awake()
        {
            GameSignals.DAY_START.AddListener(UnPauseMonsterSpawning);
            GameSignals.DAY_END.AddListener(PauseMonsterSpawning);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_START.RemoveListener(UnPauseMonsterSpawning);
            GameSignals.DAY_END.RemoveListener(PauseMonsterSpawning);
        }

        private void Start()
        {
            StartCoroutine(SpawnMonsterTimer());
        }

        public void MonsterSpawnDebugButton()
        {
            StartCoroutine(SpawnMonsters());
        }

        private void PauseMonsterSpawning(ISignalParameters parameters)
        {
            StopAllCoroutines();
        }

        private void UnPauseMonsterSpawning(ISignalParameters parameters)
        {
            StartCoroutine(SpawnMonsterTimer());
        }

        private IEnumerator SpawnMonsterTimer()
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnTimerSec, _maxSpawnTimerSec));

            if (true)
            {
                yield return SpawnMonsters();
            }

            StartCoroutine(SpawnMonsterTimer());
        }

        private IEnumerator SpawnMonsters()
        {
            int min = 1;
            int numbMonstersToSpawn = Random.Range(min, _maxMonsterCount);

            for (int i = 0; i < numbMonstersToSpawn; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));

                var spawn = CalcSpawnPos();

                if (_wallTm.Tilemap.HasTile(Vector3Int.FloorToInt(spawn)) || _floorTm.Tilemap.HasTile(Vector3Int.FloorToInt(spawn)))
                {
                    continue;
                }

                Spawn(MonsterToSpawn(), spawn);
            }
        }

        private Entity MonsterToSpawn()
        {
            return _monsterPrefab;
        }

        private void Spawn(Entity monster, Vector2 pos)
        {
            Instantiate(monster, pos, Quaternion.identity);
        }

        private Vector2 CalcSpawnPos()
        {
            GraphNode startNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;

            List<GraphNode> nodes = PathUtilities.BFS(startNode, 20);
            Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];

            return singleRandomPoint;
        }
    }
}
