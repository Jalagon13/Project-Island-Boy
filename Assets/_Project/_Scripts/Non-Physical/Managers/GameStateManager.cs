using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameObject Instantiate(GameObject o, Vector3 pos, Quaternion rot)
        {
            return Instantiate(o, pos, rot);
        }

        //List to store scenes in order of occurence
        [SerializeField]
        private List<string> m_AreaNames = new List<string>();
        //Separate variables for non-sequential scenes: Title, Win, and Lose
        [SerializeField]
        private string m_TitleMenuName;
        [SerializeField]
        private string m_SettingsName;
        [SerializeField]
        private string m_DieSceneName;

        //Tracks the one overall instance of GameStateManager
        private static GameStateManager _instance;

        /* This makes the pausing/pause menu an action, so that the GameStateManager can tell the action
        * to do all the UI work, and doesn't directly depend on an outside class.*/
        public static Action TogglePauseMenu { get; set; }
        //enums of the four current possible playstates
        enum GAMESTATE
        {
            MENU,
            PLAYING,
            PAUSED,
            GAMEOVER
        }

        //Variable to tracks the state the game is currently in
        private static GAMESTATE m_State;
    

        //Creates the static instance of the GameStateManager
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(this);
            }
        }

        /* First checks if there's functionality in the pause menu.
         * If not, doesn't need to continue. Then, checks for esc key
         * as long as pause Action is not null. Finally, performs
         * methods/actions for pausing and unpausing.*/
        private void Update()
        {
            if (TogglePauseMenu != null)
            {
                if (Keyboard.current[Key.Escape].wasPressedThisFrame)
                {
                    GameStateManager.TogglePause();
                }
            }
        }

        /* Sends the player to the first scene. Checks for at least one area
         * in the area scene list, then sends capybara to first (currently only) scene.*/
        public static void NewGame()
        {
            m_State = GAMESTATE.PLAYING;
            if (_instance.m_AreaNames.Count > 0)
            {
                SceneManager.LoadScene(_instance.m_AreaNames[0]);
            }
        }

        //Sends the player to the title. No saving functionality yet.
        //Also makes sure to disconnect user from a Photon room if they're in one.
        public static void ToTitle()
        {
            m_State = GAMESTATE.MENU;
            //FindObjectOfType<AudioManager>().Play("Theme Song");
            //FindObjectOfType<AudioManager>().Stop("Overworld Theme");
        
        }

        /*Checks if game is paused or playing, and switches to the opposite.
         * Only pauses time
         * if the game is singleplayer, otherwise it just displays the
         * pause menu on the local player's screen and doesn't pause time.*/
        public static void TogglePause()
        {
            TogglePauseMenu();
            if (m_State == GAMESTATE.PLAYING)
            {
                m_State = GAMESTATE.PAUSED;
                
                Time.timeScale = 0;
            }
            else if (m_State == GAMESTATE.PAUSED)
            {
                m_State = GAMESTATE.PLAYING;
                Time.timeScale = 1;
            }
        }

        public static void LoadDeath()
        {
            m_State = GAMESTATE.GAMEOVER;
            SceneManager.LoadScene(_instance.m_AreaNames[3]);
        }

        public static void SaveGame()
        {

        }
        public static void Settings()
        {
            
        }
    }
}
