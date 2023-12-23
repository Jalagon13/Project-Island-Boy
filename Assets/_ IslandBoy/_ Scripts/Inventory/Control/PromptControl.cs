using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PromptControl : MonoBehaviour
    {
        private Prompt _currentPrompt;

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
            if (_currentPrompt == null) return;

            if (!_currentPrompt.PlayerInRange(_currentPrompt.gameObject.transform.position + new Vector3(0.5f, 0.5f)))
            {
                InteractableHandle(null);
            }
        }

        public void DisplayPrompt(ISignalParameters parameters)
        {
            Prompt prompt = (Prompt)parameters.GetParameter("Prompt");

            InteractableHandle(prompt);
        }

        public void InteractableHandle(Prompt interactable)
        {
            if (_currentPrompt != null)
                _currentPrompt.OnPlayerExitRange?.Invoke();

            _currentPrompt = interactable;
        }
    }
}
