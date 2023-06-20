using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Entity : MonoBehaviour, IHealth<int>
    {
        public HealthSystem HealthSystem;

        [SerializeField] protected int _maxHealth;
        [SerializeField] protected float _iFrameDuration;
        [SerializeField] private float _forcePower;
        [SerializeField] private LootTable _lootTable;

        protected Timer _iFrameTimer;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            HealthSystem = new HealthSystem(_maxHealth);
            _iFrameTimer = new Timer(_iFrameDuration);
        }

        private void Update()
        {
            _iFrameTimer.Tick(Time.deltaTime);
        }

        public virtual void Damage(int damageAmount)
        {
            if (!CanDamage()) return;

            _iFrameTimer.RemainingSeconds = _iFrameDuration;
            _rb.AddForce(transform.right * _forcePower);
            HealthSystem.Damage(damageAmount);
            DamagePopup.Create(transform.position, damageAmount, 0.5f);

            if (HealthSystem.IsDead())
            {
                OnDeath();
            }
        }

        public void Heal(int healAmount)
        {
            HealthSystem.Heal(healAmount);
        }

        public virtual void OnDeath()
        {
            _lootTable.SpawnLoot(transform.position);
            Destroy(gameObject);
        }

        protected bool CanDamage()
        {
            return _iFrameTimer.RemainingSeconds <= 0;
        }
    }
}
