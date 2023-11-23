using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TreevilStateManager : MonoBehaviour
    {

        [SerializeField] private AudioClip _spawnSound;
        public PlayerReference PR;
        public AudioClip _agroSound;
        public Action OnMove;
        //[HideInInspector]
        //public IAstarAI AI;

        public readonly int HashIdle = Animator.StringToHash("[Anm] TreevilIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] TreevilMove");
        //public readonly int HashAttack = Animator.StringToHash("[Anm] TreevilAttack");


        [SerializeField] private float _agroDistance;

        private void Awake()
        {
            AudioManager.Instance.PlayClip(_spawnSound, false, false);
            //AI = GetComponent<IAstarAI>();
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

        public void ChangeToAttackState(Animator animator)
        {
            //AnimStateManager.ChangeAnimationState(animator, HashAttack);
        }

        public bool PlayerClose()
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < _agroDistance;
        }

        /*public void Seek(Vector2 pos)
        {
            AI.destination = pos;
            AI.SearchPath();
        }

        public bool CanGetToPlayer()
        {
            Path path = ABPath.Construct(gameObject.transform.position, PR.Position);

            // WIP
            return true;
        }*/
    }
}
