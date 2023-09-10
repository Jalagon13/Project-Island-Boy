using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private Bar _healthBar;
        [SerializeField] private float _deathTimer;
        [SerializeField] private UnityEvent _onDeath;
        [SerializeField] private UnityEvent _onRespawn;

        private void Start()
        {
            _healthBar.MaxValue = _maxHealth;
            _healthBar.CurrentValue = _maxHealth;
            PR.Defense = 0;
            PR.SetSpawnPosition(transform.position);
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

                    WorldItemManager.Instance.SpawnItem(transform.root.position, itemObj, itemCount);
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

            if(SceneManager.GetActiveScene().buildIndex != 0)
            {
                LevelManager.Instance.LoadSurface();
            }

            transform.root.gameObject.transform.SetPositionAndRotation(PR.SpawnPosition, Quaternion.identity);
        }
    }
}
