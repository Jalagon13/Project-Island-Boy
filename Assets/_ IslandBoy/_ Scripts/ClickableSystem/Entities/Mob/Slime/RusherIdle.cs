using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class RusherIdle : StateMachineBehaviour
    {
        private SlimeStateManager _ctx;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log("Entering Idle State");
            _ctx = animator.transform.root.GetComponent<SlimeStateManager>();
            // _ctx.AI.isStopped = true;
            _ctx.OnMove += Idle;
            _ctx.StartCoroutine(IdleDuration(animator));
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_ctx.PlayerClose(_ctx.AgroDistance) && _ctx.CanGetToPlayer())
            {
                // _ctx.AI.isStopped = false;
                _ctx.ChangeToChaseState(animator);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx.OnMove -= Idle;
        }

        private void Idle()
        {
            
        }

        private IEnumerator IdleDuration(Animator animator)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            // _ctx.AI.isStopped = false;
            _ctx.ChangeToMoveState(animator);
        }


    }
}
