using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
    public class Entity : MonoBehaviour, IHealth<int>
    {
        public PlayerReference PR;
        public HealthSystem HealthSystem;

        [SerializeField] protected int _maxHealth;
        [SerializeField] protected float _iFrameDuration = 0.17f;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected AudioClip _damageSound;
        [SerializeField] protected AudioClip _deathSound;
        [SerializeField] private LootTable _lootTable;
        [SerializeField] private UnityEvent _onDamage;

        protected Timer _iFrameTimer;
        
        public float BaseMoveSpeed { get { return _moveSpeed; } }

        protected virtual void Awake()
        {
            HealthSystem = new HealthSystem(_maxHealth);
            _iFrameTimer = new Timer(_iFrameDuration);
        }

        protected virtual void Update()
        {
            _iFrameTimer.Tick(Time.deltaTime);
        }

        public virtual void Damage(int damageAmount, GameObject sender = null)
        {
            if (!CanDamage()) return;

            HealthSystem.Damage(damageAmount);

            _onDamage?.Invoke();
            _iFrameTimer.RemainingSeconds = _iFrameDuration;

            DamagePopup.Create(transform.position, damageAmount, 0.5f);
            AudioManager.Instance.PlayClip(_damageSound, false, true);

            if (sender != null && transform.TryGetComponent(out KnockbackFeedback knockback))
                knockback.PlayFeedback(sender);

            if (HealthSystem.IsDead())
                OnDeath();
        }

        public void Heal(int healAmount)
        {
            HealthSystem.Heal(healAmount);
        }

        public virtual void OnDeath()
        {
            _lootTable.SpawnLoot(transform.position);

            AudioManager.Instance.PlayClip(_deathSound, false, true);

            Destroy(gameObject);
        }

        public bool CanDamage()
        {
            return _iFrameTimer.RemainingSeconds <= 0;
        }
    }
}
