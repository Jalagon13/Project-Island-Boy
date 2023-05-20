using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _coolDown;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _cdFillImage;
        [SerializeField] private TextMeshProUGUI _counter;

        private int _currentHealth;
        private float _cdCounter;

        public int CurrentHealth { get { return _currentHealth; } }
        public int MaxHealth { get { return _maxHealth; } }
        public bool InCoolDown { get { return _cdCounter < _coolDown; } }

        private void Awake()
        {
            _currentHealth = 1;
            _cdFillImage.enabled = false;
        }

        private void Start()
        {
            UpdateUI();
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

        public void AddToHealth(int value)
        {
            _currentHealth += value;

            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;

            StartCoroutine(Cooldown());
            UpdateUI();
        }

        private IEnumerator Cooldown()
        {
            _cdCounter = 0f;
            _cdFillImage.enabled = true;
            while (_cdCounter < _coolDown)
            {
                yield return new WaitForSeconds(0.01f);
                UpdateCoolDownUI();
            }
            _cdFillImage.enabled = false;
        }

        public IEnumerator DrainHp()
        {
            yield return new WaitForSeconds(0.5f);
            _currentHealth--;
            UpdateUI();
            StartCoroutine(DrainHp());
        }

        private void UpdateUI()
        {
            _fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, _maxHealth, _currentHealth));
            _counter.text = _currentHealth.ToString();
        }
    }
}
