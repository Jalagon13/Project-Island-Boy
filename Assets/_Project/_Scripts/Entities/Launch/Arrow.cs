using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Arrow : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Vector2 direction = (_rb.velocity - (Vector2)transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var offset = 0f;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }

        private void Update()
        {
            if(_rb.velocity.magnitude < 3f)
            {
                Destroy(gameObject);
            }
        }
    }
}
