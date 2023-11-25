using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TreevilStateManager : MonoBehaviour
    {
        public PlayerReference PR;
        public AudioClip _agroSound;
        public Action OnMove;

        public GameObject thorn;
        public Transform thornPosL, thornPosR;

        public readonly int HashIdle = Animator.StringToHash("[Anm] TreevilIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] TreevilMove");
        public readonly int HashAttack = Animator.StringToHash("[Anm] TreevilAttack");

        private float timerCooldown;
        private float timer;
        private int attackNum;


        [SerializeField] private float _agroDistance;
        [SerializeField] private float _attackCooldown; // in seconds, boss waits this amount of time before next series of attacks
        [SerializeField] private float _attackFrequency; // in seconds, how frequent attacks occur in a series of attacks
        [SerializeField] private int _numAttacks; // num of attacks per series of attacks

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
            AnimStateManager.ChangeAnimationState(animator, HashAttack);
        }

        public bool PlayerClose()
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < _agroDistance;
        }

        public void FixedUpdate()
        {
            if (PlayerClose())
                ShootThorns();
        }

        private void ShootThorns()
        {
            if (attackNum >= _numAttacks)
            {
                timerCooldown += Time.deltaTime;

                if (timerCooldown > _attackCooldown)
                {
                    attackNum = 0;
                    timerCooldown = 0;
                }
            }
            else
            {
                timer += Time.deltaTime;

                if (timer > _attackFrequency)
                {
                    timer = 0;
                    attackNum++;
                    // shoot thorns
                    Instantiate(thorn, thornPosL.position, Quaternion.identity);
                    Instantiate(thorn, thornPosR.position, Quaternion.identity);
                }
            }
        }
    }
}
