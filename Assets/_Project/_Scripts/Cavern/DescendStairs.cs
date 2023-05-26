using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class DescendStairs : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;

        private int _descendLevelIndex = -2;
        private CaveLevel _caveLevel;

        public void Initialize()
        {
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || !_pr.PlayerInRange(transform.position + new Vector3(0.5f, 0.5f))) return;

            if (_descendLevelIndex == -2)
                _descendLevelIndex = CaveManager.Instance.CreateNewLevel();

            _caveLevel.BackPoint = transform.position + new Vector3(0.5f, 0.5f);
            CaveManager.Instance.TransitionToLevel(_descendLevelIndex, true);
        }
    }
}
