using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CaveAscend : MonoBehaviour, IPointerClickHandler
    {
        private int _ascendLevelIndex = -1;
        private CaveLevel _level;

        public CaveLevel CaveLevel { get { return _level; } set { _level = value; } }

        private void Awake()
        {
            _ascendLevelIndex = CaveManager.Instance.PreviousLevelIndex;
            _level.EntranceSpawnPosition = transform.position + new Vector3(1.5f, 0f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_ascendLevelIndex == -1)
            {
                Debug.Log("Entering upperground");
            }
            else
            {
                CaveManager.Instance.TransitionToLevel(_ascendLevelIndex, _level.SpawnPosition);
            }
        }
    }
}
