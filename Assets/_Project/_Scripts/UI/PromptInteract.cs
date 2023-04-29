using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class PromptInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorManager.Instance.SetPromptCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorManager.Instance.SetDefaultCursor();
        }
    }
}
