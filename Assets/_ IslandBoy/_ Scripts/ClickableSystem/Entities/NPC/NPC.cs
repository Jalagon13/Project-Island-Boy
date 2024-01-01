using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class NPC : Prompt
	{
		[Header("NPC Parameters")]
		[SerializeField] private Canvas _questCanvas;
		[SerializeField] private Canvas _shopCanvas;

		protected KnockbackFeedback _knockback;

		protected override void Awake()
		{
			base.Awake();

			_knockback = GetComponent<KnockbackFeedback>();
		}
		
		public void OpenQuestUI() // connected to quest button
		{
			DisplayInteractable();

			_questCanvas.gameObject.SetActive(true);
			_shopCanvas.gameObject.SetActive(false);
		}
		
		public void OpenShopUI() // connected to shop button
		{
			DisplayInteractable();

			_shopCanvas.gameObject.SetActive(true);
			_questCanvas.gameObject.SetActive(false);
		}
		
		public override void CloseUI(ISignalParameters parameters)
		{
			base.CloseUI(parameters);
			
			_questCanvas.gameObject.SetActive(false);
			_shopCanvas.gameObject.SetActive(false);
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
