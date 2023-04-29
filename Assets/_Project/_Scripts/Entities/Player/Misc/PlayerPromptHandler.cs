using UnityEngine;

namespace IslandBoy
{
    public class PlayerPromptHandler : MonoBehaviour
    {


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
            Debug.Log("Interacted");
        }
    }
}
