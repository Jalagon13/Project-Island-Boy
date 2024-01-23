using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class SlimeChase : StateMachineBehaviour
	{
		private SlimeStateManager _ctx;
		private Vector2 _chasePos;
		private float _chaseSpeed = 2.5f;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//Debug.Log("Entering Chase State");
			_ctx = animator.transform.root.GetComponent<SlimeStateManager>();
			_ctx.AI.maxSpeed = _chaseSpeed;
			_ctx.AI.isStopped = false;
			_chasePos = _ctx.PR.Position;
			_ctx.Seek(_chasePos);

			MMSoundManagerSoundPlayEvent.Trigger(_ctx._agroSound, MMSoundManager.MMSoundManagerTracks.Sfx, default);
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_chasePos = _ctx.PR.Position; 
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_ctx.AI.maxSpeed = 1;
		}
	}
}
