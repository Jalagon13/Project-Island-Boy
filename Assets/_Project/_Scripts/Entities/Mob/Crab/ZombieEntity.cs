using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class ZombieEntity : Entity
    {
        [SerializeField] private float _agroDistance;

        public readonly int HashIdle = Animator.StringToHash("[Anm] ZombieIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] ZombieMove");
        public readonly int HashChase = Animator.StringToHash("[Anm] ZombieChase");
        public Action OnMove;
        public IAstarAI AI;

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
        }

        protected override void Update()
        {
            base.Update();
            OnMove?.Invoke();
        }

        public void Seek(Vector2 pos)
        {
            AI.destination = pos;
            AI.SearchPath();
        }

        public void ChangeToIdleState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, HashIdle);
        }

        public void ChangeToMoveState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, HashMove);
        }

        public void ChangeToChaseState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, HashChase);
        }

        public bool PlayerClose()
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < _agroDistance;
        }

        public bool IsStuck()
        {
            return AI.velocity.magnitude < 0.2f;
        }
    }
}
