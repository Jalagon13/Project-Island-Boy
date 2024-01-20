using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class NPCIdle : StateMachineBehaviour
    {
        private NPCStateManager _ctx;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log("Entering Idle State");
            _ctx = animator.transform.parent.GetComponent<NPCStateManager>();
            // _ctx.AI
            _ctx.OnMove += Idle;
            _ctx.StartCoroutine(IdleDuration(animator));
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

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
            yield return new WaitForSeconds(Random.Range(8f, 16f));
            // _ctx.Agent.isStopped = false;
            _ctx.ChangeToMoveState(animator);
        }
    }
}
