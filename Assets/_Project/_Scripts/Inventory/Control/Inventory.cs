using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class Inventory : MonoBehaviour
    {
        public EventHandler OnItemAdded; // connected to craftslot

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _inventoryItemPrefab;
        [SerializeField] private int _maxStack;
        [SerializeField] private UnityEvent<ItemObject, int> _onPickupItem;
        [SerializeField] private Slot[] _allSlots;

        private MouseItemHolder _mouseItemHolder;

        public Slot[] InventorySlots { get { return _allSlots; } }
        public MouseItemHolder MouseItemHolder { get { return _mouseItemHolder; } }
        public InventoryControl InventoryControl { get { return GetComponent<InventoryControl>(); } }
        public int MaxStack { get { return _maxStack; } }

        private void Awake()
        {
            _pr.Inventory = this;
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
        }

        public int AddItem(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
        {
            // return leftover if there is.
            for (int i = 0; i < amount; i++)
            {
                if (!Add(item, amount, itemParameters))
                {
                    int leftOver = amount - i;
                    OnItemAdded?.Invoke(this, EventArgs.Empty);
                    _onPickupItem?.Invoke(item, amount - leftOver);
                    return leftOver;
                }
            }

            OnItemAdded?.Invoke(this, EventArgs.Empty);
            _onPickupItem?.Invoke(item, amount);
            return 0;
        }

        private bool Add(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
        {
            // Check if any slot has the same item with count lower than max stack.
            for (int i = 0; i < _allSlots.Length; i++)
            {
                Slot slot = _allSlots[i];

                if (slot is not InventorySlot)
                    continue;

                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null &&
                    itemInSlot.Item == item &&
                    itemInSlot.Count < _maxStack &&
                    itemInSlot.Item.Stackable == true)
                {
                    itemInSlot.IncrementCount();
                    return true;
                }
            }

            // Find an empty slot
            for (int i = 0; i < _allSlots.Length; i++)
            {
                Slot slot = _allSlots[i];

                if (slot.transform.childCount == 0)
                {
                    SpawnInventoryItem(item, slot, itemParameters);
                    return true;
                }
            }

            return false;
        }

        public void RemoveItem(ItemObject item, int amount)
        {
            var counter = 0;

            for (int j = 0; j < _allSlots.Length; j++)
            {
                if (counter >= amount) return;

                Slot slot = _allSlots[j];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                repeat:
                if (itemInSlot == null || itemInSlot.Item != item)
                    continue;

                itemInSlot.Count--;
                counter++;

                if (itemInSlot.Count == 0)
                    continue;
                else if (counter < amount)
                    goto repeat;
            }
        }

        public bool Contains(ItemObject item, int amount)
        {
            var counter = amount;

            for (int i = 0; i < _allSlots.Length; i++)
            {
                Slot slot = _allSlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot == null || itemInSlot.Item != item)
                    continue;

                counter -= itemInSlot.Count;

                if (counter > 0)
                    continue;
                else
                    return true;
            }

            return false;
        }

        public ItemObject GetAmmoItem(AmmoType ammoType)
        {
            for (int i = 0; i < _allSlots.Length; i++)
            {
                Slot slot = _allSlots[i];

                if (slot.ItemObject == null) continue;

                if (slot.ItemObject.AmmoType == ammoType)
                {
                    GameObject ammoPrefab = slot.ItemObject.AmmoPrefab;
                    if (ammoPrefab != null)
                        return slot.ItemObject;
                }
            }

            return null;
        }

        private void SpawnInventoryItem(ItemObject item, Slot slot, List<ItemParameter> itemParameters)
        {
            GameObject inventoryItemGo = Instantiate(_inventoryItemPrefab, slot.transform);
            InventoryItem invItem = inventoryItemGo.GetComponent<InventoryItem>();
            invItem.Initialize(item, itemParameters);
        }
    }
}
