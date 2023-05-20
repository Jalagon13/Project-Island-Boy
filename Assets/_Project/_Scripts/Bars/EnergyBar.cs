using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private int _maxEnergy;
        [SerializeField] private float _coolDown;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _cdFillImage;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private TextMeshProUGUI _counter;

        private int _currentEnergy;
        private float _cdCounter;

        public int CurrentEnergy { get { return _currentEnergy; } }
        public int MaxEnergy { get { return _maxEnergy; } }
        public bool InCoolDown { get { return _cdCounter < _coolDown; } }

        private void Awake()
        {
            _currentEnergy = _maxEnergy;
            _cdFillImage.enabled = false;
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

        private void Update()
        {
            _cdCounter += Time.deltaTime;

            if (_cdCounter > _coolDown)
                _cdCounter = _coolDown;
        }

        private void UpdateCoolDownUI()
        {
            _cdFillImage.fillAmount = Mathf.Clamp01(1 - Mathf.InverseLerp(0, _coolDown, _cdCounter));
        }

        public void AddToEnergy(int value)
        {
            _currentEnergy += value;

            if (_currentEnergy > _maxEnergy)
                _currentEnergy = _maxEnergy;

            StartCoroutine(Cooldown());
            UpdateUI();
        }

        private IEnumerator Cooldown()
        {
            _cdCounter = 0f;
            _cdFillImage.enabled = true;
            while(_cdCounter < _coolDown)
            {
                yield return new WaitForSeconds(0.01f);
                UpdateCoolDownUI();
            }
            _cdFillImage.enabled = false;
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
            _counter.text = _currentEnergy.ToString();
        }
    }
}
