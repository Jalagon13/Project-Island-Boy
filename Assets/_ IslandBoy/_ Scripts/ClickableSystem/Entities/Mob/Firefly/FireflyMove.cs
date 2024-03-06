using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FireflyMove : StateMachineBehaviour
    {
        private FireflyStateManager _ctx;
		private Vector2 _target;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log("Entering GHOST Move State");
			_ctx = animator.transform.root.GetComponent<FireflyStateManager>();
			_target = CalcWanderPos();
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var step = 1.5f * Time.deltaTime;
			_ctx.transform.position = Vector2.MoveTowards(_ctx.transform.position, _target, step);
			
			if (_ctx.Agitated)
			{
				_ctx.ChangeToChaseState(animator);
				return;
			}
			
			if (Vector3.Distance(_ctx.transform.position, _target) < 0.01f)
			{
				_ctx.ChangeToIdleState(animator);
				return;
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
		}

		private Vector3 CalcWanderPos()
		{
			if(Vector2.Distance(_ctx.transform.position, _ctx.HomePosition) > 7)
				return _ctx.HomePosition;
			
			var randomPoint = _ctx.transform.position + Random.insideUnitSphere * 5;
			
			// GraphNode startNode = AstarPath.active.GetNearest(_ctx.transform.position, NNConstraint.Default).node; 
 
			// List<GraphNode> nodes = PathUtilities.BFS(startNode, 5); 
			// Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0]; 
			
			return randomPoint;
		}
    }
}
