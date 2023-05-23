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
            CreateNewLevel();
        }

        public int CreateNewLevel()
        {
            GameObject level = Instantiate(_caveLevelPrefab, Vector3.zero, Quaternion.identity);
            level.transform.SetParent(transform);

            CaveLevel cl = level.GetComponent<CaveLevel>();
            var levelIndex = level.transform.GetSiblingIndex();

            cl.Initialize(levelIndex);

            return levelIndex;
        }

        public void TransitionToLevel(int index)
        {
            // play transition animations here

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            transform.GetChild(index).gameObject.SetActive(true);
        }
    }
}
