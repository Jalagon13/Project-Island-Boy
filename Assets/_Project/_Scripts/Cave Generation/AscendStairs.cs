using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AscendStairs : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;

        private int _ascendLevelIndex = -1;
        private CaveLevel _caveLevel;

        public void Initialize()
        {
            _ascendLevelIndex = CaveManager.Instance.ActiveIndex;
            _caveLevel = transform.parent.transform.parent.transform.GetComponent<CaveLevel>();
            _caveLevel.EntranceSpawnPoint = transform.position + new Vector3(1.5f, 0f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || !_pr.PlayerInRange(transform.position + new Vector3(0.5f, 0.5f))) return;

            if (_ascendLevelIndex == -1)
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
