using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class CraftStation : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private RecipeDatabaseObject _rdb;
        [SerializeField] private RuneDatabaseObject _adb;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right && _pr.PlayerInRange(transform.position))
            {
                PopulateCraftSlots();
            }
        }

        private void PopulateCraftSlots()
        {
            _pr.Inventory.InventoryControl.CraftStationInteract(_rdb, _adb);
        }
    }

    public class CraftStationEventArgs : EventArgs
    {
        public RecipeDatabaseObject RDB;
        public RuneDatabaseObject RuneDB;
    }
}
