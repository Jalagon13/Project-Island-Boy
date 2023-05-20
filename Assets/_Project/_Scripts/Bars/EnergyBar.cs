using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private int _maxEnergy;
        [SerializeField] private Image _fillImage;
        [SerializeField] private HealthBar _healthBar;

        private int _currentEnergy;

        private void Awake()
        {
            _currentEnergy = _maxEnergy;
        }

        private void Start()
        {
            UpdateUI();
        }

        private void OnEnable()
        {
            ActionControl.SwingPerformEvent += DrainEnergy;
        }

        private void OnDisable()
        {
            ActionControl.SwingPerformEvent -= DrainEnergy;
        }

        private void DrainEnergy()
        {
            _currentEnergy--;

            if (_currentEnergy <= 0)
            {
                _currentEnergy = 0;
                UpdateUI();
                StartCoroutine(_healthBar.DrainHp());
            }
            else
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxEnergy, _currentEnergy));
        }
    }
}
