using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PauseMenuManager : MonoBehaviour
    {
        //Pause Menu buttons' OnClick methods:

        /* Both methods un-pause the game so that it doesn't start up frozen.
         * They then send the user to the title. Both methods do the same thing right now, but
         * are separated because later on ToTitle() will be implemented with a bool telling the game
         * whether or not it needs to save.*/

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
