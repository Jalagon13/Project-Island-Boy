using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class CavernLevel : MonoBehaviour, IAppendToLevel
    {
        [SerializeField] private GameObject _caveLevelPrefab;
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _rscSpawnChance;

        private int _currentLevelIndex;
        private GameObject _currentLevel;
        private GameObject _rscHolder;
        private GameObject _dplyHolder;
        private GameObject _wsHolder;


        public void StartCavernRun()
        {
            Restart();
            InstantiateLevel();
            PopulateHolders();
        }

        public void TransitionToNextLevel()
        {

        }

        private void PopulateHolders()
        {
            _rscHolder = transform.GetChild(0).GetChild(1).gameObject;
            _dplyHolder = transform.GetChild(0).GetChild(2).gameObject;
            _wsHolder = transform.GetChild(0).GetChild(3).gameObject;
        }

        private void InstantiateLevel()
        {
            GameObject level = Instantiate(_caveLevelPrefab);
            level.transform.SetParent(transform);
            _currentLevel = level;
        }

        public void Restart()
        {
            _currentLevelIndex = 0;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            _currentLevel = null;
        }

        public void Append(GameObject obj)
        {
            switch (obj.tag)
            {
                case "RSC":
                    AppendToRSC(obj);
                    break;
                case "DPLY":
                    AppendToDPLY(obj);
                    break;
                case "WS":
                    AppendToWS(obj);
                    break;
                default:
                    Debug.LogError($"object {obj.name} could not be appended to Surface Level because TAG not found");
                    break;
            }
        }

        private void AppendToRSC(GameObject obj)
        {
            if (Random.Range(0, 100) < _rscSpawnChance)
                obj.transform.SetParent(_rscHolder.transform);
            else
                Destroy(obj);

        }

        private void AppendToDPLY(GameObject obj)
        {
            obj.transform.SetParent(_dplyHolder.transform);
        }

        private void AppendToWS(GameObject obj)
        {
            obj.transform.SetParent(_wsHolder.transform);
        }
    }
}
