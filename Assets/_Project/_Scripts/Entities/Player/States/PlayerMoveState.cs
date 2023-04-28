using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerMoveState : PlayerBaseState
    {
        private bool temp;

        public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            //Debug.Log("Entered Move State");
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
            
        }
    }
}
