using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DayManager : Singleton<DayManager>
    {
        [SerializeField] private float _dayDurationInSec;
        [Header("Editor Stuff")]
        [SerializeField] private RectTransform _marker;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Sprite _sunSprite;
        [SerializeField] private Vector2 _markerStartPosition;
        [SerializeField] private Vector2 _markerEndPosition;

        private Timer _timer;
        private Volume _globalVolume;
        private float _duration;
        private float _phasePercent;
        private bool _isDay;

        public Volume GlobalVolume { get { return _globalVolume; } }

        protected override void Awake()
        {
            base.Awake();

            _globalVolume = transform.GetChild(1).GetComponent<Volume>();
        }

        private void Start()
        {
            StartDay();
            _timer.Tick(_dayDurationInSec * 0.1f);
        }

        private void OnDisable()
        {
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
            if (_phasePercent <= 0.15f)
            {
                float percent = _phasePercent / 0.1f;
                _globalVolume.weight = Mathf.Lerp(0.5f, _isDay ? 0 : 1, percent);
            }
            else if(_phasePercent >= 0.85f && _phasePercent <= 1f)
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

        public void StartDay()
        {
            _marker.localPosition = _markerStartPosition;
            _timer = new(_dayDurationInSec);
            _timer.OnTimerEnd += EndDay;
            _duration = _dayDurationInSec;
            _isDay = true;

            PanelEnabled(false);
            UpdateMarker(_sunSprite);
        }

        public void EndDay()
        {
            PanelEnabled(true);
            StartCoroutine(TextSequence());
        }

        private IEnumerator TextSequence()
        {
            var text = _panel.GetChild(0).GetComponent<TextMeshProUGUI>();
            var button = _panel.GetChild(1).GetComponent<Button>();

            text.gameObject.SetActive(false);
            button.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);

            text.text = "Day has ended!";
            text.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            text.text = "Your stats have been replenished!";
            button.gameObject.SetActive(true);
        }

        private void PanelEnabled(bool _)
        {
            _panel.gameObject.SetActive(_);
        }

        private void UpdateMarker(Sprite sprite)
        {
            _marker.GetComponent<Image>().sprite = sprite;
        }
    }
}
