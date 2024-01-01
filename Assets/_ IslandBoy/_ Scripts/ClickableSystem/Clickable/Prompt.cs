using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
	public class Prompt : Interactable
	{
		[Header("Base Prompt Parameters")]
		[SerializeField] private Canvas _prompCanvas;
		[SerializeField] private UnityEvent _onOpenPrompt;
		[SerializeField] private UnityEvent _onClosePrompt;

		private void OnEnable()
		{
			GameSignals.INVENTORY_CLOSE.AddListener(CloseUI);
			GameSignals.GAME_PAUSED.AddListener(CloseUI);
		}

		private void OnDisable()
		{
			GameSignals.INVENTORY_CLOSE.RemoveListener(CloseUI);
			GameSignals.GAME_PAUSED.RemoveListener(CloseUI);
		}

		public override IEnumerator Start()
		{
			OnPlayerExitRange += () => CloseUI(null);
			CloseUI(null);

			yield return base.Start();
		}

		public override void Interact()
		{
			if (Pointer.IsOverUI()) return;

			EnableInstructions(false);
			OpenUI();
		}

		private void DispatchPromptInteract()
		{
			Signal signal = GameSignals.DISPLAY_PROMPT;
			signal.ClearParameters();
			signal.AddParameter("Prompt", this);
			signal.Dispatch();
		}

		public virtual void CloseUI(ISignalParameters parameters)
		{
			_prompCanvas.gameObject.SetActive(false);
			_onClosePrompt?.Invoke();
		}

		public void OpenUI()
		{
			DispatchPromptInteract();

			_prompCanvas.gameObject.SetActive(true);
			_onOpenPrompt?.Invoke();
		}

		protected override void EnableInstructions(bool _)
		{
			_instructions.SetActive(_);
		}

		public override void ShowDisplay()
		{
			EnableInstructions(true);
		}
	}
}
