using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IslandBoy
{
    public class Prompt : Interactable
    {
        [SerializeField] private Canvas _prompCanvas;
        [SerializeField] private UnityEvent _onOpenPrompt;
        [SerializeField] private UnityEvent _onClosePrompt;

        private void OnEnable()
        {
            GameSignals.INVENTORY_CLOSE.AddListener(OnInventoryClose);
        }

        private void OnDisable()
        {
            GameSignals.INVENTORY_CLOSE.RemoveListener(OnInventoryClose);
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => CloseUI();
            CloseUI();

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
            Signal signal = GameSignals.PROMPT_INTERACT;
            signal.ClearParameters();
            signal.AddParameter("Prompt", this);
            signal.Dispatch();
        }

        private void OnInventoryClose(ISignalParameters parameters)
        {
            CloseUI();
        }

        public void CloseUI()
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
            EnableYellowCorners(true);
            EnableInstructions(true);
        }
    }
}
