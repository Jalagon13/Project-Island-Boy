using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class NPC : Prompt
	{
		[Header("NPC Parameters")]
		// [SerializeField] private Canvas _questCanvas;
		[SerializeField] private Canvas _shopCanvas;
		[SerializeField] private Canvas _upgradeCanvas;

		protected KnockbackFeedback _knockback;

		protected override void Awake()
		{
			base.Awake();

			_knockback = GetComponent<KnockbackFeedback>();
		}
		
		public void OpenQuestUI() // connected to quest button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(false);
			// _questCanvas.gameObject.SetActive(true);
			_upgradeCanvas.gameObject.SetActive(false);
		}
		
		public void OpenShopUI() // connected to shop button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(true);
			// _questCanvas.gameObject.SetActive(false);
			_upgradeCanvas.gameObject.SetActive(false);
		}
		
		public void OpenUpgradeUI() // connected to upgrade button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(false);
			// _questCanvas.gameObject.SetActive(false);
			_upgradeCanvas.gameObject.SetActive(true);
		}
		
		public void Close() => CloseUI(null);
		
		public override void CloseUI(ISignalParameters parameters)
		{
			base.CloseUI(parameters);
			
			// _questCanvas.gameObject.SetActive(false);
			_shopCanvas.gameObject.SetActive(false);
			_upgradeCanvas.gameObject.SetActive(false);
		}

		private void DisplayInteractable()
		{
			Signal signal = GameSignals.DISPLAY_INTERACTABLE;
			signal.ClearParameters();
			signal.AddParameter("Interactable", this);
			signal.Dispatch();
		}
		
		public override void ShowDisplay()
		{
			EnableInstructions(true);
			EnableAmountDisplay(false);
			EnableProgressBar(false);
		}
	}
}
