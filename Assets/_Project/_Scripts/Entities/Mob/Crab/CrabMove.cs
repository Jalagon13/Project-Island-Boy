using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CrabMove : StateMachineBehaviour
    {
        private CrabEntity _ctx;
        private Vector2 _wanderPos;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx = animator.transform.root.GetComponent<CrabEntity>();
            _ctx.OnMove += Move;
            CalcWanderPos();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_ctx.AI.reachedDestination)
                ChangeToIdleState(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _ctx.OnMove -= Move;
        }

        private void Move()
        {
            _ctx.Seek(_wanderPos);
        }

        private void CalcWanderPos()
        {
            var wanderDistance = Random.Range(1.25f, 2f);
            var wanderOffset = new Vector3(Random.Range(1f, 1.175f), Random.Range(-0.75f, 0.75f));

            if (Random.value > 0.5f)
                wanderOffset *= -1f;

            if (!_ctx.IslandTilemap.HasTile(Vector3Int.FloorToInt(_ctx.transform.position + (wanderOffset * wanderDistance))))
            {
                CalcWanderPos();
                return;
            }
            else
            {
                _wanderPos = _ctx.transform.position + (wanderOffset * wanderDistance);
            }
        }
        private void ChangeToIdleState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, _ctx.HashIdle);
        }
    }
}
