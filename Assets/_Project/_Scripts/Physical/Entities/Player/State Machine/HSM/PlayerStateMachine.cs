using UnityEngine;
using UnityEngine.InputSystem;

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
            _mainCamera = Camera.main;
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _rightDirScale = _sr.transform.localScale;
            _leftDirScale = new(-_rightDirScale.x, _rightDirScale.y);
            _states = new PlayerStateFactory(this);
            _moveInput = GetComponent<PlayerMoveInput>();
        }

        private void Start()
        {
            _currentState = _states.Grounded();
            _currentState.EnterState();
            //Cursor.SetCursor(_cursorSprite.texture, Vector2.zero, CursorMode.ForceSoftware);
        }

        private void Update()
        {
            _currentState.UpdateStates();
            _pr.Position = transform.position;
            _pr.MousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        public void SpriteFlipHandle()
        {
            _sr.transform.localScale = _moveInput.IsFacingRight ? _rightDirScale : _leftDirScale;
        }
    }
}
