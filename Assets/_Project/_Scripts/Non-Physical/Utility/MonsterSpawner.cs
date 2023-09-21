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
        [SerializeField] private int _maxMonsterCount;
        [SerializeField] private Entity _monsterPrefab;

        private int _monsterCounter;

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            OnSceneChanged(new(), SceneManager.GetActiveScene());
        }

        private void OnSceneChanged(Scene current, Scene next)
        {
            Debug.Log($"Current: {current.buildIndex} Next: {next.buildIndex}");
            if (next.buildIndex == 1)
                StartCoroutine(SpawnMonsterTimer());
            else
                StopAllCoroutines();
        }

        private IEnumerator SpawnMonsterTimer()
        {
            yield return new WaitForSeconds(Random.Range(10f, 20f));

            if(CanSpawn())
            {
                yield return SpawnMonsters();
            }

            StartCoroutine(SpawnMonsterTimer());
        }

        private bool CanSpawn()
        {
            //Debug.Log($"CurrentMon: {_monsterCounter} maxMon: {_maxMonsterCount}");
            float spawnRatio = (float)_monsterCounter / (float)_maxMonsterCount;
            var n = Random.Range(0f, 1f);
            //Debug.Log($"{n}  {spawnRatio}");
            return n > spawnRatio;
        }

        private IEnumerator SpawnMonsters()
        {
            int min = 1;
            int max = _maxMonsterCount - _monsterCounter;
            int numbMonstersToSpawn = Random.Range(min, max + 1);

            for (int i = 0; i < numbMonstersToSpawn; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));
                Spawn(MonsterToSpawn(), CalcSpawnPos()); ;
            }
        }

        private Entity MonsterToSpawn()
        {
            return _monsterPrefab;
        }

        private void Spawn(Entity monster, Vector2 pos)
        {
            Entity entity = Instantiate(monster, pos, Quaternion.identity);

            entity.gameObject.AddComponent<UndergroundAsset>();
            entity.GetComponent<UndergroundAsset>().RegisterAsset(() =>
            {
                _monsterCounter--;
            });

            _monsterCounter++;
        }

        private Vector2 CalcSpawnPos()
        {
            GraphNode startNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;

            List<GraphNode> nodes = PathUtilities.BFS(startNode, 15);
            Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];

            return singleRandomPoint;
        }
    }
}
