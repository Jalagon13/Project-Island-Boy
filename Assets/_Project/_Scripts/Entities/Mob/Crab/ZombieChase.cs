using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ZombieChase : StateMachineBehaviour
    {
        private ZombieEntity _ctx;
        private Vector2 _chasePos;
        private float _chaseSpeed = 1.5f;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log("Entering Chase State");
            _ctx = animator.transform.root.GetComponent<ZombieEntity>();
            _ctx.AI.maxSpeed = _chaseSpeed;
            _chasePos = _ctx.PR.Position;
            _ctx.OnMove += Chase;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _chasePos = _ctx.PR.Position; 

            if (!_ctx.PlayerClose())
            {
                _ctx.ChangeToIdleState(animator);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx.AI.maxSpeed = _ctx.BaseMoveSpeed;
            _ctx.OnMove -= Chase;
        }

        private void Chase()
        {
            _ctx.Seek(_chasePos);
        }
    }
}
