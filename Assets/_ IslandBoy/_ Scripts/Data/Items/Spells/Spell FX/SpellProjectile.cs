using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
// Written by Justin Alagon

namespace IslandBoy
{
    public abstract class SpellProjectile : MonoBehaviour
    {
        public bool SpellTargetPlayer; // if this is true, it targets player
        public bool SpellEffectPlayer; //Can effect player
        public bool DestroyOnHit; //If the projectile should be destroyed on hit
        public LayerMask StrikeableMask;
        public float CastForce;
        public float ChargeDuration = 0.5f;
        public float ProjectileDuration = 15f;
        public float StrikeRadius = 1f;
        public float Damage = 100f;
        public Camera Cam { get; set; }
        public Vector2 CastPoint;

        //[SerializeField]
        //private GameObject _spell; // For spellBehavior

        [SerializeField] private GameObject _spellBehav;

        private Rigidbody2D _projRb;
        private Vector2 _castDir;

        private protected virtual void Awake()
        {
            _projRb = GetComponent<Rigidbody2D>();

            //Destroy(gameObject, ProjectileDuration);
        }

        private protected virtual void Start()
        {
            //_pv = GetComponent<PhotonView>();
            StartCoroutine(CastProjectileSpell());
            //Resplace destroy with coroutine for multi -Nick
            StartCoroutine(MaxDuration());
        }

        //Destroy after max duration
        protected IEnumerator MaxDuration()
        {
            yield return new WaitForSeconds(ProjectileDuration);
            DestroyProj();
        }

        private protected IEnumerator CastProjectileSpell()
        {
            yield return new WaitForSeconds(ChargeDuration);

            MousePosToPoint();

            _castDir = CastPoint - (Vector2)transform.position;

            //FindObjectOfType<AudioManager>().Play("PEW");

            _projRb.AddForce(_castDir.normalized * CastForce);
        }

        private protected IEnumerator CastMousePosSpell()
        {
            yield return new WaitForSeconds(ChargeDuration);

            MousePosToPoint();

            transform.position = new Vector2(CastPoint.x, CastPoint.y);
            Debug.Log("Shark Spawned");
            Vector2 spawnPos = new Vector3(CastPoint.x, CastPoint.y);

            if (transform.position != Vector3.zero)
                GameStateManager.Instantiate(_spellBehav, spawnPos + Random.insideUnitCircle, transform.rotation);
            //PhotonNetwork.Instantiate(_spellBehav.name, spawnPos + Random.insideUnitCircle, transform.rotation);
            Destroy(gameObject, 3f);
        }

        private void MousePosToPoint()
        {
            if (SpellTargetPlayer)
            {
                // Find nearest player
                CastPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
            }
        }

        public void DestroyProj()
        {
            _projRb.velocity = Vector2.zero;
            Instantiate(_spellBehav, transform);

            Destroy(gameObject);
        }

        /*
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Updated to stop enemy projects colliding with themselves 
            //Might want them to actually hit terrain later though which they don't do now -Nick 
            if ((collision.gameObject.layer == 3 && !SpellTargetPlayer) || (collision.gameObject.layer == 3 || collision.gameObject.layer == 6 && SpellTargetPlayer))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, StrikeRadius, StrikeableMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<Destructible>(out Destructible dest))
                   {
                        dest.TakeDamage(Damage);
                        break;
                    }
                }

            }
        }
        */
    }
}