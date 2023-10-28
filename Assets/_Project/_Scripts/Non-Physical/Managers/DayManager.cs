using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace IslandBoy
{
    public class DayManager : MonoBehaviour
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

        private Timer _timer;
        private Volume _globalVolume;
        private float _duration;
        private float _phasePercent;
        private bool _isDay, _hasDisplayedWarning;
        private List<string> _endDaySlides = new();

        private void Awake()
        {
            _globalVolume = FindObjectOfType<Volume>();
            _timer = new(_dayDurationInSec);
            _timer.OnTimerEnd += OutOfTime;
        }

        private void Start()
        {
            StartDay();
            _timer.Tick(_dayDurationInSec * _debugDayPercentage); // starts the day some percent way through
        }

        private void OnEnable()
        {
            GameSignals.DAY_END.AddListener(EndDay);
            GameSignals.NPC_MOVED_IN.AddListener(NpcMovedIn);
            GameSignals.NPC_MOVED_OUT.AddListener(NpcMovedOut);
        }

        private void OnDisable()
        {
            GameSignals.DAY_END.RemoveListener(EndDay);
            GameSignals.NPC_MOVED_IN.RemoveListener(NpcMovedIn);
            GameSignals.NPC_MOVED_OUT.RemoveListener(NpcMovedOut);
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
                    GameSignals.CAN_SLEEP.Dispatch();
                    _hasDisplayedWarning = true;
                }
            }
        }

        private void OutOfTime()
        {
            GameSignals.DAY_OUT_OF_TIME.Dispatch();
        }

        private void NpcMovedIn(ISignalParameters parameters)
        {
            NpcObject npc = parameters.GetParameter("MovedInNpc") as NpcObject;

            _endDaySlides.Add($"{npc.Name} has moved in!");
        }

        private void NpcMovedOut(ISignalParameters parameters)
        {
            NpcObject npc = parameters.GetParameter("MovedOutNpc") as NpcObject;

            _endDaySlides.Add($"{npc.Name} has moved out!");
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
            DispatchStartDay();
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
        }

        private void DispatchStartDay()
        {
            // implement optional parameters before dispatch here
            GameSignals.DAY_START.Dispatch();
        }

        public bool CanSleep()
        {
            return _phasePercent > _percentTillCanSleep;
        }

        [ContextMenu("End Day")]
        public void EndDay(ISignalParameters parameters) // connected to bed
        {
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
