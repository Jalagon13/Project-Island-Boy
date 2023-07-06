using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ItemSeaWander : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Vector2 _dir;
        private float _speed;

        private void FixedUpdate()
        {
            if (_rb == null) return;

            _rb.MovePosition(_rb.position + _dir * _speed * Time.deltaTime);
        }

        public void Wander(PlayerReference pr)
        {
            _rb = GetComponent<Rigidbody2D>();
            _speed = 1f;

            var perpOffset = Vector2.Perpendicular((pr.Position - (Vector2)transform.position).normalized);

            if (Random.Range(0f, 1f) < 0.5f)
                perpOffset *= -1;

            _dir = ((pr.Position + (perpOffset * Random.Range(8f, 10f))) - (Vector2)transform.position).normalized;

            Destroy(gameObject, 60f);
        }
    }
}
