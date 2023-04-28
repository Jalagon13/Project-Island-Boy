using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlayerStateFactory
    {
        private PlayerStateMachine _context;

        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext;
        }

        public PlayerBaseState Idle()
        {
            return new PlayerIdleState(_context, this);
        }
        public PlayerBaseState Move()
        {
            return new PlayerMoveState(_context, this);
        }
        public PlayerBaseState Action()
        {
            return new PlayerActionState(_context, this);
        }
        public PlayerBaseState Grounded()
        {
            return new PlayerGroundedState(_context, this);
        }
    }
}
