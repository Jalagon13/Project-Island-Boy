using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IslandBoy
{
	public class GhostIdle : StateMachineBehaviour
	{
		private GhostStateManager _ctx;
		private bool _stopStateChange;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("Entering GHOST Idle State");
			_stopStateChange = false;
			_ctx = animator.transform.root.GetComponent<GhostStateManager>();
			_ctx.StartCoroutine(IdleDuration(animator));
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.Agitated)
			{
				_ctx.ChangeToChaseState(animator);
				_stopStateChange = true;
				return;
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			
		}

		private IEnumerator IdleDuration(Animator animator)
		{
			yield return new WaitForSeconds(Random.Range(2f, 3f));
			
			if(!_stopStateChange) 
				_ctx.ChangeToMoveState(animator);
		}
	}
}
