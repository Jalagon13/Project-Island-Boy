using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SlimeStateManager : MonoBehaviour
    {
        public PlayerReference PR;
        public AudioClip _agroSound;
        public Action OnMove;
        [HideInInspector]
        public Seeker Seeker;
        public IAstarAI AI;

        public readonly int HashIdle = Animator.StringToHash("[Anm] RusherIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] RusherMove");
        public readonly int HashChase = Animator.StringToHash("[Anm] RusherChase");


        [SerializeField] private float _agroDistance;

        public float AgroDistance { get { return _agroDistance; } set { _agroDistance = value; } }

        private void Awake()
        {
            AI = GetComponent<IAstarAI>();
            Seeker = GetComponent<Seeker>();
        }
        private void Update()
        {
            OnMove?.Invoke();
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

        public void Seek(Vector2 pos)
        {
            AI.destination = pos;
            AI.SearchPath();
        }

        public bool PlayerClose(float distance)
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < distance;
        }

        public bool CanGetToPlayer()
        {
            Path path = ABPath.Construct(gameObject.transform.position, PR.Position);

            // WIP
            return true;
        }
    }
}
