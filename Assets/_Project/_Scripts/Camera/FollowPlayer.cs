using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private Vector3 _position;

        private void LateUpdate()
        {
            SetPosition();
        }

        private void SetPosition()
        {
            _position = new(_pr.PlayerPosition.x, _pr.PlayerPosition.y, -10);
            transform.position = _position;
        }
    }
}
