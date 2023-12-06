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
        private bool _appQuitting;

        protected override void Awake()
        {
            base.Awake();
            _slotCanvas = transform.GetChild(3).GetComponent<Canvas>();

            GameSignals.INVENTORY_CLOSE.AddListener(CloseChest);
            
            GameSignals.ADD_ITEMS_TO_CHEST.AddListener(AddItemsToChest); // BROOKE --------------------------------------------------
            foreach (Transform transform in _slotCanvas.transform.GetChild(0))
            {
                Slot chestSlot = transform.GetComponent<Slot>();
                chestSlot.SetChestSlot();
            }                                                           // BROOKE --------------------------------------------------
        }

        private void OnDestroy()
        {
            GameSignals.INVENTORY_CLOSE.RemoveListener(CloseChest);
            GameSignals.ADD_ITEMS_TO_CHEST.RemoveListener(AddItemsToChest); // BROOKE

            if (_appQuitting) return;

            EnableChestSlots(false);

            foreach (Transform transform in _slotCanvas.transform.GetChild(0))
            {
                Slot slot = transform.GetComponent<Slot>();
                if (slot.ItemObject != null)
                {
                    Vector3 offset = new(0.5f, 0.5f);
                    GameAssets.Instance.SpawnItem(this.transform.position + offset, slot.ItemObject, slot.InventoryItem.Count);
                }
            }
        }

        public override IEnumerator Start()
        {
            OnPlayerExitRange += () => EnableChestSlots(false);
            FillWithPresetItems();
            EnableChestSlots(false);

            return base.Start();
        }

        private void OnApplicationQuit()
        {
            _appQuitting = true;
        }

        private void FillWithPresetItems() // BROOKE --------------------------------------------------
        {
            AddItemsToChest(_presetItems);
        }

        private void AddItemsToChest(ISignalParameters parameters)
        {
            Debug.Log("signal for additemstochest");
            // if item was added successfully, delete item from inventory
            if (AddItemsToChest(parameters.GetParameter("itemsToAdd") as List<ChestInvSlot>))
            {
                Destroy(parameters.GetParameter("itemObj") as GameObject);
            }
            // TODO: don't play sound or play error sound if wasn't able to add item
        }

        private bool AddItemsToChest(List<ChestInvSlot> itemsToAdd)
        {
            // return true if all items were added successfully
            // false otherwise
            // only matters for when adding items in game via shift-click, does not matter otherwise
            foreach (ChestInvSlot item in itemsToAdd)
            {
                if (AddItem(item) != 0)
                    return false;
            }
            return true;
        }

        private int AddItem(ChestInvSlot itemToAdd)
        {
            // If stackable, check if any slot has the same item
            if (itemToAdd.OutputItem.Stackable == true)
            {
                foreach (Transform transform in _slotCanvas.transform.GetChild(0))
                {
                    Slot chestSlot = transform.GetComponent<Slot>();

                    if (chestSlot.ItemObject != null && chestSlot.ItemObject == itemToAdd.OutputItem)
                    {
                        if (chestSlot.InventoryItem.Count < chestSlot.GetMaxStack())
                        {
                            var count = chestSlot.InventoryItem.Count + itemToAdd.OutputAmount;

                            if (count <= chestSlot.GetMaxStack())
                            {
                                chestSlot.SetCount(count);
                                return 0;
                            }
                            else
                            {
                                chestSlot.SetCount(chestSlot.GetMaxStack());
                                itemToAdd.OutputAmount = count - chestSlot.GetMaxStack();
                            }
                        }
                    }
                }
            }

            // Find an empty slot
            foreach (Transform transform in _slotCanvas.transform.GetChild(0))
            {
                Slot chestSlot = transform.GetComponent<Slot>();

                if (chestSlot.ItemObject == null)
                {
                    chestSlot.SpawnInventoryItem(itemToAdd.OutputItem, itemToAdd.OutputItem.DefaultParameterList, itemToAdd.OutputAmount);
                    return 0;
                }
            }

            return itemToAdd.OutputAmount;
        }
        // BROOKE --------------------------------------------------

        public override void Interact()
        {
            if (!_canInteract) return;

            _instructions.gameObject.SetActive(false);
            DispatchChestInteract();
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

        private void DispatchChestInteract()
        {
            Signal signal = GameSignals.CHEST_INTERACT;
            signal.ClearParameters();
            signal.AddParameter("ChestInteract", this);
            signal.Dispatch();
        }

        public override void ShowDisplay()
        {
            _yellowCorners.gameObject.SetActive(true);
            _instructions.gameObject.SetActive(true);
        }
    }

    [Serializable]
    public class ChestInvSlot
    {
        public ItemObject OutputItem;
        public int OutputAmount;
    }
}
