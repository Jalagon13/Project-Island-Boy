using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class PromptInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static Action<PromptInteract> PromptInterectEvent;

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private RectTransform _promptCanvas;

        private bool _hovering;
        private bool _promptOn;

        protected virtual void Update()
        {
            if (!_hovering) return;

            if (_pr.PlayerInRange(transform.position))
                CursorManager.Instance.SetPromptCursor();
            else
                CursorManager.Instance.SetPromptCursorTransparent();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _hovering = true;
            CursorManager.Instance.SetPromptCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hovering = false;
            CursorManager.Instance.SetDefaultCursor();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right && _pr.PlayerInRange(transform.position))
            {
                if (_promptOn)
                    ClosePrompt();
                else
                    OpenPrompt();
            }
        }

        private void OpenPrompt()
        {
            _promptOn = true;
            _promptCanvas.gameObject.SetActive(true);

            PromptInterectEvent?.Invoke(this);
        }

        private void ClosePrompt()
        {
            _promptOn = false;
            _promptCanvas.gameObject.SetActive(false);
        }
    }
}
