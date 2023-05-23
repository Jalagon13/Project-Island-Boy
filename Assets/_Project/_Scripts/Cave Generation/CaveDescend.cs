using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CaveDescend : MonoBehaviour, IPointerClickHandler
    {
        private int _descendLevelIndex = -2;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_descendLevelIndex == -2)
            {
                _descendLevelIndex = CaveManager.Instance.CreateNewLevel();
                CaveManager.Instance.TransitionToLevel(_descendLevelIndex);
            }
            else
            {
                CaveManager.Instance.TransitionToLevel(_descendLevelIndex);
            }
        }
    }
}
