using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ZombieMove : StateMachineBehaviour
    {
        private ZombieEntity _ctx;
        private Vector2 _wanderPos;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Entering Move State");
            _ctx = animator.transform.root.GetComponent<ZombieEntity>();
            _ctx.OnMove += Move;
            _wanderPos = CalcWanderPos();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_ctx.AI.reachedDestination)
            {
                _ctx.ChangeToIdleState(animator);
            }

            if (_ctx.PlayerClose())
            {
                _ctx.ChangeToChaseState(animator);
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
            GraphNode startNode = AstarPath.active.GetNearest(_ctx.transform.position, NNConstraint.Default).node;
            List<GraphNode> nodes = PathUtilities.BFS(startNode, 20);
            Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];

            return singleRandomPoint;
        }

    }
}
