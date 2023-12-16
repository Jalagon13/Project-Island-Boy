using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PromptControl : MonoBehaviour
    {
        private Interactable _currentInteractable;

        private void Awake()
        {
            GameSignals.CHEST_INTERACT.AddListener(DisablePromptDisplay);
            GameSignals.CRAFT_STATION_INTERACT.AddListener(DisablePromptDisplay);
            GameSignals.PROMPT_INTERACT.AddListener(PromptInteract);
            //GameSignals.TIMED_CONVERTER_INTERACT.AddListener(TCInteract);
        }

        private void OnDestroy()
        {
            GameSignals.CHEST_INTERACT.RemoveListener(DisablePromptDisplay);
            GameSignals.CRAFT_STATION_INTERACT.RemoveListener(DisablePromptDisplay);
            GameSignals.PROMPT_INTERACT.RemoveListener(PromptInteract);
            //GameSignals.TIMED_CONVERTER_INTERACT.RemoveListener(TCInteract);
        }

        private void Update()
        {
            if (_currentInteractable == null) return;

            if (!_currentInteractable.PlayerInRange(_currentInteractable.gameObject.transform.position))
            {
                InteractableHandle(null);
            }
        }

        private void DisablePromptDisplay(ISignalParameters parameters)
        {
            InteractableHandle(null);
        }

        public void TCInteract(ISignalParameters parameters)
        {
            Interactable tc = (Interactable)parameters.GetParameter("TimedConverter");

            InteractableHandle(tc);
        }

        public void PromptInteract(ISignalParameters parameters)
        {
            Interactable prompt = (Interactable)parameters.GetParameter("Prompt");

            InteractableHandle(prompt);
        }

        public void InteractableHandle(Interactable interactable)
        {
            if (_currentInteractable != null)
                _currentInteractable.OnPlayerExitRange?.Invoke();

            _currentInteractable = interactable;
        }
    }
}
