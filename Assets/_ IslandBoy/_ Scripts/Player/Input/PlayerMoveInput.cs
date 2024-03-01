using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IslandBoy
{
	public class PlayerMoveInput : MonoBehaviour
	{
		[SerializeField] private float _speed;
		[SerializeField] private UnityEvent _onIdle;
		[SerializeField] private UnityEvent _onMove;
		[SerializeField] private UnityEvent _onFrontMove;
		[SerializeField] private UnityEvent _onRightMove;
		[SerializeField] private UnityEvent _onLeftMove;
		[SerializeField] private UnityEvent _onBackMove;
		
		private Rigidbody2D _rb;
		private PlayerInput _playerInput;
		private Vector2 _moveDirection;
		private Vector2 _lastNonZeroMoveDirection;
		private float _baseSpeed;
		private bool _swinging;
		private bool _moving;
		private bool _canMove = true;

		public float Speed { get { return _speed; } set { _speed = value; } }
		public float BaseSpeed { get { return _baseSpeed; }}
		public Vector2 MoveDirection { get { return _moveDirection; } }
		public Vector2 LastNonZeroMoveDirection { get { return _lastNonZeroMoveDirection; } }

		private void Awake()
		{
			_playerInput = new();
			_baseSpeed = _speed;
			_rb = GetComponent<Rigidbody2D>();
			
			GameSignals.SCENE_TRANSITION_START.AddListener(DisableMovement);
			GameSignals.SCENE_TRANSITION_END.AddListener(EnableMovement);
		}
		
		private void OnDestroy()
		{
			GameSignals.SCENE_TRANSITION_START.RemoveListener(DisableMovement);
			GameSignals.SCENE_TRANSITION_END.RemoveListener(EnableMovement);
			
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
		
		private void EnableMovement(ISignalParameters parameters)
		{
			// _canMove = true;
			_playerInput.Enable();
		}
		
		private void DisableMovement(ISignalParameters parameters)
		{
			// _canMove = false;
			_playerInput.Disable();
			_moveDirection = Vector2.zero;
			_onIdle?.Invoke();
			StopAllCoroutines();
		}

		public void SetSwing(bool _)
		{
			_swinging = _;
		}
		
		public void InvokeBackMove()
		{
			_onBackMove?.Invoke();
			_swinging = true;
		}
		
		public void InvokeFrontMove()
		{
			_onFrontMove?.Invoke();
			_swinging = true;
		}
		
		public void InvokeRightMove()
		{
			_onRightMove?.Invoke();
			_swinging = true;
		}
		
		public void InvokeLeftMove()
		{
			_onLeftMove?.Invoke();
			_swinging = true;
		}
		
		public void SetMoveAnimation()
		{
			if(_moveDirection.magnitude > 0.1f || _moveDirection.magnitude < -0.1f)
			{
				_onMove?.Invoke();
				_moving = true;
			}
			else
			{
				_onIdle?.Invoke();
				_moving = false;
			}
			
			if(_moving)
			{
				if(_moveDirection.x > 0 && _moveDirection.y > 0)
				{
					_onBackMove?.Invoke();
				}
				else if(_moveDirection.x < 0 && _moveDirection.y > 0)
				{
					_onBackMove?.Invoke();
				}
				else if(_moveDirection.x < 0 && _moveDirection.y < 0)
				{
					_onFrontMove?.Invoke();
				}
				else if(_moveDirection.x > 0 && _moveDirection.y < 0)
				{
					_onFrontMove?.Invoke();
				}
				else if(_moveDirection.x == 1)
				{
					_onRightMove?.Invoke();
				}
				else if(_moveDirection.x == -1)
				{
					_onLeftMove?.Invoke();
				}
				else if(_moveDirection.y == 1)
				{
					_onBackMove?.Invoke();
				}
				else if(_moveDirection.y == -1)
				{
					_onFrontMove?.Invoke();
				}
			}
		}
		
		private void MovementAction(InputAction.CallbackContext context)
		{
			// if(!_canMove) return;
			
			_moveDirection = context.ReadValue<Vector2>();
			
			StartCoroutine(PlayTwice());
		}
		
		private void MoveHandle()
		{
			if(_moveDirection.magnitude > 0.1f || _moveDirection.magnitude < -0.1f)
			{
				_onMove?.Invoke();
				_moving = true;
			}
			else
			{
				_onIdle?.Invoke();
				_moving = false;
			}
			
			if(!_swinging)
			{
				SetMoveAnimation();
			}
				
			if(_moveDirection.magnitude != 0)
				_lastNonZeroMoveDirection = _moveDirection;
		}
		
		private IEnumerator PlayTwice()
		{
			MoveHandle();
			yield return new WaitForEndOfFrame();
			MoveHandle();
		}
	}
}
