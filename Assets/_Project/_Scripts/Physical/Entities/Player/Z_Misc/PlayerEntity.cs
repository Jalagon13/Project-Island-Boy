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
        [SerializeField] private UnityEvent _onDeath;
        [SerializeField] private UnityEvent _onRespawn;

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

            int damageDelt = CalcDamage(incomingDamage);

            _iFrameTimer.RemainingSeconds = _iFrameDuration;
            _onDamage?.Invoke(-damageDelt);

            HealthSystem.Damage(damageDelt);

            PopupMessage.Create(transform.position, damageDelt.ToString(), Color.red, new(0f, 0.5f));
            AudioManager.Instance.PlayClip(_damageSound, false, true);

            if (sender != null && transform.TryGetComponent(out KnockbackFeedback knockback))
                knockback.PlayFeedback(sender);

            if (HealthSystem.IsDead())
                KillEntity();
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
            foreach (Slot slot in PR.Inventory.InventorySlots)
            {
                if (slot.InventoryItem != null)
                {
                    var itemObj = slot.ItemObject;
                    var itemCount = slot.InventoryItem.Count;

                    PR.Inventory.RemoveItem(itemObj, itemCount);

                    GameAssets.Instance.SpawnItem(transform.root.position, itemObj, itemCount);
                }
            }

            AudioManager.Instance.PlayClip(_deathSound, false, true);

            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            _onDeath?.Invoke();

            yield return new WaitForSeconds(_deathTimer);
            _onRespawn?.Invoke();

            HealthSystem = new(_maxHealth);
        }
    }
}
