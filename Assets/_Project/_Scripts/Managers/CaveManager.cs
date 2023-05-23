using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveManager : Singleton<CaveManager>
    {
        [SerializeField] private GameObject _caveLevelPrefab;

        private void Start()
        {
            InstantiateLevel();
        }

        public void CreateNewLevel()
        {
            InstantiateLevel();
        }

        public void InstantiateLevel()
        {
            GameObject level = Instantiate(_caveLevelPrefab, Vector3.zero, Quaternion.identity);
            level.transform.SetParent(transform);

            CaveLevel cl = level.GetComponent<CaveLevel>();
            cl.Initialize(level.transform.GetSiblingIndex());
        }
    }
}
