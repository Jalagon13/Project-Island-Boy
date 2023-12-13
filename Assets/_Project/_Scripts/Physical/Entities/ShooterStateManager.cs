using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class ShooterStateManager : MonoBehaviour
    {
        public PlayerReference PR;
        public AudioClip _agroSound;
        public Action OnMove;
        [HideInInspector]
        public IAstarAI AI;

        public readonly int HashIdle = Animator.StringToHash("[Anm] ShooterIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] ShooterMove");
        public readonly int HashChase = Animator.StringToHash("[Anm] ShooterChase");

        public GameObject proj;
        public Transform projPos;

        private float cooldownTimer;
        private float attackTimer;
        private int attackNum;

        [SerializeField] private float _agroDistance;
        [SerializeField] private float _socialDistance; // How far away the shooter mob will keep itself from the player
        [SerializeField] private float _attackCooldown; // in seconds, mob waits this amount of time before next series of ranged attacks
        [SerializeField] private float _attackFrequency; // in seconds, how frequent attacks occur in a series of ranged attacks
        [SerializeField] private int _numAttacks; // MIN num of quick attacks per series of attacks

        public float AgroDistance { get { return _agroDistance; } set { _agroDistance = value; } }

        private void Awake()
        {
            AI = GetComponent<IAstarAI>();
        }
        private void Update()
        {
            OnMove?.Invoke();
        }

        public void FixedUpdate()
        {
            if (PlayerClose(_agroDistance))
                Shoot();
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
            if (PlayerClose(_socialDistance) && AI.canMove)
                AI.canMove = false;
            else if (!PlayerClose(_socialDistance) && !AI.canMove)
                AI.canMove = true;
            else
            {
                AI.destination = pos;
                AI.SearchPath();
            }
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

        private void Shoot()
        {
            if (attackNum >= _numAttacks)
            {
                cooldownTimer += Time.deltaTime;

                // After the Attack cooldown time has passed, the boss randomly switches its attack type
                if (cooldownTimer > _attackCooldown)
                {
                    attackTimer = 0;
                    attackNum = 0;
                    cooldownTimer = 0;
                }
            }
            else
            {
                attackTimer += Time.deltaTime;

                if (attackTimer > _attackFrequency)
                {
                    attackTimer = 0;
                    attackNum++;
                    // shoot thorns
                    Instantiate(proj, projPos.position, Quaternion.identity);
                }
            }
        }
    }
}
