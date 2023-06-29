using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _ghostMobPrefab;
        [SerializeField] private float _spawnRangeMax;
        [SerializeField] private float _spawnRangeMin;
        [SerializeField] private float _spawnTimerMax;
        [SerializeField] private float _spawnTimerMin;

        private void OnEnable()
        {
            StartCoroutine(SpawnMonster());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnMonster()
        {
            yield return new WaitForSeconds(Random.Range(_spawnTimerMin, _spawnTimerMax));
            CheckIfCanSpawn();
            StartCoroutine(SpawnMonster());
        }

        private void CheckIfCanSpawn()
        {
            var spawnPos = CalcSpawnPos();
            Camera cam = Camera.main;
            Vector3 viewport = cam.WorldToViewportPoint(spawnPos);
            bool inCamFrustum = Is01(viewport.x) && Is01(viewport.y);

            if (inCamFrustum) return;

            SpawnMob(spawnPos);
        }

        private void SpawnMob(Vector2 spawnPos)
        {
            Instantiate(_ghostMobPrefab, spawnPos, Quaternion.identity);
        }

        private bool Is01(float a)
        {
            return a > 0 && a < 1;
        }

        private Vector2 CalcSpawnPos()
        {
            var pos = _pr.Position + (Random.insideUnitCircle * _spawnRangeMax);
            var posPlayerVector = _pr.Position - pos;

            if (posPlayerVector.magnitude < _spawnRangeMin)
            {
                return _pr.Position + (posPlayerVector.normalized * (_spawnRangeMin * 1.5f));
            }

            return pos;
        }
    }
}
