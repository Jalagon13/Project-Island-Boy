using UnityEngine;

namespace IslandBoy
{
    public class PlayerPromptHandler : MonoBehaviour
    {
        private PromptInteract _currentPromptInteract;

        private void OnEnable()
        {
            PromptInteract.PromptInterectEvent += PromptTracker;
        }

        private void OnDisable()
        {
            PromptInteract.PromptInterectEvent += PromptTracker;
        }

        private void PromptTracker(PromptInteract promptInteract)
        {
            if (_currentPromptInteract != promptInteract && _currentPromptInteract != null)
                _currentPromptInteract.ClosePrompt();

            _currentPromptInteract = promptInteract;
        }
    }
}
