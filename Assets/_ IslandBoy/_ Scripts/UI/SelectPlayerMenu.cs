using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class SelectPlayerMenu : MonoBehaviour
	{
		[SerializeField] private PlayerObject _pr;
		[SerializeField] private GameObject selectMenu;

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			PauseGame();
		}

		public void PauseGame()
		{
			if(_pr.Inventory == null) return;
			if (_pr.Inventory.InventoryControl.IsInventoryOpen)
			{
				GameSignals.INVENTORY_CLOSE.Dispatch();
				return;
			}

			selectMenu.SetActive(true);
			GameSignals.DISABLE_PAUSE.Dispatch();
			GameSignals.GAME_PAUSED.Dispatch();
			Time.timeScale = 0;
		}

		public void SelectPlayer()
		{
			selectMenu.SetActive(false);
			GameSignals.ENABLE_PAUSE.Dispatch();
			GameSignals.GAME_UNPAUSED.Dispatch();
			Time.timeScale = 1;
		}
	}
}
