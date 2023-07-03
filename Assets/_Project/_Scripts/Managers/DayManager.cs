using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class DayManager : Singleton<DayManager>
    {
        [SerializeField] private float _dayDurationInSec;
        [SerializeField] private AudioClip _dayTransitionSound;
        [SerializeField] private GameObject _minerNpcPrefab;
        [SerializeField] private GameObject _crabMobPrefab;
        [SerializeField] private TileBase _sandTile;
        [SerializeField] private Tilemap _island;
        [SerializeField] private Tilemap _floor;
        [SerializeField] private Tilemap _walls;
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _timeCounterText;
        [SerializeField] private TextMeshProUGUI _currentDayText;
        [SerializeField] private TextMeshProUGUI _dayEndText;
        [SerializeField] private RectTransform _dayEndPanel;
        [SerializeField] private RectTransform _continueButtonRt;
        [Header("Sun Marker")]
        [SerializeField] private RectTransform _sunMarker;
        [SerializeField] private Vector2 _sunMarkerStartPosition;
        [SerializeField] private Vector2 _sunMarkerEndPosition;

        private Timer _timer;
        private List<Vector2> _bedPositions = new();
        private List<GameObject> _crabMobs = new();
        private int _currentDay = 1;

        protected override void Awake()
        {
            base.Awake();
            
            _timer = new(_dayDurationInSec);
            _timer.OnTimerEnd += PlayerPassesOut;
        }

        private void Start()
        {
            _dayEndPanel.gameObject.SetActive(false);
            _currentDayText.text = $"Day {_currentDay}";
            _sunMarker.localPosition = _sunMarkerStartPosition;

            //SpawnCrabs();
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            //_timeCounterText.text = $"Day Time Left: {Mathf.RoundToInt(_timer.RemainingSeconds)}";

            if (!_timer.IsPaused)
            {
                float percentageComplete = (_dayDurationInSec - _timer.RemainingSeconds) / _dayDurationInSec;
                float xValue = Mathf.Lerp(_sunMarkerStartPosition.x, _sunMarkerEndPosition.x, percentageComplete);
                _sunMarker.anchoredPosition = new Vector2(xValue, _sunMarkerStartPosition.y);
            }
        }

        public void PushBedPosition(Vector2 pos)
        {
            if(!_bedPositions.Contains(pos))
                _bedPositions.Add(pos);
        }

        public void PopBedPosition(Vector2 pos)
        {
            if (_bedPositions.Contains(pos))
                _bedPositions.Remove(pos);
        }

        public void EndDay()
        {
            Time.timeScale = 0;

            _timer.IsPaused = true;
            _dayEndPanel.gameObject.SetActive(true);
            _continueButtonRt.gameObject.SetActive(true);
            _dayEndText.text = $"Day {_currentDay} Completed!";

            foreach (Vector2 pos in _bedPositions)
            {
                CheckHousing(pos);
            }

            //SpawnCrabs();
        }

        private void SpawnCrabs()
        {
            foreach (GameObject crab in _crabMobs)
            {
                Destroy(crab);
            }

            _crabMobs.Clear();

            int crabCounter = 0;
            int crabMax = Random.Range(1, 2);

            while(crabCounter < crabMax)
            {
                var xVal = Random.Range(_island.cellBounds.xMin, _island.cellBounds.xMax);
                var yVal = Random.Range(_island.cellBounds.yMin, _island.cellBounds.yMax);
                var pos = Vector3Int.FloorToInt(new Vector2(xVal, yVal));

                if (_island.HasTile(pos) && _island.GetTile(pos) == _sandTile)
                {
                    var crab = Instantiate(_crabMobPrefab, pos, Quaternion.identity);
                    _crabMobs.Add(crab);
                    crabCounter++;
                }
            }
        }

        public void ContinueButton()
        {
            StartCoroutine(ContinueSequence());
        }

        private IEnumerator ContinueSequence()
        {
            _dayEndText.text = $"Day {_currentDay}";

            yield return new WaitForSecondsRealtime(1.5f);

            _currentDay++;
            _dayEndText.text = $"Day {_currentDay}";

            AudioManager.Instance.PlayClip(_dayTransitionSound, false, false);

            yield return new WaitForSecondsRealtime(2.75f);

            Time.timeScale = 1;

            _currentDayText.text = $"Day {_currentDay}";
            _timer = new(_dayDurationInSec);
            _timer.IsPaused = false;
            _dayEndPanel.gameObject.SetActive(false);
            _sunMarker.localPosition = _sunMarkerStartPosition;
        }

        private void CheckHousing(Vector2 startPos)
        {
            Stack<Vector3Int> tiles = new();
            List<Vector3Int> validPositions = new();
            tiles.Push(Vector3Int.FloorToInt(startPos));

            while (tiles.Count > 0)
            {
                var p = Vector3Int.FloorToInt(tiles.Pop());
                if (_floor.HasTile(p))
                {
                    if (_walls.HasTile(p) || HasDoor(p))
                    {
                        continue;
                    }
                    else if (!validPositions.Contains(p))
                    {
                        validPositions.Add(p);
                        tiles.Push(new Vector3Int(p.x - 1, p.y));
                        tiles.Push(new Vector3Int(p.x + 1, p.y));
                        tiles.Push(new Vector3Int(p.x, p.y - 1));
                        tiles.Push(new Vector3Int(p.x, p.y + 1));
                    }
                }
                else if (_walls.HasTile(p) || HasDoor(p))
                {
                    continue;
                }
                else
                {
                    Debug.Log($"There is an empty space at {p} therefore {startPos} is not a house");
                    return;
                }
            }

            Debug.Log($"{startPos} is valid housing! # of spaces: {validPositions.Count}");
            Instantiate(_minerNpcPrefab, validPositions[2], Quaternion.identity);
            return;
        }

        private bool HasDoor(Vector3Int pos)
        {
            var centerPos = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
            var colliders = Physics2D.OverlapCircleAll(centerPos, 0.1f);

            foreach (Collider2D col in colliders)
            {
                if (col.TryGetComponent(out Door door))
                    return true;
            }

            return false;
        }

        private void PlayerPassesOut()
        {
            Debug.Log("Player passes out.");
        }
    }
}
