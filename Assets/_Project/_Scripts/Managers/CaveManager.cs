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
            TransitionToLevel(CurrentLevelIndex, true);
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
            level.GetComponent<CaveLevelGenerator>().Generate();

            return levelIndex;
        }

        public void TransitionToLevel(int index, bool isDescending)
        {
            // play transition animations here
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            transform.GetChild(index).gameObject.SetActive(true);

            if (isDescending)
                transform.GetChild(index).GetComponent<CaveLevel>().PutPlayerAtEntrance();
            else
                transform.GetChild(index).GetComponent<CaveLevel>().PutPlayerAtBackPoint();
        }
    }
}
