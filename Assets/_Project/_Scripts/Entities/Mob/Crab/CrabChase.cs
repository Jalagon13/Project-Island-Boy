using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CrabChase : StateMachineBehaviour
    {
        private CrabEntity _ctx;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx = animator.transform.root.GetComponent<CrabEntity>();
            _ctx.OnMove += Chase;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx.OnMove -= Chase;
        }

        private void Chase()
        {

        }
    }
}
