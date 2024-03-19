using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace IslandBoy
{
	public class NPCMove : StateMachineBehaviour
	{
		private NPCStateManager _ctx;
		private Animator _animator;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Debug.Log("Entering Move State");
			_ctx = animator.transform.parent.GetComponent<NPCStateManager>();
			_animator = animator;
			_ctx.Seek(CalcWanderPos());
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.AI.reachedEndOfPath)
			{
				_ctx.ChangeToIdleState(animator);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			
		}
		
		private void SetToIdle()
		{
			_ctx.ChangeToIdleState(_animator);
		}

		private Vector2 CalcWanderPos()
		{
			if(Vector3.Distance(_ctx.transform.position, _ctx.HomePoint) > 5)
			{
				if(_ctx.HomePoint != new Vector2(999,999))
				{
					return _ctx.HomePoint;
				}
			}

			float radius = 4f;
			Vector3 randomDirection = Random.insideUnitSphere * radius;
			Vector3 origin = _ctx.gameObject.transform.position;
			randomDirection += origin;
			
			GraphNode nearestNode = AstarPath.active.GetNearest(randomDirection).node;
			if(nearestNode != null && nearestNode.Walkable)
			{
				Vector3 target = (Vector3)nearestNode.position;
			
				var colliders = Physics2D.OverlapCircleAll(target, 0.2f);
				foreach (var col in colliders)
				{
					if (col.TryGetComponent(out Door door))
					{
						return _ctx.HomePoint;
					}
				}
				//Debug.Log(Vector3.Distance(_ctx.transform.position, target));
				return target;
			}
			
			return _ctx.HomePoint;
		}
	}
}
