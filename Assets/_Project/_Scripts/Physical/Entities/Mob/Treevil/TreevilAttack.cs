using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TreevilAttack : StateMachineBehaviour
    {
        private TreevilStateManager _ctx;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log("Entering Chase State");
            _ctx = animator.transform.root.GetComponent<TreevilStateManager>();

            AudioManager.Instance.PlayClip(_ctx._agroSound, false, true, 0.25f);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_ctx.PlayerClose())
            {
                _ctx.ChangeToIdleState(animator);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

    }
}
