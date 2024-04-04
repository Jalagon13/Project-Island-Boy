using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
	public class PlayerStateMachine : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;

		private PlayerBaseState _currentState;
		private PlayerStateFactory _states;
		private PlayerMoveInput _moveInput;
		private Camera _mainCamera;
		private SpriteRenderer _sr;

		public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
		public Vector2 MoveDirection { get { return _moveInput.MoveDirection; } }

		private void Awake()
		{
			_pr.SpawnPoint = transform.position;
			_pr.GameObject = gameObject;
			_sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
			_states = new PlayerStateFactory(this);
			_moveInput = GetComponent<PlayerMoveInput>();
			
			GameSignals.DAY_END.AddListener(OnEndDay);
			GameSignals.DAY_START.AddListener(OnStartDay);
			GameSignals.PLAYER_DIED.AddListener(PlayerDied);
			GameSignals.PLAYER_RESPAWN.AddListener(PlayerRespawn);
		}

		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(OnEndDay);
			GameSignals.DAY_START.RemoveListener(OnStartDay);
			GameSignals.PLAYER_DIED.RemoveListener(PlayerDied);
			GameSignals.PLAYER_RESPAWN.RemoveListener(PlayerRespawn);
		}

		private void Start()
		{
			_currentState = _states.Grounded();
			_currentState.EnterState();
			_mainCamera = Camera.main;
		}

		private void LateUpdate()
		{
			_currentState.UpdateStates();
			_pr.MousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		}

		private void PlayerDied(ISignalParameters parameters)
		{
			EnableMovement(false);
		}
		
		private void PlayerRespawn(ISignalParameters parameters)
		{
			EnableMovement(true);
		}

		private void OnEndDay(ISignalParameters parameters)
		{
			TeleportPlayerToSpawn();
			EnableMovement(false);
		}

		private void OnStartDay(ISignalParameters parameters)
		{
			EnableMovement(true);
		}

		private void EnableMovement(bool _)
		{
			var input = GetComponent<PlayerMoveInput>();
			input.enabled = _;
		}

		private void TeleportPlayerToSpawn()
		{
			transform.SetPositionAndRotation(_pr.SpawnPoint, Quaternion.identity);
		}
	}
}
