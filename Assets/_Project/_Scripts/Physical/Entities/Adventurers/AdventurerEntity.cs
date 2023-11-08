using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class AdventurerEntity : Entity
    {
        public string AdventurerName;
        public readonly int HashIdle = Animator.StringToHash("[Anm] AdventurerIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] AdventurerMove");
        public Action OnMove;
        public Seeker Seeker;
        public IAstarAI AI;

        private Animator _animator;
        private Bed _bed;
        private bool _canRegister = false;

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
            Seeker = GetComponent<Seeker>();
            _animator = transform.GetChild(0).GetComponent<Animator>();
        }

        private void OnEnable()
        {
            ChangeToIdleState(_animator);
            CheckForBed();
        }

        private void Start()
        {
            _canRegister = true;
        }

        protected override void Update()
        {
            base.Update();
            OnMove?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out Door door))
            {
                door.Open();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Door door))
            {
                door.Close();
            }
        }

        private void CheckForBed()
        {
            if (!_canRegister) return;

            if (_bed == null)
                KillEntity();
        }

        public void RegisterBed(Bed bed)
        {
            _bed = bed;
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
    }
}
