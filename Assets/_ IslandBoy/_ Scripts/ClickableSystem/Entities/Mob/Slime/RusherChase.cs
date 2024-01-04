using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class RusherChase : StateMachineBehaviour
    {
        private SlimeStateManager _ctx;
        private Vector2 _chasePos;
        // private float _chaseSpeed = 2.5f;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Debug.Log("Entering Chase State");
            _ctx = animator.transform.root.GetComponent<SlimeStateManager>();
            // _ctx.AI.maxSpeed = _chaseSpeed;
            _chasePos = _ctx.PR.Position;
            _ctx.OnMove += Chase;

            MMSoundManagerSoundPlayEvent.Trigger(_ctx._agroSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _chasePos = _ctx.PR.Position; 

            if (!_ctx.PlayerClose(6) || !_ctx.CanGetToPlayer())
            {
                _ctx.ChangeToIdleState(animator);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // _ctx.AI.maxSpeed = 1;
            _ctx.OnMove -= Chase;
        }

        private void Chase()
        {
            _ctx.Seek(_chasePos);
        }
    }
}
