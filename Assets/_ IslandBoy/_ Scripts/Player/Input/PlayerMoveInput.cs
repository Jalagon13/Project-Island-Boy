using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IslandBoy
{
	public class PlayerMoveInput : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private UnityEvent _frontMove;
		[SerializeField] private UnityEvent _rightMove;
		[SerializeField] private UnityEvent _leftMove;
		[SerializeField] private UnityEvent _backMove;
		[SerializeField] private UnityEvent _idle;
		

		private Rigidbody2D _rb;
		private PlayerInput _playerInput;
		private Vector2 _moveDirection;
		private Vector2 _lastNonZeroMoveDirection;
		private SpriteRenderer _sr;
		private float _baseSpeed;

		public float Speed { get { return _speed; } set { _speed = value; } }
		public float BaseSpeed { get { return _baseSpeed; }}

		public Vector2 MoveDirection { get { return _moveDirection; } }
		public Vector2 LastNonZeroMoveDirection { get { return _lastNonZeroMoveDirection; } }

		private void Awake()
		{
			_playerInput = new();
			_baseSpeed = _speed;
			_sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
			// _playerInput.Player.Move.started += MovementAction;
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
			
			// if (_moveDirection.x != 0)
			// 	_isFacingRight = _moveDirection.x > 0;
			
			if(_moveDirection.x > 0)
			{
				_rightMove?.Invoke();
				Debug.Log("Right");
				_sr.flipX = false;
			}
			else if(_moveDirection.x < 0)
			{
				_leftMove?.Invoke();
				Debug.Log("Left");
				_sr.flipX = true;
			}
			else if(_moveDirection.y > 0)
			{
				_backMove?.Invoke();
				Debug.Log("Back");
				_sr.flipX = false;
			}
			else if(_moveDirection.y < 0)
			{
				_frontMove?.Invoke();
				Debug.Log("Front");
				_sr.flipX = false;
			}
			else if(_moveDirection.magnitude <= 0)
			{
				_idle?.Invoke();
				Debug.Log(("Idle"));
			}
				
			if(_moveDirection.magnitude != 0)
				_lastNonZeroMoveDirection = _moveDirection;
		}
	}
}
