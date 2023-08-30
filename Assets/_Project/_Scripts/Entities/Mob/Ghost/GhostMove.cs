using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GhostMove : StateMachineBehaviour
    {
        private GhostEntity _ctx;
        private Vector2 _wanderPos;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Ghost Move");
            _ctx = animator.transform.root.GetComponent<GhostEntity>();
            _wanderPos = CalcWanderPos();
            _ctx.ReachedDestination = false;
            _ctx.OnMove += Move;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_ctx.ReachedDestination)
            {
                _ctx.ChangeToIdleState(animator);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx.OnMove -= Move;
        }

        private void Move()
        {
            _ctx.Seek(_wanderPos);
        }

        private Vector2 CalcWanderPos()
        {
            Vector3 randDir = Random.insideUnitCircle.normalized * 5;
            var pos = _ctx.transform.position + randDir;

            return pos;
        }
    }
}
