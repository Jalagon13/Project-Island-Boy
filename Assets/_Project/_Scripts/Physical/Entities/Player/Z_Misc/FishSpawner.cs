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
            var itemGo = Instantiate(_fishMobPrefab, CalcSpawnPos(), Quaternion.identity);
            FishEntity fishEntity = itemGo.GetComponent<FishEntity>();
            fishEntity.StartWander(_tmr.GroundTilemap);
        }

        private Vector2 CalcSpawnPos()
        {
            float randomX = Random.value;
            float randomY = Random.value;

            if (Random.value < 0.5f)
                randomX = randomX < 0.5f ? -0.1f : 1.1f;
            else
                randomY = randomY < 0.5f ? -0.1f : 1.1f;

            return Camera.main.ViewportToWorldPoint(new Vector2(randomX, randomY));
        }
    }
}
