using UnityEngine;

namespace IslandBoy
{
    public class CaveBehavior : MonoBehaviour
    {
        private CaveLevel _caveLevel;
        private bool _isQuitting;

        private void OnDestroy()
        {
            if (!_isQuitting)
            {
                _caveLevel.TryToSpawnStairs(transform.position);
            }
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public void Initialize()
        {
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
        }
    }
}
