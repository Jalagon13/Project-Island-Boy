namespace IslandBoy
{
    public abstract class PlayerBaseState
    {
        private PlayerStateMachine _ctx;
        private PlayerStateFactory _factory;
        private PlayerBaseState _currentSubState;
        private PlayerBaseState _currentSuperState;

        private bool _isRootState = false;

        protected bool IsRootState { set { _isRootState = value; } }
        protected PlayerStateMachine Ctx { get { return _ctx; } }
        protected PlayerStateFactory Factory { get { return _factory; } }

        public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        {
            _ctx = currentContext;
            _factory = playerStateFactory;
        }

        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        public void UpdateStates()
        {
            UpdateState();
            if (_currentSubState != null)
                _currentSubState.UpdateStates();
        }
        protected void SwitchState(PlayerBaseState newState)
        {
            // current state exits state
            ExitState();

            // new state enters state
            newState.EnterState();

            if (_isRootState)
            {
                // switch current state of context
                _ctx.CurrentState = newState;
            }
            else if (_currentSuperState != null)
            {
                // set the current super state sub state to the new state
                _currentSuperState.SetSubState(newState);
            }
        }
        protected void SetSuperState(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }
        protected void SetSubState(PlayerBaseState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }
    }
}