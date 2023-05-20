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
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _counter;

        private int _currentHealth;

        public int CurrentHealth { get { return _currentHealth; } }
        public int MaxHealth { get { return _maxHealth; } }

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void AddToHealth(int value)
        {
            _currentHealth += value;

            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;

            UpdateUI();
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
