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

        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<IAstarAI>();
            Seeker = GetComponent<Seeker>();
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

        public void EnterUnderground() // connected to prompt
        {
            PR.Inventory.InventoryControl.CloseInventory();
            LevelManager.Instance.LoadUnderground();
        }
    }
}
