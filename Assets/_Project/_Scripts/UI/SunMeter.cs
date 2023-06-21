using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class SunMeter : MonoBehaviour
    {
        [SerializeField] private float _dayDurationInSec;
        [SerializeField] private TextMeshProUGUI _timeCounterText;

        private Timer _timer;

        private void Awake()
        {
            _timer = new(_dayDurationInSec);

            _timer.OnTimerEnd += EndOfDay;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            _timeCounterText.text = $"Day Time Left: {Mathf.RoundToInt(_timer.RemainingSeconds)}";
        }

        private void EndOfDay()
        {
            Debug.Log("End of day callback");
        }
    }
}
