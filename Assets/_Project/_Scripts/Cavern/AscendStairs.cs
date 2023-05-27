using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class AscendStairs : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || !_pr.PlayerInRange(transform.position + new Vector3(0.5f, 0.5f))) return;

            CaveFloor caveFloor = transform.parent.transform.parent.transform.GetComponent<CaveFloor>();
            caveFloor.CaveLevels.Ascend();
        }
    }
}
