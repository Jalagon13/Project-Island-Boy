using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class Chest : Interactable
    {
        [SerializeField] private List<ChestInvSlot> _presetItems = new(); // need to build functionality to populate chests with preset items.
        private Canvas _slotCanvas;

        public override void Awake()
        {
            base.Awake();
            _slotCanvas = transform.GetChild(2).GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            GameSignals.INVENTORY_CLOSE.AddListener(CloseChest);
            //_pr.Inventory.InventoryControl.OnInventoryClosed += CloseChest;
        }

        private void OnDisable()
        {
            GameSignals.INVENTORY_CLOSE.RemoveListener(CloseChest);
            //_pr.Inventory.InventoryControl.OnInventoryClosed -= CloseChest;
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => EnableChestSlots(false);
            FillWithPresetItems();
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

        private void FillWithPresetItems()
        {
            var slotTransform = _slotCanvas.transform.GetChild(0);
            int counter = 0;

            _presetItems.ForEach((chestInvSlot) => 
            {
                var chestSlot = slotTransform.GetChild(counter).GetComponent<Slot>();
                chestSlot.SpawnInventoryItem(chestInvSlot.OutputItem, chestInvSlot.OutputItem.DefaultParameterList, chestInvSlot.OutputAmount);
                counter++;
            });
        }

        public override void Interact()
        {
            if (!_canInteract) return;
            HandleInventoryControl();
            EnableChestSlots(true);
        }

        private void CloseChest(ISignalParameters parameters)
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

    [Serializable]
    public class ChestInvSlot
    {
        public ItemObject OutputItem;
        public int OutputAmount;
    }
}
