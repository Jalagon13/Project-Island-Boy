using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FireflyIdle : StateMachineBehaviour
    {
        private FireflyStateManager _ctx;
		private bool _stopStateChange;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("Entering Firefly Idle State");
			_stopStateChange = false;
			_ctx = animator.transform.root.GetComponent<FireflyStateManager>();
			_ctx.StartCoroutine(IdleDuration(animator));
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.Agitated)
			{
				_stopStateChange = true;
				_ctx.ChangeToChaseState(animator);
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
