using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveBehavior : MonoBehaviour
    {
        private CaveLevel _caveLevel;
        private bool _isQuitting;

        public void Initialize()
        {
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
        }

        private void OnDestroy()
        {
            if (_isQuitting) return;

            _caveLevel.TryToSpawnStairs(transform.position);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }
}
