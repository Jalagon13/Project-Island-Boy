using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class RusherEntity : Entity
    {
        public AudioClip AgroClip;

        [SerializeField] private float _agroDistance;

        public readonly int HashIdle = Animator.StringToHash("[Anm] RusherIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] RusherMove");
        public readonly int HashChase = Animator.StringToHash("[Anm] RusherChase");
        public Action OnMove;
        public Seeker Seeker;
        public IAstarAI AI;

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
            Seeker = GetComponent<Seeker>();
        }

        private void OnDisable()
        {
            Destroy(gameObject);
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

        public bool CanGetToPlayer()
        {
            Path path = ABPath.Construct(gameObject.transform.position, PR.Position);

            // WIP
            return true;
        }

        public bool IsStuck()
        {
            return AI.velocity.magnitude < 0.2f;
        }
    }
}