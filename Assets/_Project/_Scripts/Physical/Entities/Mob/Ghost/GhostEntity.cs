using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class GhostEntity : Entity
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private float _agroDistance;

        public readonly int HashIdle = Animator.StringToHash("[Anm] GhostIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] GhostMove");
        public readonly int HashAgro = Animator.StringToHash("[Anm] GhostAgro");
        public Action OnMove;
        //public Seeker Seeker;
        //public IAstarAI AI;

        private bool _reachedDestination;

        public TilemapReferences TMR { get { return _tmr; } }
        public bool ReachedDestination { get { return _reachedDestination; } set { _reachedDestination = value; } }

        protected override void Awake()
        {
            base.Awake();
            //AI = GetComponent<IAstarAI>();
            //Seeker = GetComponent<Seeker>();
        }

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        protected void FixedUpdate()
        {
            OnMove?.Invoke();
        }

        public void Seek(Vector2 pos)
        {
            //AI.destination = pos;
            //AI.SearchPath();

            var step = _moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
            _reachedDestination = Vector3.Distance(transform.position, pos) < 0.1f;
        }

        public void ChangeToIdleState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, HashIdle);
        }

        public void ChangeToMoveState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, HashMove);
        }

        public void ChangeToAgroState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, HashAgro);
        }

        public bool PlayerClose()
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < _agroDistance;
        }

        //public bool CanGetToPlayer()
        //{
        //    Path path = ABPath.Construct(gameObject.transform.position, PR.Position);

        //    // WIP
        //    return true;
        //}

        //public bool IsStuck()
        //{
        //    return AI.velocity.magnitude < 0.2f;
        //}
    }
}
