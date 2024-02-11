using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace IslandBoy
{
	public class CrabChase : StateMachineBehaviour
	{
		private CrabStateManager _ctx;
		private Vector2 _chasePos;
		private float _chaseSpeed = 2.75f;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Debug.Log("Entering Chase State");
			_ctx = animator.transform.root.GetComponent<CrabStateManager>();
			_ctx.AI.maxSpeed = _chaseSpeed;
			_ctx.AI.isStopped = false;
			SeekPlayer();

			// MMSoundManagerSoundPlayEvent.Trigger(_ctx._agroSound, MMSoundManager.MMSoundManagerTracks.Sfx, default, volume:0.25f);
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(Vector2.Distance(_ctx.PR.Position, _ctx.transform.position) > 5)
			{
				_ctx.ChangeToMoveState(animator);
				return;
			}
			
			SeekPlayer();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_ctx.AI.maxSpeed = 1;
		}
		
		private void SeekPlayer()
		{
			_chasePos = _ctx.PR.Position; 
			_ctx.Seek(_chasePos);
		}
	}
}
