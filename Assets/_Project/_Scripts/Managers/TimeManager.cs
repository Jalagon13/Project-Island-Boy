using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace IslandBoy
{
    public class TimeManager : Singleton<TimeManager>
    {
        [SerializeField] private float _dayDurationInSec;
        [SerializeField] private AudioClip _dayTransitionSound;
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _timeCounterText;
        [SerializeField] private TextMeshProUGUI _currentDayText;
        [SerializeField] private TextMeshProUGUI _dayEndText;
        [SerializeField] private RectTransform _dayEndPanel;
        [SerializeField] private RectTransform _continueButtonRt;

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
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            _timeCounterText.text = $"Day Time Left: {Mathf.RoundToInt(_timer.RemainingSeconds)}";
        }

        public void EndDay()
        {
            _timer.IsPaused = true;
            Time.timeScale = 0;

            _dayEndPanel.gameObject.SetActive(true);
            _continueButtonRt.gameObject.SetActive(true);
            _dayEndText.text = $"Day {_currentDay} Completed!";
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
            AudioManager.Instance.PlayClip(_dayTransitionSound, false, false);
            _dayEndText.text = $"Day {_currentDay}";
            yield return new WaitForSecondsRealtime(2.75f);
            Time.timeScale = 1;
            _currentDayText.text = $"Day {_currentDay}";
            _timer = new(_dayDurationInSec);
            _timer.IsPaused = false;
            _dayEndPanel.gameObject.SetActive(false);
        }

        private void PlayerPassesOut()
        {
            Debug.Log("Player passes out.");
        }
    }
}
