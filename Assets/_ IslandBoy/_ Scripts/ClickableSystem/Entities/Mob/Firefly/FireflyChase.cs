using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FireflyChase : StateMachineBehaviour
    {
        private FireflyStateManager _ctx;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("Entering AGRO State");
			_ctx = animator.transform.root.GetComponent<FireflyStateManager>();
			_ctx.StartCoroutine(AttackSequence());
			// SeekPlayer();

			// MMSoundManagerSoundPlayEvent.Trigger(_ctx._agroSound, MMSoundManager.MMSoundManagerTracks.Sfx, default, volume:0.25f);
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			
		}
		
		private IEnumerator AttackSequence()
		{
			// Debug.Log("Charging Attack...");
			_ctx.RigidBody.velocity = Vector2.zero;
			_ctx.ChargeFeedbacks?.PlayFeedbacks();
			
			yield return new WaitForSeconds(2.5f);
			
			// Debug.Log("Attack!");
			_ctx.ChargeFeedbacks?.StopFeedbacks();
			_ctx.AttackFeedbacks?.PlayFeedbacks();
			_ctx.ChargePlayer(CalculateChargeDirection(), CalculateForce());
			
			yield return new WaitForSeconds(Random.Range(1.75f, 2.25f));
			
			_ctx.transform.position = CalculateTpPosition();
			_ctx.StartCoroutine(AttackSequence());
		}
		
		private Vector2 CalculateTpPosition()
		{
			_ctx.TeleportFeedbacks?.PlayFeedbacks();
			return _ctx.PlayerObject.Position + Random.insideUnitCircle * 8;
		}
		
		private float CalculateForce()
		{
			var force = Vector2.Distance(_ctx.transform.position, _ctx.PlayerObject.Position) * 2f;
			
			if(force < 10)
				 force = 10;
				 
			return force;
		}
		
		private Vector2 CalculateChargeDirection()
		{
			Vector2 origin = _ctx.transform.position;
			Vector2 target = _ctx.PlayerObject.Position + Vector2.up * 0.5f;
			return (target - origin).normalized;
		}
    }
}
