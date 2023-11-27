using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TreevilThorn : MonoBehaviour
    {
        [SerializeField] private float _force;
        [SerializeField] private int _damageAmount;

        public PlayerReference PR;
        private Rigidbody2D rb;
        private float timer;
        private Clickable _clickableFound = null;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            Vector3 playerPos = PR.Position;
            Vector3 direction = playerPos - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * _force;

            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }

        void FixedUpdate()
        {
            timer += Time.deltaTime;

            if(timer > 5)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                Vector2 damagerPosition = transform.root.gameObject.transform.position;
                player.Damage(_damageAmount, damagerPosition);

                Destroy(gameObject);
            }

            GameObject colliderGo = collision.gameObject;

            if (!collision.gameObject.CompareTag("MOB"))
            {
                if (colliderGo.TryGetComponent<Clickable>(out var clickable))
                {
                    if (_clickableFound != null) return;

                    _clickableFound = clickable;
                    _clickableFound.OnHit(ToolType.Sword, _damageAmount, false);

                    Destroy(gameObject);
                }
            }
        }
    }
}
