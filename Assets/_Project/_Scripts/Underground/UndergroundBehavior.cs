using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class UndergroundBehavior : MonoBehaviour
    {
        private Action _destroyAction;
        private bool _applicationQuitting;

        public void RegisterAsset(Action onDestroy)
        {
            _destroyAction = onDestroy;
        }

        private void OnDestroy()
        {
            if (_applicationQuitting) return;

            _destroyAction?.Invoke();
        }

        private void OnApplicationQuit()
        {
            _applicationQuitting = true;
        }
    }
}
