using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class IronAnvil : Interactable
    {
        private Canvas _tcCanvas;

        protected override void Awake()
        {
            base.Awake();

            _tcCanvas = transform.GetChild(3).GetComponent<Canvas>();
            GameSignals.INVENTORY_CLOSE.AddListener(CloseUI);
        }

        private void OnDestroy()
        {
            GameSignals.INVENTORY_CLOSE.RemoveListener(CloseUI);
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => DisableUI();
            DisableUI();

            return base.Start();
        }

        public override void Interact()
        {
            if (Pointer.IsOverUI()) return;

            EnableUI();
            EnableInstructions(false);
        }

        public override void ShowDisplay()
        {
            EnableInstructions(true);
        }

        private void CloseUI(ISignalParameters parameters)
        {
            DisableUI();
        }

        public void EnableUI()
        {
            DisplayInteractable();

            _tcCanvas.gameObject.SetActive(true);
        }

        public void DisableUI()
        {
            _tcCanvas.gameObject.SetActive(false);
        }

		private void DisplayInteractable()
		{
			Signal signal = GameSignals.DISPLAY_INTERACTABLE;
			signal.ClearParameters();
			signal.AddParameter("Interactable", this);
			signal.Dispatch();
		}
    }
}
