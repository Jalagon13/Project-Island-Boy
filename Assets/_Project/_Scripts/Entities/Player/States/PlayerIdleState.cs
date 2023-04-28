using UnityEngine;

namespace IslandBoy
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            //Debug.Log("Entered Idle State");
            Ctx.SpriteFlipHandle();
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
