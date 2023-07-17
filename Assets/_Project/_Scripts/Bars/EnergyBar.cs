using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class EnergyBar : Bar
    {
        [SerializeField] private HealthBar _healthBar;

        private void OnEnable()
        {
            ActionControl.SwingPerformEvent += DrainEnergy;
            ThrowObject.ThrowEvent += DrainEnergy;
        }

        private void OnDisable()
        {
            ActionControl.SwingPerformEvent -= DrainEnergy;
            ThrowObject.ThrowEvent -= DrainEnergy;
        }

        public override void AddTo(int value)
        {
            base.AddTo(value);

            _healthBar.Draining = false;
        }

        private void DrainEnergy()
        {
            _currentValue--;

            if (_currentValue <= 0)
            {
                _currentValue = 0;
                UpdateUI();
                _healthBar.Draining = true;
                StartCoroutine(_healthBar.DrainHp());
            }
            else
            {
                UpdateUI();
            }
        }
    }
}
