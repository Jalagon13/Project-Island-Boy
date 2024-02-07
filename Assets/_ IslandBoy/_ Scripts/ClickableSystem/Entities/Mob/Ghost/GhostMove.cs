using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace IslandBoy
{
    public class GhostMove : StateMachineBehaviour
    {
        private GhostStateManager _ctx;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Debug.Log("Entering CRAB Move State");
			_ctx = animator.transform.root.GetComponent<GhostStateManager>();
			_ctx.Seek(CalcWanderPos());
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_ctx.IsAgro)
			{
				_ctx.ChangeToChaseState(animator);
				return;
			}
			
			if (_ctx.AI.remainingDistance < 0.25f)
			{
				_ctx.ChangeToIdleState(animator);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		private Vector3 CalcWanderPos()
		{
			if(Vector2.Distance(_ctx.transform.position, _ctx.HomePosition) > 5)
				return _ctx.HomePosition;
			
			GraphNode startNode = AstarPath.active.GetNearest(_ctx.transform.position, NNConstraint.Default).node; 
 
			List<GraphNode> nodes = PathUtilities.BFS(startNode, 3); 
			Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0]; 
			
			return singleRandomPoint;
		}
    }
}
