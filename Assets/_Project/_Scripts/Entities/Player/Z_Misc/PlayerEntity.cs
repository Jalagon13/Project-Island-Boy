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
            PR.Defense = 0;
        }

        public override void Damage(int incomingDamage, GameObject sender = null)
        {
            if (!CanDamage()) return;

            int damageDelt = CalcDamage(incomingDamage);

            _iFrameTimer.RemainingSeconds = _iFrameDuration;
            _healthBar.AddTo(-damageDelt);
            _onDamage?.Invoke(damageDelt);

            HealthSystem.Damage(damageDelt);

            DamagePopup.Create(transform.position, damageDelt, 0.5f);
            AudioManager.Instance.PlayClip(_damageSound, false, true);

            if (sender != null && transform.TryGetComponent(out KnockbackFeedback knockback))
                knockback.PlayFeedback(sender);

            if (HealthSystem.IsDead())
                OnDeath();
        }

        private int CalcDamage(int damage)
        {
            int dmg = damage - PR.Defense;

            if(dmg < 1)
                dmg = 1;

            return dmg;
        }

        public override void OnDeath()
        {
            Debug.Log("Player Dead");
        }
    }
}
