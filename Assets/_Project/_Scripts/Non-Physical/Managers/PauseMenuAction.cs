using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PauseMenuAction : MonoBehaviour
    {
        //Stores all features in a game obejct so it doesn't have to disable itself:
        [SerializeField]
        private GameObject m_PauseMenuFeatures;

        //This class adds its own method to the GameStateManager's pause Action
        void Awake()
        {
            GameStateManager.TogglePauseMenu += TogglePauseMenu;
        }

        //Makes sure GameObject exists first, then reactivates or deactivates the Pause Menu UI.
        private void TogglePauseMenu()
        {
            if (m_PauseMenuFeatures != null)
            {
                m_PauseMenuFeatures.SetActive(!m_PauseMenuFeatures.activeSelf);
            }
        }
    }
}
