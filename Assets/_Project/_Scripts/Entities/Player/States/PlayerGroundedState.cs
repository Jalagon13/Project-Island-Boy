using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState()
        {
            //Debug.Log("Entered Grounded State");
            
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

        private void SetUpParameters()
        {
            
        }

        private void AnimHandle()
        {
            
        }
    }
}
