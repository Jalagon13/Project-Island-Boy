using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class CrabIdle : StateMachineBehaviour
	{
		private CrabStateManager _ctx;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Debug.Log("Entering CRAB Idle State");
			_ctx = animator.transform.root.GetComponent<CrabStateManager>();
			_ctx.AI.isStopped = true;
			_ctx.StartCoroutine(IdleDuration(animator));
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.IsAgro)
			{
			    _ctx.ChangeToChaseState(animator);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		private IEnumerator IdleDuration(Animator animator)
		{
			yield return new WaitForSeconds(Random.Range(1f, 3f));
			_ctx.AI.isStopped = false;
			_ctx.ChangeToMoveState(animator);
		}
	}
}
