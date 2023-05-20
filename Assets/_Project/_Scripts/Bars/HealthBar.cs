using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private Image _fillImage;

        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
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
        }
    }
}
