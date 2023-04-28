using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class PlayerMoveInput : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;

        private Rigidbody2D _rb;
        private Vector2 _moveDirection;
        private PlayerInput _playerInput;

        public Vector2 MoveDirection { get { return _moveDirection; } }

        private void Awake()
        {
            _playerInput = new();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        private void Start()
        {
            _playerInput.Player.Move.started += MovementAction;
            _playerInput.Player.Move.performed += MovementAction;
            _playerInput.Player.Move.canceled += MovementAction;
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _moveDirection * _speed * Time.deltaTime);
        }

        private void MovementAction(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
    }
}
