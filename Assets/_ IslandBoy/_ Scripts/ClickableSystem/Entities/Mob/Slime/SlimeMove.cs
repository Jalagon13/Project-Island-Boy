using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace IslandBoy
{
	public class SlimeMove : StateMachineBehaviour
	{
		private SlimeStateManager _ctx;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Debug.Log("Entering Move State");
			_ctx = animator.transform.root.GetComponent<SlimeStateManager>();
			_ctx.Seek(CalcWanderPos());
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.AI.remainingDistance < 0.25f)
			{
				_ctx.ChangeToIdleState(animator);
			}

			if (_ctx.PlayerClose(_ctx.AgroDistance) && _ctx.CanGetToPlayer())
			{
				_ctx.ChangeToChaseState(animator);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		private Vector3 CalcWanderPos()
		{
			GraphNode startNode = AstarPath.active.GetNearest(_ctx.transform.position, NNConstraint.Default).node; 
 
			List<GraphNode> nodes = PathUtilities.BFS(startNode, 10); 
			Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0]; 
			
			return singleRandomPoint;
		}

	}
}
