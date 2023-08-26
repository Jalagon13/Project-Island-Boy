using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DayNightManager : Singleton<DayNightManager>
    {
        [SerializeField] private float _dayDurationInSec;
        [SerializeField] private float _nightDurationInSec;
        [Header("Sun/Moon Marker")]
        [SerializeField] private RectTransform _marker;
        [SerializeField] private Sprite _sunSprite;
        [SerializeField] private Sprite _moonSprite;
        [SerializeField] private Vector2 _markerStartPosition;
        [SerializeField] private Vector2 _markerEndPosition;

        private Timer _timer;
        private float _duration;

        private void Start()
        {
            StartDay();
        }

        private void OnDisable()
        {
            _timer.OnTimerEnd -= StartNight;
            _timer.OnTimerEnd -= StartDay;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            //_timeCounterText.text = $"Day Time Left: {Mathf.RoundToInt(_timer.RemainingSeconds)}";

            if (!_timer.IsPaused)
            {
                float percentageComplete = (_duration - _timer.RemainingSeconds) / _duration;
                float xValue = Mathf.Lerp(_markerStartPosition.x, _markerEndPosition.x, percentageComplete);
                _marker.anchoredPosition = new Vector2(xValue, _markerStartPosition.y);
            }
        }

        private void StartDay()
        {
            _marker.localPosition = _markerStartPosition;
            _timer = new(_dayDurationInSec);
            _timer.OnTimerEnd += StartNight;
            _duration = _dayDurationInSec;

            UpdateMarker(_sunSprite);
        }

        private void StartNight()
        {
            _marker.localPosition = _markerStartPosition;
            _timer = new(_nightDurationInSec);
            _timer.OnTimerEnd += StartDay;
            _duration = _nightDurationInSec;

            UpdateMarker(_moonSprite);
        }

        private void UpdateMarker(Sprite sprite)
        {
            _marker.GetComponent<Image>().sprite = sprite;
        }
    }
}
