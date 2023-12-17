using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerObject _pr;

        private void Start()
        {
            _pr.Position = Vector3.zero;
        }

        private void LateUpdate()
        {
            SetPosition();
        }

        private void SetPosition()
        {
            transform.position = new(_pr.Position.x, _pr.Position.y, -10);
        }
    }
}
