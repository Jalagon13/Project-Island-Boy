using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private AudioClip _explosionSound;
        [SerializeField] private AudioSource _sizzlingSource;
        [SerializeField] private float _detonationTimer;
        [SerializeField] private float _explodeRadius;
        [SerializeField] private float _breakableDamage;
        [SerializeField] private int _enemyDamage;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_detonationTimer);

            BlowUpResources();
            BlowUpEntities();

            _explosionParticles.transform.parent = null;

            _explosionParticles.Play();

            _sizzlingSource.Stop();

            AudioManager.Instance.PlayClip(_explosionSound, false, true, 0.55f);

            Destroy(_explosionParticles.gameObject, _explosionParticles.main.duration);
            Destroy(gameObject);
        }

        private void BlowUpResources()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explodeRadius);

            foreach (Collider2D collider in colliders)
            {
                Resource rsc = collider.GetComponent<Resource>();

                if (!rsc)
                    continue;

                rsc.Hit(_breakableDamage);
            }
        }

        private void BlowUpEntities()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explodeRadius);

            foreach (Collider2D collider in colliders)
            {
                IHealth<int> health = collider.GetComponent<IHealth<int>>();

                if (health != null)
                    health.Damage(_enemyDamage);

                KnockbackFeedback knockback = collider.GetComponent<KnockbackFeedback>();

                if (knockback)
                    knockback.PlayFeedback(gameObject.transform.position);
            }
        }
    }
}
