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
        public Seeker Seeker;

        private float _despawnCd = 15f;
        private Timer _despawnTimer;

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
            Seeker = GetComponent<Seeker>();
            _despawnTimer = new(_despawnCd);
            _despawnTimer.OnTimerEnd += OnDespawnTimerEnd;
        }

        protected override void Update()
        {
            base.Update();
            OnMove?.Invoke();

            _despawnTimer.Tick(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out FrustumCollider fc))
            {
                _despawnTimer.IsPaused = true;
                _despawnTimer.RemainingSeconds = _despawnCd;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out FrustumCollider fc))
            {
                _despawnTimer.IsPaused = false;
                _despawnTimer.RemainingSeconds = _despawnCd;
            }
        }

        public void OnDespawnTimerEnd()
        {
            _despawnTimer.OnTimerEnd -= OnDespawnTimerEnd;
            Destroy(gameObject);
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

            return !path.error;
        }

        public bool IsStuck()
        {
            return AI.velocity.magnitude < 0.2f;
        }
    }
}
