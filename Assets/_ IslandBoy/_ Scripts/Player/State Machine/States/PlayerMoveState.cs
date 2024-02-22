using UnityEngine;

namespace IslandBoy
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            // Debug.Log("Entered Move State");
        }

        public override void UpdateState()
        {
            // Ctx.SpriteFlipHandle();
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
            if (Ctx.MoveDirection.magnitude == 0)
                SwitchState(Factory.Idle());
        }
    }
}
