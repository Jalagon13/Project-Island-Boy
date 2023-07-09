using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class DayManager : Singleton<DayManager>
    {
        [SerializeField] private float _dayDurationInSec;
        [SerializeField] private AudioClip _dayTransitionSound;
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

        public void EndDay()
        {
            Time.timeScale = 0;

            _timer.IsPaused = true;
            _dayEndPanel.gameObject.SetActive(true);
            _continueButtonRt.gameObject.SetActive(true);
            _dayEndText.text = $"Day {_currentDay} Completed!";

            IslandManager.Instance.CheckBedsForHousing();
            IslandManager.Instance.SpawnPiles();
            //IslandManager.Instance.SpawnCrabs();
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

        private void PlayerPassesOut()
        {
            Debug.Log("Player passes out.");
        }
    }
}
