using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Prompt : Interactable
    {
        [SerializeField] private Canvas _prompCanvas;

        private void OnEnable()
        {
            _pr.Inventory.InventoryControl.OnInventoryClosed += CloseUI;
        }

        private void OnDisable()
        {
            _pr.Inventory.InventoryControl.OnInventoryClosed -= CloseUI;
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => CloseUI();
            CloseUI();

            yield return base.Start();
        }

        public override void Interact()
        {
            _pr.Inventory.PromptControl.PromptInteract(this);
            OpenUI();
        }

        private void CloseUI(object obj = null, EventArgs args = null)
        {
            _prompCanvas.gameObject.SetActive(false);
        }

        private void OpenUI()
        {
            _prompCanvas.gameObject.SetActive(true);
        }
    }
}
