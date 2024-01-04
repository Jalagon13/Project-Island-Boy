using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class SlimeStateManager : MonoBehaviour
    {
        public PlayerObject PR;
        public AudioClip _agroSound;
        public Action OnMove;

        public readonly int HashIdle = Animator.StringToHash("[Anm] RusherIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] RusherMove");
        public readonly int HashChase = Animator.StringToHash("[Anm] RusherChase");


        [SerializeField] private float _agroDistance;

        public float AgroDistance { get { return _agroDistance; } set { _agroDistance = value; } }

        private void Awake()
        {
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
        }

        public bool PlayerClose(float distance)
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < distance;
        }

        public bool CanGetToPlayer()
        {

            // WIP
            return true;
        }
    }
}
