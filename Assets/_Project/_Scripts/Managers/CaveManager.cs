using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveManager : Singleton<CaveManager>
    {
        [SerializeField] private GameObject _caveLevelPrefab;

        public int CurrentLevelIndex { get; private set; }
        public int PreviousLevelIndex { get; private set; }

        private void Start()
        {
            CurrentLevelIndex = -1;
            CreateNewLevel();
        }

        public int CreateNewLevel()
        {
            GameObject level = Instantiate(_caveLevelPrefab, Vector3.zero, Quaternion.identity);
            level.transform.SetParent(transform);

            CaveLevel cl = level.GetComponent<CaveLevel>();
            var levelIndex = level.transform.GetSiblingIndex();

            cl.Initialize(levelIndex);

            PreviousLevelIndex = CurrentLevelIndex;
            CurrentLevelIndex = levelIndex;

            return levelIndex;
        }

        public void TransitionToLevel(int index, Vector2 spawnPos)
        {
            // play transition animations here

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            transform.GetChild(index).gameObject.SetActive(true);
            transform.GetComponent<CaveLevel>().SpawnPlayer(spawnPos);
        }
    }
}
