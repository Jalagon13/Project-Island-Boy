using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Entity : MonoBehaviour, IHealth<int>
    {
        public HealthSystem HealthSystem;

        [SerializeField] private int _maxHealth;

        private void Awake()
        {
            HealthSystem = new HealthSystem(_maxHealth);
        }

        public void Damage(int damageAmount)
        {
            HealthSystem.Damage(damageAmount);
            Debug.Log(HealthSystem.Health);
            if (HealthSystem.IsDead())
            {
                OnDeath();
            }
        }

        public void Heal(int healAmount)
        {
            HealthSystem.Heal(healAmount);
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
