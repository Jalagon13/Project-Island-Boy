using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : Interactable
    {
        [SerializeField] private RecipeDatabaseObject _rdb;
        [SerializeField] private RuneDatabaseObject _adb;

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => RefreshCraftSlotsToDefault();

            return base.Start();
        }

        private void OnDestroy()
        {
            RefreshCraftSlotsToDefault();
        }

        private void RefreshCraftSlotsToDefault()
        {
            _pr.Inventory.InventoryControl.RefreshCraftSlotsToDefault();
        }

        public override void Interact()
        {
            if (!_canInteract) return;

            _pr.Inventory.InventoryControl.CraftStationInteract(this, _rdb, _adb);
        }
    }

    public class CraftStationEventArgs : EventArgs
    {
        public RecipeDatabaseObject RDB;
        public RuneDatabaseObject RuneDB;
    }
}
