using System;
using UnityEngine;

namespace IslandBoy
{
    public class HealthSystem
    {
        public event EventHandler<HealthChangedEventArgs> OnHealthChanged;

        private int _health;
        private int _healthMax;

        public HealthSystem(int healthMax)
        {
            _healthMax = healthMax;
            _health = _healthMax;
        }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = (int)Mathf.Clamp(value, 0, _healthMax);

                OnHealthChanged?.Invoke(this, new HealthChangedEventArgs
                {
                    Health = _health,
                    MaxHealth = _healthMax
                });
            }
        }

        public int HealthMax { get { return _healthMax; } set { _healthMax = value; } }

        public bool IsDead()
        {
            return _health <= 0;
        }

        public bool IsDamaged()
        {
            return _health < _healthMax;
        }

        public bool IsFullHP()
        {
            return _health >= _healthMax;
        }

        public float GetHealthPercent()
        {
            return (float)_health / _healthMax;
        }

        public void Damage(int value)
        {
            value = (int)Mathf.Max(value, 0f);
            Health -= value;
        }

        public void Heal(int value)
        {
            value = (int)Mathf.Max(value, 0f);
            Health += value;
        }
    }

    public class HealthChangedEventArgs
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
}

