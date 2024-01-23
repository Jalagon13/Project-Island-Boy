using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target; //Target point to rotate around
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        private float _angle;

        private PlayerInput _fishingInput;
        private Vector2 _moveDirection;
        private bool _isMoving = false;

        private void Awake()
        {
            _fishingInput = new PlayerInput();
            _fishingInput.Enable();
            _fishingInput.Player.Fishing.started += OnFishingInput;
            _fishingInput.Player.Fishing.canceled += OffFishingInput;
        }

        private void FixedUpdate()
        {
            if (_isMoving)
                Move();
        }

        public void OnFishingInput(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
            _isMoving = true;
        }

        public void OffFishingInput(InputAction.CallbackContext context)
        {
            _isMoving = false;
        }

        public void Move()
        {
            if (_moveDirection.x > 0)
            {
                _angle -= _speed + Time.deltaTime; // right = clockwise
            }
            else if (_moveDirection.x < 0)
            {
                _angle += _speed + Time.deltaTime; // left = counter-clockwise
            }

            float x = _target.position.x + Mathf.Cos(_angle) * _radius;
            float y = _target.position.y + Mathf.Sin(_angle) * _radius;

            transform.position = new Vector3(x, y, 0);

            float rotationAngle = Mathf.Atan2(y - _target.position.y, x - _target.position.x) * Mathf.Rad2Deg;

            // Set the rotation of the object
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }
}
