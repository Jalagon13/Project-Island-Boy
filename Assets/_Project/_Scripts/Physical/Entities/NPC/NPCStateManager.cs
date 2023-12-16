using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class NPCStateManager : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private Action _onMove;
        private Seeker _seeker;
        private IAstarAI _ai;
        private Vector2 _homePoint;
        private readonly int _hashIdle = Animator.StringToHash("[Anm] NPCIdle");
        private readonly int _hashMove = Animator.StringToHash("[Anm] NPCMove");

        public IAstarAI AI { get { return _ai; } }
        public Action OnMove { get { return _onMove; } set { _onMove = value; } }
        public Vector2 HomePoint { get { return _homePoint; } }

        private void Awake()
        {
            _ai = GetComponent<IAstarAI>();
            _seeker = GetComponent<Seeker>();
            _homePoint = transform.position;
        }

        private void Update()
        {
            _onMove?.Invoke();
        }

        public void ChangeToIdleState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, _hashIdle);
        }

        public void ChangeToMoveState(Animator animator)
        {
            AnimStateManager.ChangeAnimationState(animator, _hashMove);
        }

        public void Seek(Vector2 pos)
        {
            _ai.destination = pos;
            _ai.SearchPath();
        }
    }
}
