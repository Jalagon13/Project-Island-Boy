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
            GameSignals.CHEST_INTERACT.AddListener(DisablePromptDisplay);
            GameSignals.CRAFT_STATION_INTERACT.AddListener(DisablePromptDisplay);
            GameSignals.PROMPT_INTERACT.AddListener(PromptInteract);
        }

        private void OnDestroy()
        {
            GameSignals.CHEST_INTERACT.RemoveListener(DisablePromptDisplay);
            GameSignals.CRAFT_STATION_INTERACT.RemoveListener(DisablePromptDisplay);
            GameSignals.PROMPT_INTERACT.RemoveListener(PromptInteract);
        }

        private void Update()
        {
            if (_currentPrompt == null) return;

            if (!_currentPrompt.PlayerInRange(_currentPrompt.gameObject.transform.position))
            {
                PromptHandle(null);
            }
        }

        private void DisablePromptDisplay(ISignalParameters parameters)
        {
            PromptHandle(null);
        }

        public void PromptInteract(ISignalParameters parameters)
        {
            Prompt prompt = (Prompt)parameters.GetParameter("Prompt");

            PromptHandle(prompt);
        }

        public void PromptHandle(Prompt adventurer)
        {
            if (_currentPrompt != null)
                _currentPrompt.OnPlayerExitRange?.Invoke();

            _currentPrompt = adventurer;
        }
    }
}
