using UnityEngine;

namespace IslandBoy
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerBaseState _currentState;
        private PlayerStateFactory _states;
        private PlayerMoveInput _moveInput;

        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public Vector2 MoveDirection { get { return _moveInput.MoveDirection; } }
        public Vector2 LocalScale { set { transform.localScale = value; } }
        public bool IsFacingRight { get { return _moveInput.IsFacingRight; } }

        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            _moveInput = GetComponent<PlayerMoveInput>();
        }

        private void Start()
        {
            _currentState = _states.Grounded();
            _currentState.EnterState();
        }

        private void Update()
        {
            _currentState.UpdateStates();
        }
    }
}
