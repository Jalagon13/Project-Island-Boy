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
            EnableYellowCorners(true);
            EnableInstructions(true);
        }

        private void CloseUI(ISignalParameters parameters)
        {
            DisableUI();
        }

        public void EnableUI()
        {
            DispatchTcSignal();

            _tcCanvas.gameObject.SetActive(true);
        }

        public void DisableUI()
        {
            _tcCanvas.gameObject.SetActive(false);
        }

        private void DispatchTcSignal()
        {
            Signal signal = GameSignals.TIMED_CONVERTER_INTERACT;
            signal.ClearParameters();
            signal.AddParameter("TimedConverter", this);
            signal.Dispatch();
        }
    }
}
