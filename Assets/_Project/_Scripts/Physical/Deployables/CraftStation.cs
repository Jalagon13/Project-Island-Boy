using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : Interactable
    {
        [SerializeField] private RecipeDatabaseObject _rdb;

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
            _pr.Inventory.InventoryControl.RefreshCraftSlotsToDefault();
            GameSignals.CRAFT_STATION_INTERACT.Dispatch();
        }

        public override void Interact()
        {
            if (!_canInteract) return;
            _pr.Inventory.InventoryControl.CraftStationInteract(this, _rdb);

            GameSignals.CRAFT_STATION_INTERACT.Dispatch();
        }
    }
}
