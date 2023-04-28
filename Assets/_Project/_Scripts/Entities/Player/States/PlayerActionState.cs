using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerActionState : PlayerBaseState
    {

        public PlayerActionState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {
            //Debug.Log("Entered Action State");
            
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
