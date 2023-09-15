using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class Chest : Interactable
    {
        private Canvas _slotCanvas;

        public override void Awake()
        {
            base.Awake();
            _slotCanvas = transform.GetChild(2).GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            _pr.Inventory.InventoryControl.OnInventoryClosed += CloseChest;
        }

        private void OnDisable()
        {
            _pr.Inventory.InventoryControl.OnInventoryClosed -= CloseChest;
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => EnableChestSlots(false);
            EnableChestSlots(false);

            return base.Start();
        }

        private void OnDestroy()
        {
            EnableChestSlots(false);

            foreach (Transform transform in _slotCanvas.transform.GetChild(0))
            {
                Slot slot = transform.GetComponent<Slot>();
                if(slot.ItemObject != null)
                {
                    Vector3 offset = new(0.5f, 0.5f);
                    WorldItemManager.Instance.SpawnItem(this.transform.position += offset, slot.ItemObject, slot.InventoryItem.Count);
                }
            }
        }

        public override void Interact()
        {
            if (!_canInteract) return;

            HandleInventoryControl();
            EnableChestSlots(true);
        }

        private void CloseChest(object obj, EventArgs args)
        {
            EnableChestSlots(false);
        }

        public void EnableChestSlots(bool val)
        {
            _slotCanvas.gameObject.SetActive(val);
        }

        private void HandleInventoryControl()
        {
            _pr.Inventory.InventoryControl.ChestInteract(this);
        }
    }
}
