using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class PromptInteract : MonoBehaviour, IPointerClickHandler
    {
        public static Action<PromptInteract> PromptInterectEvent;

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private RectTransform _promptCanvas;

        private bool _promptOn;
        private bool _canOpenPrompt;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.25f);
            _canOpenPrompt = true;
        }

        protected virtual void Update()
        {
            if (!_pr.PlayerInRange(transform.position) && _promptOn)
                ClosePrompt();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canOpenPrompt) return;

            if (eventData.button == PointerEventData.InputButton.Right && _pr.PlayerInRange(transform.position))
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

        public void ClosePrompt()
        {
            _promptOn = false;
            _promptCanvas.gameObject.SetActive(false);
        }
    }
}
