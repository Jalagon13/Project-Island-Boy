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
            GameSignals.DISPLAY_PROMPT.AddListener(DisplayPrompt);
        }

        private void OnDestroy()
        {
            GameSignals.DISPLAY_PROMPT.RemoveListener(DisplayPrompt);
        }

        private void Update()
        {
            if (_currentInteractable == null) return;

            if (!_currentInteractable.PlayerInRange(_currentInteractable.gameObject.transform.position + new Vector3(0.5f, 0.5f)))
            {
                InteractableHandle(null);
            }
        }

        public void DisplayPrompt(ISignalParameters parameters)
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
