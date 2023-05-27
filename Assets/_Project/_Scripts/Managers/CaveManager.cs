using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class CaveManager : Singleton<CaveManager>
    {
        [SerializeField] private GameObject _caveLevelPrefab;

        private int _activeIndex = -1;

        public int ActiveIndex { get { return _activeIndex; } }
        public int CurrentLevelIndex { get; private set; }
        public int PreviousLevelIndex { get; private set; }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        private void Start()
        {
            CurrentLevelIndex = -1;
            CreateNewLevel();
            TransitionToLevel(CurrentLevelIndex, true);
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(scene.buildIndex);
        }

        public int CreateNewLevel()
        {
            GameObject level = Instantiate(_caveLevelPrefab, Vector3.zero, Quaternion.identity);
            level.transform.SetParent(transform);

            var newLevelIndex = level.transform.GetSiblingIndex();

            PreviousLevelIndex = CurrentLevelIndex;
            CurrentLevelIndex = newLevelIndex;

            CaveLevel cl = level.GetComponent<CaveLevel>();
            cl.FloorNum = PreviousLevelIndex == -1 ? 1 : transform.GetChild(_activeIndex).GetComponent<CaveLevel>().FloorNum + 1;

            level.GetComponent<CaveLevelGenerator>().Generate();

            return newLevelIndex;
        }

        public void TransitionToLevel(int index, bool isDescending)
        {
            // play transition animations here
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            transform.GetChild(index).gameObject.SetActive(true);
            _activeIndex = index;

            if (isDescending)
                transform.GetChild(index).GetComponent<CaveLevel>().PutPlayerAtEntrance();
            else
                transform.GetChild(index).GetComponent<CaveLevel>().PutPlayerAtBackPoint();
        }

        public void GoToSurface()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            //LevelManager.Instance.LoadScene("Surface");
        }
    }
}
