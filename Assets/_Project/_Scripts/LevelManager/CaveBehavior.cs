using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveBehavior : MonoBehaviour
    {
        private Action<Vector3> _destroyAction;

        public void Initialize(Action<Vector3> destroyAction)
        {
            _destroyAction = destroyAction;
        }

        public void DestroyByPlayerAction()
        {
            _destroyAction?.Invoke(transform.position);
        }
    }
}
