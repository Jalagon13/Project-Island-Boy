using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class FishSpawner : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private GameObject _fishMobPrefab;
        [SerializeField] private bool _spawnFish;

        private void OnEnable()
        {
            for (int i = 0; i < 6; i++)
            {
                SpawnFish();
            }
        }

        private IEnumerator Start()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0 || !_spawnFish)
                yield break;

            yield return new WaitForSeconds(Random.Range(1f, 5f));

            SpawnFish();

            StartCoroutine(Start());
        }

        private void SpawnFish()
        {
            var pos = Vector3Int.FloorToInt(CalcSpawnPos());

            if (_tmr.GroundTilemap.HasTile(pos))
                return;

            var itemGo = Instantiate(_fishMobPrefab, pos, Quaternion.identity);
            FishEntity fishEntity = itemGo.GetComponent<FishEntity>();
            fishEntity.StartWander(_tmr.GroundTilemap);
        }

        private Vector2 CalcSpawnPos()
        {
            Vector2 direction = ((Vector2)transform.position + Random.insideUnitCircle).normalized;
            Vector2 spawnPos = direction * 25;

            return spawnPos;
        }
    }
}
