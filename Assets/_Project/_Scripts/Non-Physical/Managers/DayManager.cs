using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DayManager : Singleton<DayManager>
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private float _dayDurationInSec;
        [Range(0, 1f)]
        [SerializeField] private float _percentTillCanSleep;
        [Range(0, 1f)]
        [SerializeField] private float _debugDayPercentage;
        [Header("Editor Stuff")]
        [SerializeField] private RectTransform _marker;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Sprite _sunSprite;
        [SerializeField] private Vector2 _markerStartPosition;
        [SerializeField] private Vector2 _markerEndPosition;

        private event EventHandler _onStartDay;
        private event EventHandler _onEndDay;
        private Timer _timer;
        private Volume _globalVolume;
        private float _duration;
        private float _phasePercent;
        private bool _isDay, _hasDisplayedWarning;
        private List<string> _endDaySlides = new();

        public Volume GlobalVolume { get { return _globalVolume; } }
        public EventHandler OnStartDay { get { return _onStartDay; } set { _onStartDay = value; } }
        public EventHandler OnEndDay { get { return _onEndDay; } set { _onEndDay = value; } }
        public Timer DayTimer { get { return _timer; } }

        protected override void Awake()
        {
            base.Awake();
            _timer = new(_dayDurationInSec);
            _globalVolume = transform.GetChild(1).GetComponent<Volume>();
        }

        private void Start()
        {
            StartDay();
            _timer.Tick(_dayDurationInSec * _debugDayPercentage); // starts the day some percent way through
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);

            if (!_timer.IsPaused)
            {
                MoveMarker();
                VolumeHandle();
                if(_phasePercent > _percentTillCanSleep && !_hasDisplayedWarning)
                {
                    PopupMessage.Create(_pr.Position, "I need to sleep soon..", Color.cyan, new(0f, 0.75f), 1.5f);
                    _hasDisplayedWarning = true;
                }
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
            ResetDay();
            PanelEnabled(false);
            UpdateMarker(_sunSprite);
        }

        private void ResetDay()
        {
            _marker.localPosition = _markerStartPosition;
            _timer.RemainingSeconds = _dayDurationInSec;
            _duration = _dayDurationInSec;
            _hasDisplayedWarning = false;
            _timer.IsPaused = false;
            _isDay = true;
            _onStartDay?.Invoke(this, EventArgs.Empty);
        }

        public bool CanSleep()
        {
            return _phasePercent > _percentTillCanSleep;
        }

        public void AddEndDaySlide(string text)
        {
            _endDaySlides.Add(text);
        }

        public void ClearEndDaySlides()
        {
            _endDaySlides.Clear();
        }

        [ContextMenu("End Day")]
        public void EndDay() // connected to bed
        {
            _onEndDay?.Invoke(this, EventArgs.Empty);
            _timer.IsPaused = true;

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

            foreach (string slide in _endDaySlides)
            {
                text.text = slide;
                yield return new WaitForSeconds(2f);
            }

            text.text = "Your stats have been replenished!";
            button.gameObject.SetActive(true);

            _endDaySlides.Clear();
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
