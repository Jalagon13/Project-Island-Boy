using MoreMountains.Tools;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
	//enums of the four current possible playstates
	enum GAMESTATE
	{
		MENU,
		PLAYING,
		PAUSED
	}

	public class PauseMenuAction : MonoBehaviour
	{
		//Stores all features in a game obejct so it doesn't have to disable itself:
		[SerializeField] private PlayerObject _po;
		[SerializeField] private GameObject _pauseMenu;

		private PlayerInput _playerInput;
		private static GAMESTATE _state;
		private bool _quitGame;

		private void Awake()
		{
			_playerInput = new();
			_playerInput.Player.PauseMenu.started += TogglePause;
			_state = GAMESTATE.PLAYING;
			_quitGame = false;
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
            if (_po.Inventory.InventoryControl.IsInventoryOpen)
			{
				GameSignals.INVENTORY_CLOSE.Dispatch();
				return;
			}
			
			if (_state == GAMESTATE.PLAYING)
			{
				_state = GAMESTATE.PAUSED;
				_pauseMenu.SetActive(true);

				GameSignals.GAME_PAUSED.Dispatch();
				Time.timeScale = 0;
			}
			else if (_state == GAMESTATE.PAUSED)
			{
				OnClickBack2Pause();
				_state = GAMESTATE.PLAYING;
				_pauseMenu.SetActive(false);

				GameSignals.GAME_UNPAUSED.Dispatch();
				Time.timeScale = 1;
			}
		}

		public void ExitOrQuit(bool toggle)
		{
			// true to quit, false to exit to menu
			_quitGame = toggle;
		}

		public void OnClickVerifyMenu()
		{
            // Brings up menu asking if player is sure they want to exit/quit
            transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
		}

		public void OnClickBack2Pause()
		{
            // Brings main pause menu back
            transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
		}

		public void OnClickSettings()
		{
			Debug.Log("Settings button clicked");
		}

		public void OnClickSave()
		{
			Debug.Log("Save button clicked");
		}

		public void OnClickBack2Game()
		{
            TogglePause(new());
		}

		public void OnClickLeaveGame()
		{
			if (_quitGame)
			{
				Debug.Log("Quit game");
				Application.Quit();
			}
			else //exit to title
			{
				Debug.Log("Exited game");
				TogglePause(new());
                SceneManager.LoadScene("MainMenu");
			}
		}
    }
}
