using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class EntityHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _hpBackground;
        [SerializeField] private Image _hpFill;

        private HealthSystem _healthSystem;

        private void OnDisable()
        {
            _healthSystem.OnHealthChanged -= HandleHealthChanged;
        }

        private void Start()
        {
            //BarEnabled(_healthSystem.IsFullHP() ? false : true);
        }

        private void HandleHealthChanged(object sender, HealthChangedEventArgs e)
        {
            UpdateHealthBar(e.Health, e.MaxHealth);
        }

        public void SetUp(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _healthSystem.OnHealthChanged += HandleHealthChanged;
            UpdateHealthBar(healthSystem.Health, healthSystem.HealthMax);
        }

        public void UpdateHealthBar(int health, int maxHealth)
        {
            //BarEnabled(_healthSystem.IsFullHP() ? false : true);

            _hpFill.fillAmount = Mathf.Clamp01(
                Mathf.InverseLerp(0, maxHealth, health));
        }

        private void BarEnabled(bool var)
        {
            _hpBackground.enabled = var;
            _hpFill.enabled = var;
        }
    }
}
