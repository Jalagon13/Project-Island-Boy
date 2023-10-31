using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CollectCollider : MonoBehaviour
    {
        private Collider2D _collectCollider;

        private void Awake()
        {
            _collectCollider = GetComponent<Collider2D>();
            GameSignals.PLAYER_DIED.AddListener(DisableCollider);
            GameSignals.DAY_START.AddListener(EnableCollider);
        }

        private void OnDestroy()
        {
            GameSignals.PLAYER_DIED.RemoveListener(DisableCollider);
            GameSignals.DAY_START.RemoveListener(EnableCollider);
        }

        private void DisableCollider(ISignalParameters parameters)
        {
            _collectCollider.enabled = false;
        }

        private void EnableCollider(ISignalParameters parameters)
        {
            _collectCollider.enabled = true;
        }
    }
}
