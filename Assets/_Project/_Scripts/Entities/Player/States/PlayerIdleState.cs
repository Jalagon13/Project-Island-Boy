using UnityEngine;

namespace IslandBoy
{
    public class PlayerIdleState : PlayerBaseState
    {
        private static Vector3 _rightDirScale = new(1, 1, 1);
        private static Vector3 _leftDirScale = new(-1, 1, 1);

        public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            Debug.Log("Entered Idle State");
            Ctx.LocalScale = Ctx.IsFacingRight ? _rightDirScale : _leftDirScale;
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void CheckSwitchStates()
        {
            if (Ctx.MoveDirection.magnitude > 0)
                SwitchState(Factory.Move());
        }
    }
}
