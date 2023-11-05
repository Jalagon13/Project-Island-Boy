using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private float _deathTimer;
        [SerializeField] private UnityEvent _localDeathHandle;
        [SerializeField] private UnityEvent _localRespawnHandle;

        private void Start()
        {
            PR.Defense = 0;
        }

        private void OnEnable()
        {
            GameSignals.DAY_OUT_OF_TIME.AddListener(KillPlayer);
            
        }

        private void OnDisable()
        {
            GameSignals.DAY_OUT_OF_TIME.RemoveListener(KillPlayer);
        }

        private void KillPlayer(ISignalParameters parameter)
        {
            KillEntity();
        }

        public override void Damage(int incomingDamage, GameObject sender = null)
        {
            if (!CanDamage()) return;

            int damageDealt = CalcDamage(incomingDamage);

            _iFrameTimer.RemainingSeconds = _iFrameDuration;
            _onDamage?.Invoke(-damageDealt);

            HealthSystem.Damage(damageDealt);

            PopupMessage.Create(transform.position, damageDealt.ToString(), Color.red, new(0f, 0.5f));
            AudioManager.Instance.PlayClip(_damageSound, false, true);

            if (HealthSystem.IsDead())
                KillEntity();
            else if (sender != null && transform.TryGetComponent(out KnockbackFeedback knockback))
                knockback.PlayFeedback(sender);
        }

        private int CalcDamage(int damage)
        {
            int dmg = damage - PR.Defense;

            if(dmg < 1)
                dmg = 1;

            return dmg;
        }

        public override void KillEntity()
        {
            AudioManager.Instance.PlayClip(_deathSound, false, true);

            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            _localDeathHandle?.Invoke();

            GameSignals.PLAYER_DIED.Dispatch();

            yield return new WaitForSeconds(_deathTimer);

            _localRespawnHandle?.Invoke();

            GameSignals.DAY_END.Dispatch();

            HealthSystem = new(_maxHealth);
        }
    }
}
