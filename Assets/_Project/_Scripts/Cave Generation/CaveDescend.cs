using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CaveDescend : MonoBehaviour, IPointerClickHandler
    {
        private int _descendLevelIndex = -2;
        private CaveLevel _level;

        public CaveLevel CaveLevel {set { _level = value; } }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;

            if (_descendLevelIndex == -2)
                _descendLevelIndex = CaveManager.Instance.CreateNewLevel();

            _level.SpawnPosition = transform.position + new Vector3(0.5f, 0.5f);
            CaveManager.Instance.TransitionToLevel(_descendLevelIndex, _level.EntranceSpawnPosition);
        }
    }
}
