using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
        private Volume _globalVolume;
        private float _duration;
        private float _phasePercent;
        private bool _isDay;

        public Volume GlobalVolume { get { return _globalVolume; } set { _globalVolume = value; } }

        protected override void Awake()
        {
            base.Awake();

            _globalVolume = transform.GetChild(1).GetComponent<Volume>();
        }

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
                MoveMarker();
                VolumeHandle();
            }
        }

        private void VolumeHandle()
        {
            if (_phasePercent <= 0.1f)
            {
                float percent = _phasePercent / 0.1f;
                _globalVolume.weight = Mathf.Lerp(0.5f, _isDay ? 0 : 1, percent);
            }
            else if(_phasePercent >= 0.9f && _phasePercent <= 1f)
            {
                float percent =  1 - ((1f - _phasePercent) * 10);
                _globalVolume.weight = Mathf.Lerp(_isDay ? 0 : 1, 0.5f, percent);
            }
        }

        private void MoveMarker()
        {
            _phasePercent = (_duration - _timer.RemainingSeconds) / _duration;
            float xValue = Mathf.Lerp(_markerStartPosition.x, _markerEndPosition.x, _phasePercent);
            _marker.anchoredPosition = new Vector2(xValue, _markerStartPosition.y);
        }

        private void StartDay()
        {
            _marker.localPosition = _markerStartPosition;
            _timer = new(_dayDurationInSec);
            _timer.OnTimerEnd += StartNight;
            _duration = _dayDurationInSec;
            _isDay = true;

            UpdateMarker(_sunSprite);
        }

        private void StartNight()
        {
            _marker.localPosition = _markerStartPosition;
            _timer = new(_nightDurationInSec);
            _timer.OnTimerEnd += StartDay;
            _duration = _nightDurationInSec;
            _isDay = false;

            UpdateMarker(_moonSprite);
        }

        private void UpdateMarker(Sprite sprite)
        {
            _marker.GetComponent<Image>().sprite = sprite;
        }
    }
}
