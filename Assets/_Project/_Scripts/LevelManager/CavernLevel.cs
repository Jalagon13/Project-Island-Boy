using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class CavernLevel : MonoBehaviour, IAppendToLevel
    {
        [SerializeField] private int _levelAmount;
        [SerializeField] private GameObject _portalRoomPrefab;
        [SerializeField] private GameObject _lockedHatch;
        [SerializeField] private GameObject[] _cavePrefabs;
        [Range(0.0f, 100.0f)]
        [SerializeField] private float _rscSpawnChance;

        private int _currentLevelNum = 0;
        private int _currentLevelIndex;
        private Canvas _caveCanvas;
        private GameObject _currentLevel;
        private GameObject _rscHolder;
        private GameObject _dplyHolder;
        private GameObject _wsHolder;

        private void Awake()
        {
            _caveCanvas = transform.parent.GetChild(3).GetComponent<Canvas>();
        }

        public void StartCavernRun()
        {
            _currentLevelNum = 1;

            UpdateUI();
            Restart();
            InstantiateLevel();
            PopulateHolders();
            StartCoroutine(ApplyCaveBehaviorToRsc());

        }

        public void TransitionToNextLevel()
        {
            _currentLevelNum++;

            UpdateUI();
            Restart();
            InstantiateLevel();
            PopulateHolders();
            StartCoroutine(ApplyCaveBehaviorToRsc());
        }

        private void UpdateUI()
        {
            _caveCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Current Level: {_currentLevelNum}";
            _caveCanvas.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Level {_currentLevelNum} of The Cavern.";
        }
        private IEnumerator ApplyCaveBehaviorToRsc()
        {
            yield return new WaitForEndOfFrame();

            foreach (Transform child in _rscHolder.transform)
            {
                child.gameObject.AddComponent<CaveBehavior>();
                child.GetComponent<CaveBehavior>().Initialize(SpawnLockedHatch);
            }
        }

        public void SpawnLockedHatch(Vector3 pos)
        {
            Instantiate(_lockedHatch, pos, Quaternion.identity);
        }

        private void PopulateHolders()
        {
            _rscHolder = _currentLevel.transform.GetChild(1).gameObject;
            _dplyHolder = _currentLevel.transform.GetChild(2).gameObject;
            _wsHolder = _currentLevel.transform.GetChild(3).gameObject;
        }

        private void InstantiateLevel()
        {
            if(_currentLevelNum == _levelAmount)
            {
                GameObject portalRoom = Instantiate(_portalRoomPrefab);
                portalRoom.transform.SetParent(transform);
                _currentLevel = portalRoom;
                return;
            }

            calcIndex:
            int index = Random.Range(0, _cavePrefabs.Length);

            if (index == _currentLevelIndex)
                goto calcIndex;

            _currentLevelIndex = index;

            GameObject level = Instantiate(_cavePrefabs[index]);
            level.transform.SetParent(transform);
            _currentLevel = level;
        }

        public void Restart()
        {
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
