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
        public GameObject root;
        public Transform rootPos;

        public readonly int HashIdle = Animator.StringToHash("[Anm] TreevilIdle");
        public readonly int HashMove = Animator.StringToHash("[Anm] TreevilMove");
        public readonly int HashAttack = Animator.StringToHash("[Anm] TreevilAttack");

        private float thornTimerCooldown;
        private float thornTimer;
        private int thornAttackNum;
        private float rootTimer;


        [SerializeField] private float _agroDistance;
        [SerializeField] private float _attackThornCooldown; // in seconds, boss waits this amount of time before next series of thorn attacks
        [SerializeField] private float _attackThornFrequency; // in seconds, how frequent attacks occur in a series of thorn attacks
        [SerializeField] private int _numThornAttacks; // num of thorn attacks per series of attacks

        [SerializeField] private float _attackRootCooldown; // in seconds, boss waits this amount of time before next root attack

        private string currentAttackType = "thorn";
        private List<string> attackTypes = new List<string> {"thorn", "root"};

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
                if (currentAttackType == "thorn")
                    ShootThorns();
                else if (currentAttackType == "root")
                    RootAttack();
            }
        }

        private void ShootThorns()
        {
            if (thornAttackNum >= _numThornAttacks)
            {
                thornTimerCooldown += Time.deltaTime;

                // After the Thorn Attack cooldown time has passed, the boss randomly switches its attack type
                if (thornTimerCooldown > _attackThornCooldown)
                {
                    thornAttackNum = 0;
                    thornTimerCooldown = 0;
                    currentAttackType = PickAttackType();
                }
            }
            else
            {
                thornTimer += Time.deltaTime;

                if (thornTimer > _attackThornFrequency)
                {
                    thornTimer = 0;
                    thornAttackNum++;
                    // shoot thorns
                    Instantiate(thorn, thornPosL.position, Quaternion.identity);
                    Instantiate(thorn, thornPosR.position, Quaternion.identity);
                }
            }
        }

        private void RootAttack()
        {
            if (rootTimer == 0)
                Instantiate(root, rootPos.position, Quaternion.identity);

            rootTimer += Time.deltaTime;

            // After the Root Attack cooldown time has passed, the boss randomly switches its attack type
            if (rootTimer > _attackRootCooldown)
            {
                rootTimer = 0;
                currentAttackType = PickAttackType();
            }
        }

        private string PickAttackType()
        {
            int randomIndex = UnityEngine.Random.Range(0, attackTypes.Count);
            return attackTypes[randomIndex];
        }
    }
}
