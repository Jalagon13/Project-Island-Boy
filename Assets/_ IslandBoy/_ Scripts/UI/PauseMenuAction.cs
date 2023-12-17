using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    //enums of the four current possible playstates
    enum GAMESTATE
    {
        MENU,
        PLAYING,
        PAUSED,
        GAMEOVER
    }

    public class PauseMenuAction : MonoBehaviour
    {
        //Stores all features in a game obejct so it doesn't have to disable itself:
        [SerializeField] private GameObject _pauseMenu;

        private PlayerInput _playerInput;
        private static GAMESTATE _state;

        private void Awake()
        {
            _playerInput = new();
            _playerInput.Player.PauseMenu.started += TogglePause;
            _state = GAMESTATE.PLAYING;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        private void Start()
        {
            _pauseMenu.SetActive(false);
        }

        public void TogglePause(InputAction.CallbackContext ctx)
        {
            if (_state == GAMESTATE.PLAYING)
            {
                _state = GAMESTATE.PAUSED;
                _pauseMenu.SetActive(true);

                GameSignals.GAME_PAUSED.Dispatch();
                Time.timeScale = 0;
            }
            else if (_state == GAMESTATE.PAUSED)
            {
                _state = GAMESTATE.PLAYING;
                _pauseMenu.SetActive(false);

                GameSignals.GAME_UNPAUSED.Dispatch();
                Time.timeScale = 1;
            }
        }

        public void OnClickTitle()
        {
            Debug.Log("Quit button clicked");
            Application.Quit();
        }

        public void OnClickSettings()
        {
            Debug.Log("Settings button clicked");
        }

        public void OnClickSave()
        {
            Debug.Log("Save button clicked");
        }

        public void OnClickBack()
        {
            TogglePause(new());
        }
    }
}
