using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class MovingClouds : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        private Transform _left;
        private Transform _right;
        private float _restartPoint;
        private float _endPoint;

        private void Awake()
        {
            _left = transform.GetChild(0);
            _right = transform.GetChild(1);
        }

        private void Start()
        {
            _restartPoint = _right.position.x;
            _endPoint = _left.position.x;
        }

        private void FixedUpdate()
        {
            MoveCloud(_left);
            MoveCloud(_right);
        }

        private void MoveCloud(Transform cloud)
        {
            float x = cloud.position.x - _moveSpeed;
            if (x < -_endPoint)
            {
                x = _restartPoint;
            }
            cloud.position = new Vector3(x, transform.position.y, 0);
        }
    }
}
