using System.Collections;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
    public class HealthBar : Bar
    {
        [SerializeField] private float _drainRate = 0.5f;

        private bool _draining;

        public bool Draining { get { return _draining; } set { _draining = value; } }

        public IEnumerator DrainHp()
        {
            yield return new WaitForSeconds(_drainRate);

            if (!_draining)
                yield break;

            _currentValue--;
            UpdateUI();
            //StartCoroutine(DrainHp());
        }
    }
}
