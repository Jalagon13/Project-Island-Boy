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
        public GameObject branch;
        public GameObject root;

        public Transform thornPosL, thornPosR;
        public Transform rootPos;

        public readonly int HashIdle = Animator.StringToHash("[Anm] TreevilIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] TreevilMove");
        public readonly int HashAttack = Animator.StringToHash("[Anm] TreevilAttack");

        private float quickAttackCooldownTimer;
        private float attackTimer;
        private int quickAttackNum;


        [SerializeField] private float _agroDistance;
        [SerializeField] private float _attackThornCooldown; // in seconds, boss waits this amount of time before next series of thorn attacks
        [SerializeField] private float _attackThornFrequency; // in seconds, how frequent attacks occur in a series of thorn attacks
        [SerializeField] private int _numQuickAttacksMin; // MIN num of quick attacks per series of attacks
        [SerializeField] private int _numQuickAttacksMax; // MAX num

        [SerializeField] private float _attackBranchCooldown; // in seconds, boss waits this amount of time before next branch attack
        [SerializeField] private float _attackRootCooldown; // in seconds, boss waits this amount of time before next root attack

        private int numQuickAttacks;
        private int attackType = 0;

        private void Start()
        {
            numQuickAttacks = UnityEngine.Random.Range(_numQuickAttacksMin, _numQuickAttacksMax);
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
            AnimStateManager.ChangeAnimationState(animator, HashAttack);
        }

        public bool PlayerClose()
        {
            return Vector3.Distance(gameObject.transform.position, PR.Position) < _agroDistance;
        }

        public void FixedUpdate()
        {
            if (PlayerClose())
            {
                switch (attackType)
                {
                    case 1:
                        RootAttack();
                        break;
                    case 2:
                        BranchAttack();
                        break;
                    default:
                        ShootThorns();
                        break;

                }
            }
        }

        private void ShootThorns()
        {
            if (quickAttackNum >= numQuickAttacks)
            {
                quickAttackCooldownTimer += Time.deltaTime;

                // After the Thorn Attack cooldown time has passed, the boss randomly switches its attack type
                if (quickAttackCooldownTimer > _attackThornCooldown)
                {
                    attackTimer = 0;
                    quickAttackNum = 0;
                    quickAttackCooldownTimer = 0;
                    numQuickAttacks = UnityEngine.Random.Range(_numQuickAttacksMin, _numQuickAttacksMax);
                    PickAttackType();
                }
            }
            else
            {
                attackTimer += Time.deltaTime;

                if (attackTimer > _attackThornFrequency)
                {
                    attackTimer = 0;
                    quickAttackNum++;
                    // shoot thorns
                    Instantiate(thorn, thornPosL.position, Quaternion.identity);
                    Instantiate(thorn, thornPosR.position, Quaternion.identity);
                }
            }
        }

        private void BranchAttack()
        {
            if (attackTimer == 0)
                Instantiate(branch, rootPos.position, Quaternion.identity);

            attackTimer += Time.deltaTime;

            // After the Branch Attack cooldown time has passed, the boss randomly switches its attack type
            if (attackTimer > _attackBranchCooldown)
            {
                attackTimer = 0;
                PickAttackType();
            }
        }

        private void RootAttack()
        {
            if (quickAttackNum >= numQuickAttacks)
            {
                quickAttackCooldownTimer += Time.deltaTime;

                // After the Root Attack cooldown time has passed, the boss randomly switches its attack type
                if (quickAttackCooldownTimer > _attackRootCooldown)
                {
                    attackTimer = 0;
                    quickAttackNum = 0;
                    quickAttackCooldownTimer = 0;
                    numQuickAttacks = UnityEngine.Random.Range(_numQuickAttacksMin, _numQuickAttacksMax);
                    PickAttackType();
                }
            }
            else
            {
                attackTimer += Time.deltaTime;

                // After the Root Attack cooldown time has passed, the boss randomly switches its attack type
                if (attackTimer > _attackRootCooldown)
                {
                    attackTimer = 0;
                    quickAttackNum++;
                    Instantiate(root, PR.Position, Quaternion.identity);
                }
            }
        }

        private void PickAttackType()
        {
            attackType = UnityEngine.Random.Range(0, 3);
        }
    }
}
