using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AscendStairs : MonoBehaviour, IPointerClickHandler
    {
        private int _ascendLevelIndex = -1;
        private CaveLevel _caveLevel;

        public void Initialize()
        {
            _ascendLevelIndex = CaveManager.Instance.PreviousLevelIndex;
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
            _caveLevel.EntranceSpawnPoint = transform.position + new Vector3(1.5f, 0f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_ascendLevelIndex == -1)
            {
                Debug.Log("Entering upperground");
            }
            else
            {
                CaveManager.Instance.TransitionToLevel(_ascendLevelIndex, false);
            }
        }
    }
}
