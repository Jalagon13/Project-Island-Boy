using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private static Vector3 _rightDirScale;
        private static Vector3 _leftDirScale;
        private PlayerBaseState _currentState;
        private PlayerStateFactory _states;
        private PlayerMoveInput _moveInput;
        private Camera _mainCamera;
        private SpriteRenderer _sr;

        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public Vector2 MoveDirection { get { return _moveInput.MoveDirection; } }

        private void Awake()
        {
            _pr.PlayerGO = gameObject;
            _pr.SpawnPoint = transform.position;
            _mainCamera = Camera.main;
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _rightDirScale = _sr.transform.localScale;
            _leftDirScale = new(-_rightDirScale.x, _rightDirScale.y);
            _states = new PlayerStateFactory(this);
            _moveInput = GetComponent<PlayerMoveInput>();
        }

        private void OnEnable()
        {
            GameSignals.DAY_END.AddListener(OnEndDay);
            GameSignals.DAY_START.AddListener(OnStartDay);
        }

        private void OnDisable()
        {
            GameSignals.DAY_END.RemoveListener(OnEndDay);
            GameSignals.DAY_START.RemoveListener(OnStartDay);
        }

        private void Start()
        {
            _currentState = _states.Grounded();
            _currentState.EnterState();
        }

        private void Update()
        {
            _currentState.UpdateStates();
            _pr.Position = transform.position;
            _pr.MousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                LevelManager.Instance.LoadSurface();
            }

            transform.SetPositionAndRotation(_pr.SpawnPoint, Quaternion.identity);
        }

        public void SpriteFlipHandle()
        {
            _sr.transform.localScale = _moveInput.IsFacingRight ? _rightDirScale : _leftDirScale;
        }
    }
}
