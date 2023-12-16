using System;
using UnityEngine;

namespace IslandBoy
{
    public class Timer
    {
        public event Action OnTimerEnd;
        public bool IsPaused = false;

        private float _remainingSeconds;

        public float RemainingSeconds
        {
            get { return _remainingSeconds; }
            set
            {
                value = Mathf.Max(value, 0f);
                _remainingSeconds = value;
            }
        }

        public Timer(float duration)
        {
            _remainingSeconds = duration;
        }

        public void Tick(float deltaTime)
        {
            if (_remainingSeconds == 0f || IsPaused) return;

            _remainingSeconds -= deltaTime;

            CheckForTimerEnd();
        }

        private void CheckForTimerEnd()
        {
            if (_remainingSeconds > 0f) return;

            _remainingSeconds = 0f;

            OnTimerEnd?.Invoke();
        }
    }
}
