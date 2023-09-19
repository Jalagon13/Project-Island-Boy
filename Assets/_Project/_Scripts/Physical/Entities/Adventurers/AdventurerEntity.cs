using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class AdventurerEntity : Entity
    {
        public readonly int HashIdle = Animator.StringToHash("[Anm] AdventurerIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] AdventurerMove");
        public Action OnMove;
        public Seeker Seeker;
        public IAstarAI AI;

        private SpriteRenderer _selectedIndicator;
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
            Seeker = GetComponent<Seeker>();
            _animator = transform.GetChild(0).GetComponent<Animator>();
            _selectedIndicator = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            HideSelectIndicator();
            ChangeToIdleState(_animator);
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

        public void ShowSelectIndicator()
        {
            _selectedIndicator.enabled = true;
        }

        public void HideSelectIndicator()
        {
            _selectedIndicator.enabled = false;
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

        public void EnterUnderground() // connected to prompt
        {
            PR.Inventory.InventoryControl.CloseInventory();
            LevelManager.Instance.LoadUnderground();
        }
    }
}
