using UnityEngine;

namespace IslandBoy
{
    public class CaveLevels : MonoBehaviour, IAppendToLevel
    {
        private int _currentLevelIndex = 0;
        private GameObject _rscHolder;
        private GameObject _dplyHolder;
        private GameObject _wsHolder;

        private void OnEnable()
        {
            DisableAllLevels();

            _currentLevelIndex = 0;
            Transform level = transform.GetChild(_currentLevelIndex);
            level.gameObject.SetActive(true);
            level.GetComponent<CaveFloor>().SpawnAtEntrance();
        }

        public void Descend()
        {
            DisableAllLevels();
            _currentLevelIndex++;

            Transform level = transform.GetChild(_currentLevelIndex);

            if (level == null)
            {
                _currentLevelIndex--;
                return;
            }

            level.gameObject.SetActive(true);
            level.GetComponent<CaveFloor>().SpawnAtEntrance();
        }

        public void Ascend()
        {
            DisableAllLevels();
            _currentLevelIndex--;

            if (_currentLevelIndex < 0)
            {
                LevelManager.Instance.TransitionToSurfaceLevel();
                return;
            }

            Transform level = transform.GetChild(_currentLevelIndex);
            level.gameObject.SetActive(true);
            level.GetComponent<CaveFloor>().SpawnAtBackPoint();
        }

        private void DisableAllLevels()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void Append(GameObject obj)
        {
            SetHolders();

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
            obj.transform.SetParent(_rscHolder.transform);
        }

        private void AppendToDPLY(GameObject obj)
        {
            obj.transform.SetParent(_dplyHolder.transform);
        }

        private void AppendToWS(GameObject obj)
        {
            obj.transform.SetParent(_wsHolder.transform);
        }

        private void SetHolders()
        {
            Transform level = transform.GetChild(_currentLevelIndex);
            _rscHolder = level.GetChild(1).gameObject;
            _dplyHolder = level.GetChild(2).gameObject;
            _wsHolder = level.GetChild(3).gameObject;
        }
    }
}
