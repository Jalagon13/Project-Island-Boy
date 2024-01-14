using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Sapling : MonoBehaviour
    {
        [SerializeField] private GameObject _treePrefab;

        private void Awake()
        {
            GameSignals.DAY_START.AddListener(SpawnPrefab);
        }

        private void OnDestroy()
        {
            GameSignals.DAY_START.AddListener(SpawnPrefab);
        }

        private void SpawnPrefab(ISignalParameters parameters)
        {
            if (_treePrefab == null) return;

            Instantiate(_treePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
