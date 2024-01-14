using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TreevilBranch : MonoBehaviour
    {
        public GameObject pivot;

        [SerializeField] private int _damageAmount;
        [SerializeField] private float _extendLength; // in seconds, how long it takes the branch to extend before attacking
        [SerializeField] private float _sweepAttackLength; // in seconds, how long this sweep attack lasts
        [SerializeField] private float _rotationSpeed; // adjust the speed of rotation
        [SerializeField] private Quaternion _initialRotation; // initial rotation

        private float timer;
        private bool isSwinging;
        private int rotationDirection;

        void Start()
        {
            transform.rotation = _initialRotation;
            rotationDirection = RotationDirection();
        }

        void FixedUpdate()
        {
            timer += Time.deltaTime;

            // Maybe implement branch appearing animation in future
            if (!isSwinging)
            {
                if (timer > _extendLength)
                {
                    isSwinging = true;
                    timer = 0;
                }
            }
            else
            {
                if (timer > _sweepAttackLength)
                {
                    Destroy(gameObject);
                }

                transform.RotateAround(pivot.transform.position, new Vector3(0, 0, rotationDirection), _rotationSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // Doesn't start damaging player until branch starts swinging
            if (isSwinging && collision.TryGetComponent(out Player player))
            {
                Vector2 damagerPosition = transform.root.gameObject.transform.position;
                player.Damage(_damageAmount, damagerPosition);
            }
        }


        private int RotationDirection()
        {
            // Randomly decides which direction to pivot around
            int randomIndex = UnityEngine.Random.Range(0, 2);
            if (randomIndex == 0)
                return -1;
            return randomIndex;
        }
    }
}
