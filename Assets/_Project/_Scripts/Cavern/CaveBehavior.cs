using UnityEngine;

namespace IslandBoy
{
    public class CaveBehavior : MonoBehaviour
    {
        private CaveLevel _caveLevel;

        private void OnDestroy()
        {
            _caveLevel.TryToSpawnStairs(transform.position);
        }

        public void Initialize()
        {
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
        }
    }
}
