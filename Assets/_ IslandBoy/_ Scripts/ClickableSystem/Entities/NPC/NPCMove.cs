using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (_ctx.AI.reachedDestination)
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

            calcWanderPos:

            GraphNode startNode = AstarPath.active.GetNearest(_ctx.transform.position, NNConstraint.Default).node;
            List<GraphNode> nodes = PathUtilities.BFS(startNode, 10);
            Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];

            var colliders = Physics2D.OverlapCircleAll(singleRandomPoint, 0.2f);

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out Door door))
                {
                    goto calcWanderPos;
                }
            }

            return singleRandomPoint;
        }
    }
}
