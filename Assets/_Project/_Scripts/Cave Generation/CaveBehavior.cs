using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class CaveBehavior : MonoBehaviour
    {
        private CaveLevel _caveLevel;

        public void Initialize()
        {
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
        }

        private void OnDestroy()
        {
            _caveLevel.TryToSpawnStairs(transform.position);
        }
    }
}
