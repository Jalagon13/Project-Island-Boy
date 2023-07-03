using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CrabIdle : StateMachineBehaviour
    {
        private CrabEntity _ctx;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx = animator.transform.root.GetComponent<CrabEntity>();
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
            yield return new WaitForSeconds(Random.Range(3f, 5f));
            ChangeToMoveState(animator);
        }

        private void ChangeToMoveState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, _ctx.HashMove);
        }
    }
}
