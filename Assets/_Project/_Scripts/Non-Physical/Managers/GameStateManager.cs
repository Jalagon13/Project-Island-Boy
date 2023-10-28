using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        //enums of the four current possible playstates
        enum GAMESTATE
        {
            MENU,
            PLAYING,
            PAUSED,
            GAMEOVER
        }

        public static GameObject Instantiate(GameObject o, Vector3 pos, Quaternion rot)
        {
            return Instantiate(o, pos, rot);
        }

        //List to store scenes in order of occurence
        [SerializeField] private List<string> m_AreaNames = new List<string>();
        //Separate variables for non-sequential scenes: Title, Win, and Lose
        [SerializeField] private string m_TitleMenuName;
        [SerializeField] private string m_SettingsName;
        [SerializeField] private string m_DieSceneName;

        /* This makes the pausing/pause menu an action, so that the GameStateManager can tell the action
        * to do all the UI work, and doesn't directly depend on an outside class.*/
        public static Action TogglePauseMenu { get; set; }

        //Variable to tracks the state the game is currently in
        private static GAMESTATE m_State;
        private static PlayerInput _playerInput;

        protected override void Awake()
        {
            base.Awake();
            _playerInput = new();
            _playerInput.Player.PauseMenu.started += PauseInput;
            m_State = GAMESTATE.PLAYING;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        /* Sends the player to the first scene. Checks for at least one area
         * in the area scene list, then sends capybara to first (currently only) scene.*/
        public static void NewGame()
        {
            m_State = GAMESTATE.PLAYING;
            if (Instance.m_AreaNames.Count > 0)
            {
                SceneManager.LoadScene(Instance.m_AreaNames[0]);
            }
        }

        //Sends the player to the title. No saving functionality yet.
        //Also makes sure to disconnect user from a Photon room if they're in one.
        public static void ToTitle()
        {
            //m_State = GAMESTATE.MENU;
            //FindObjectOfType<AudioManager>().Play("Theme Song");
            //FindObjectOfType<AudioManager>().Stop("Overworld Theme");
        
        }

        private void PauseInput(InputAction.CallbackContext ctx)
        {
            TogglePause();
        }

        /*Checks if game is paused or playing, and switches to the opposite.
         * Only pauses time
         * if the game is singleplayer, otherwise it just displays the
         * pause menu on the local player's screen and doesn't pause time.*/
        public static void TogglePause()
        {
            TogglePauseMenu?.Invoke();

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
            SceneManager.LoadScene(Instance.m_AreaNames[3]);
        }

        public static void SaveGame()
        {

        }

        public static void Settings()
        {
            
        }
    }
}
