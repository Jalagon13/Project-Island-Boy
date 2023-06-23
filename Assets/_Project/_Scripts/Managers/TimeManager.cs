using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class TimeManager : Singleton<TimeManager>
    {
        [SerializeField] private float _dayDurationInSec;
        [SerializeField] private TextMeshProUGUI _timeCounterText;

        private Timer _timer;

        protected override void Awake()
        {
            base.Awake();
            
            _timer = new(_dayDurationInSec);
            _timer.OnTimerEnd += PlayerPassesOut;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            _timeCounterText.text = $"Day Time Left: {Mathf.RoundToInt(_timer.RemainingSeconds)}";
        }

        private void PlayerPassesOut()
        {
            Debug.Log("Player passes out.");
        }
    }
}
