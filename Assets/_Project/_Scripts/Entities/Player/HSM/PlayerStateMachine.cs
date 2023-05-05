using UnityEngine;

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

        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public Vector2 MoveDirection { get { return _moveInput.MoveDirection; } }


        private void Awake()
        {
            _rightDirScale = transform.localScale;
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
            _pr.UpdatePlayerPositionReference(transform.position);
        }

        public void SpriteFlipHandle()
        {
            transform.localScale = _moveInput.IsFacingRight ? _rightDirScale : _leftDirScale;
        }
    }
}
