using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Entity : MonoBehaviour, IHealth<int>
    {
        public HealthSystem HealthSystem;

        [SerializeField] private int _maxHealth;
        [SerializeField] private EntityHealthBar _hpBar;
        [SerializeField] private LootTable _lootTable;

        private void Awake()
        {
            HealthSystem = new HealthSystem(_maxHealth);
            _hpBar.SetUp(HealthSystem);
        }

        public void Damage(int damageAmount)
        {
            HealthSystem.Damage(damageAmount);
            
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
            _lootTable.SpawnLoot(transform.position);
            Destroy(gameObject);
        }
    }
}
