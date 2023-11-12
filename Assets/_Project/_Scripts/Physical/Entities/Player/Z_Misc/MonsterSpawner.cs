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
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private int _maxMonsterCount;
        [SerializeField] private Entity _monsterPrefab;

        private void Start()
        {
            StartCoroutine(SpawnMonsterTimer());
        }

        private IEnumerator SpawnMonsterTimer()
        {
            yield return new WaitForSeconds(Random.Range(15f, 25f));

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

                if (_tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(spawn)) || _tmr.FloorTilemap.HasTile(Vector3Int.FloorToInt(spawn)))
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
