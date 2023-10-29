using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;

        private void Start()
        {
            transform.position = Vector3.zero;
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
