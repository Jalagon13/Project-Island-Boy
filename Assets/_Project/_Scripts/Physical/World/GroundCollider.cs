using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    // dynamicalls alters pathfinding area
    public class GroundCollider : MonoBehaviour
    {
        private bool _applicationQuitting;

        private void OnEnable()
        {
            UpdatePathfinding();
        }

        private void OnDestroy()
        {
            if (_applicationQuitting) return;

            UpdatePathfinding();
        }

        private void OnApplicationQuit()
        {
            _applicationQuitting = true;
        }

        private void UpdatePathfinding()
        {
            AstarManager.Instance.UpdateGrid(transform.parent.position);
        }
    }
}
