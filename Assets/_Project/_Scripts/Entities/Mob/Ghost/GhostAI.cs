using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GhostAI : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private float _speed;
        [SerializeField] private float _teleportDistance;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            _rb.MovePosition(CalcMovePosition());
        }

        public void TeleportToPlayer() // attached to Entity _onDamage Event
        {
            var direction = Random.insideUnitCircle.normalized;

            _rb.position = _pr.Position + direction * _teleportDistance;
        }

        private Vector2 CalcMovePosition()
        {
            Vector2 movement = (_pr.Position - (Vector2)transform.position).normalized * _speed * Time.deltaTime;

            return _rb.position + movement;
        }
    }
}