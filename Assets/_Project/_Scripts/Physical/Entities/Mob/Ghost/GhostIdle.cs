using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GhostIdle : StateMachineBehaviour
    {
        private GhostEntity _ctx;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Ghost Idle");
            _ctx = animator.transform.root.GetComponent<GhostEntity>();
            _ctx.ReachedDestination = true;
            _ctx.StartCoroutine(IdleDuration(animator));
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        private IEnumerator IdleDuration(Animator animator)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            _ctx.ChangeToMoveState(animator);
        }
    }
}
