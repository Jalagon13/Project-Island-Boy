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
            DispatchPromptInteract();
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

        private void CloseUI()
        {
            _prompCanvas.gameObject.SetActive(false);
            _onClosePrompt?.Invoke();
        }

        private void OpenUI()
        {
            _prompCanvas.gameObject.SetActive(true);
            _onOpenPrompt?.Invoke();
        }
    }
}
