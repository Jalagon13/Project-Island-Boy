using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PromptControl : MonoBehaviour
    {
        private Prompt _currentPrompt;

        private void Update()
        {
            if (_currentPrompt == null) return;

            if (!_currentPrompt.PlayerInRange(_currentPrompt.gameObject.transform.position))
            {
                PromptHandle(null);
            }
        }

        public void PromptInteract(Prompt adventurer)
        {
            PromptHandle(adventurer);
        }

        public void PromptHandle(Prompt adventurer)
        {
            if (_currentPrompt != null)
                _currentPrompt.OnPlayerExitRange?.Invoke();

            _currentPrompt = adventurer;
        }
    }
}
