using UnityEngine;

namespace IslandBoy
{
    public class PlayerStateMachine : MonoBehaviour
    {
        //[SerializeField] private Sprite _cursorSprite;

        private static Vector3 _rightDirScale = new(1, 1, 1);
        private static Vector3 _leftDirScale = new(-1, 1, 1);
        private PlayerBaseState _currentState;
        private PlayerStateFactory _states;
        private PlayerMoveInput _moveInput;

        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public Vector2 MoveDirection { get { return _moveInput.MoveDirection; } }


        private void Awake()
        {
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
        }

        public void SpriteFlipHandle()
        {
            transform.localScale = _moveInput.IsFacingRight ? _rightDirScale : _leftDirScale;
        }
    }
}
