using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : Interactable
    {
        [SerializeField] private CraftingDatabaseObject _cdb;

        private bool _appQuitting;

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => RefreshCraftSlotsToDefault();

            return base.Start();
        }

        private void OnApplicationQuit()
        {
            _appQuitting = true;
        }

        private void OnDestroy()
        {
            if (_appQuitting) return;

            RefreshCraftSlotsToDefault();
        }

        private void RefreshCraftSlotsToDefault()
        {
            _po.Inventory.InventoryControl.RefreshCraftSlotsToDefault();
            GameSignals.CRAFT_STATION_INTERACT.Dispatch();
        }

        public override void Interact()
        {
            if (!_canInteract) return;
            _po.Inventory.InventoryControl.CraftStationInteract(this, _cdb);

            GameSignals.CRAFT_STATION_INTERACT.Dispatch();
            EnableInstructions(false);
        }

        public override void ShowDisplay()
        {
            EnableInstructions(true);
        }

        public override void HideDisplay()
        {
            EnableProgressBar(false);
            EnableAmountDisplay(false);
            EnableInstructions(false);
        }
    }
}
