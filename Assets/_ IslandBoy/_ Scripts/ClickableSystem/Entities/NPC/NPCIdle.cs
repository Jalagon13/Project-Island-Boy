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
			// Debug.Log("Entering Idle State");
			_ctx = animator.transform.parent.GetComponent<NPCStateManager>();
			_ctx.AI.isStopped = true;
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
			yield return new WaitForSeconds(Random.Range(8,16));
			_ctx.AI.isStopped = false;
			_ctx.ChangeToMoveState(animator);
		}
	}
}
