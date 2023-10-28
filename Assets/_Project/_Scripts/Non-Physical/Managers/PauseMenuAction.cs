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

        private void OnEnable()
        {
            GameStateManager.TogglePauseMenu += TogglePauseMenu;
        }

        private void OnDisable()
        {
            GameStateManager.TogglePauseMenu -= TogglePauseMenu;
        }

        private void Start()
        {
            m_PauseMenuFeatures.SetActive(false);
        }

        //Makes sure GameObject exists first, then reactivates or deactivates the Pause Menu UI.
        private void TogglePauseMenu()
        {
            if (m_PauseMenuFeatures != null)
            {
                m_PauseMenuFeatures.SetActive(!m_PauseMenuFeatures.activeSelf);
            }
        }

        public void OnClickQuit()
        {
            GameStateManager.TogglePause();
            GameStateManager.ToTitle();
        }

        public void OnClickSettings()
        {
            GameStateManager.Settings();
        }

        public void OnClickSave()
        {
            GameStateManager.SaveGame();
        }

        public void OnClickBack()
        {
            GameStateManager.TogglePause();
        }
    }
}
