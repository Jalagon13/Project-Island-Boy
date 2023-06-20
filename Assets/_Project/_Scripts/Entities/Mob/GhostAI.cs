using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GhostAI : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private float _speed;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(CalcMovePosition());
        }

        private Vector2 CalcMovePosition()
        {
            Vector2 movement = (_pr.PositionReference - (Vector2)transform.position).normalized * _speed * Time.deltaTime;

            return _rb.position + movement;
        }
    }
}