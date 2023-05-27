using UnityEngine;

namespace IslandBoy
{
    public class CaveLevel : MonoBehaviour
    {
        [SerializeField] private GameObject _descendStairsPrefab;

        private GameObject _assetHolder;
        private GameObject _player;
        private CaveCanvas _caveCanvas;
        private Vector2 _backPoint;
        private Vector2 _entranceSpawnPoint;
        private int _floorNum;
        private bool _isQuitting;

        public Vector2 BackPoint { get { return _backPoint; } set { _backPoint = value; } }
        public Vector2 EntranceSpawnPoint { get { return _entranceSpawnPoint; } set {_entranceSpawnPoint = value; } }
        public int FloorNum { get { return _floorNum; } set { _floorNum = value; _caveCanvas.UpdateFloorText(_floorNum); } }

        private void Awake()
        {
            _assetHolder = transform.GetChild(1).transform.gameObject;
            _caveCanvas = transform.GetChild(2).GetComponent<CaveCanvas>();
            _caveCanvas.Initialize();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            _isQuitting = false;
        }

        private void OnDisable()
        {
            _isQuitting = true;
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public void TryToSpawnStairs(Vector2 spawnPos)
        {
            if (_isQuitting) return;

            GameObject descendStairs = Instantiate(_descendStairsPrefab, spawnPos, Quaternion.identity);
            descendStairs.transform.SetParent(_assetHolder.transform);
            descendStairs.GetComponent<DescendStairs>().Initialize();
        }

        public void PutPlayerAtEntrance()
        {
            _player.transform.position = _entranceSpawnPoint;
        }

        public void PutPlayerAtBackPoint()
        {
            _player.transform.position = _backPoint;
        }
    }
}
