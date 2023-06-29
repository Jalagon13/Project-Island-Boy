using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private Bar _healthBar;

        private void Start()
        {
            _healthBar.MaxValue = _maxHealth;
            _healthBar.CurrentValue = _maxHealth;
        }

        public override void Damage(int damageAmount, GameObject sender = null)
        {
            if (!CanDamage()) return;

            _iFrameTimer.RemainingSeconds = _iFrameDuration;
            _healthBar.AddTo(-damageAmount);
            HealthSystem.Damage(damageAmount);

            DamagePopup.Create(transform.position, damageAmount, 0.5f);
            AudioManager.Instance.PlayClip(_damageSound, false, true);

            if (sender != null && transform.TryGetComponent(out KnockbackFeedback knockback))
                knockback.PlayFeedback(sender);

            if (HealthSystem.IsDead())
                OnDeath();
        }

        public override void OnDeath()
        {
            Debug.Log("Player Dead");
        }
    }
}
