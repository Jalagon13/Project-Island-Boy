using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TreevilShooting : MonoBehaviour
    {
        [SerializeField] private float _maxPlayerDistance;
        [SerializeField] private float _attackCooldown; // in seconds, boss waits this amount of time before next series of attacks
        [SerializeField] private float _attackFrequency; // in seconds, how frequent attacks occur in a series of attacks
        [SerializeField] private int _numAttacks; // num of attacks per series of attacks

        public GameObject thorn;
        public Transform thornPosL;
        public Transform thornPosR;
        public PlayerReference PR;

        private float timerCooldown;
        private float timer;
        private int attackNum;
        private bool _canShoot;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            _canShoot = true;
        }

        void FixedUpdate()
        {
            if (!_canShoot) return;

            float distance = Vector2.Distance(transform.position, PR.Position);

            if (distance < _maxPlayerDistance)
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
                        Shoot();
                    }
                }
            }
        }

        void Shoot()
        {
            Instantiate(thorn, thornPosL.position, Quaternion.identity);
            Instantiate(thorn, thornPosR.position, Quaternion.identity);
        }
    }
}
