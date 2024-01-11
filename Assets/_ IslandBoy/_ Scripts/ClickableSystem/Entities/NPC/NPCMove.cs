using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IslandBoy
{
	public class NPCMove : StateMachineBehaviour
	{
		private NPCStateManager _ctx;
		private Vector2 _wanderPos;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//Debug.Log("Entering Move State");
			_ctx = animator.transform.parent.GetComponent<NPCStateManager>();
			_ctx.OnMove += Move;
			_wanderPos = CalcWanderPos();
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.Agent.remainingDistance < 0.25f)
			{
				_ctx.ChangeToIdleState(animator);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_ctx.OnMove -= Move;
		}

		private void Move()
		{
			_ctx.Seek(_wanderPos);
		}

		private Vector2 CalcWanderPos()
		{
			if(Vector3.Distance(_ctx.transform.position, _ctx.HomePoint) > 5)
			{
				return _ctx.HomePoint;
			}

			float radius = 4f;
			Vector3 randomDirection = Random.insideUnitSphere * radius;
			Vector3 origin = _ctx.gameObject.transform.position;
			randomDirection += origin;
			
			NavMeshHit navMeshHit;

			// Find the nearest point on the NavMesh within the specified radius
			if (NavMesh.SamplePosition(randomDirection, out navMeshHit, radius, -1))
			{
				Vector3 target = navMeshHit.position;
				var colliders = Physics2D.OverlapCircleAll(target, 0.2f);

				foreach (var col in colliders)
				{
					if (col.TryGetComponent(out Door door))
					{
						return _ctx.HomePoint;
					}
				}

				return target;
			}
			
			return _ctx.HomePoint;
		}
	}
}
