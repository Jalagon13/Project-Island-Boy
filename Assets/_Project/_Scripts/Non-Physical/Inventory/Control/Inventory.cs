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
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private int _maxStack;
        [SerializeField] private List<ChestInvSlot> _startingItems = new();
        [SerializeField] private List<Slot> _allSlots = new();

        private MouseSlot _mouseItemHolder;

        public List<Slot> InventorySlots { get { return _allSlots; } }
        public MouseSlot MouseItemHolder { get { return _mouseItemHolder; } }
        public InventoryControl InventoryControl { get { return GetComponent<InventoryControl>(); } }
        public int MaxStack { get { return _maxStack; } }

        private void Awake()
        {
            _pr.Inventory = this;
            _mouseItemHolder = transform.GetChild(3).GetChild(0).GetComponent<MouseSlot>();
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            foreach (ChestInvSlot item in _startingItems)
            {
                AddItem(item.OutputItem, item.OutputAmount, item.OutputItem.DefaultParameterList);
            }
        }

        public int AddItem(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
        {
            // return leftover if there is.
            for (int i = 0; i < amount; i++)
            {
                if (!Add(item, amount, itemParameters))
                {
                    int leftOver = amount - i;
                    DispatchItemAdded();
                    //_onPickupItem?.Invoke(item, amount - leftOver);
                    return leftOver;
                }
            }

            DispatchItemAdded();
            //_onPickupItem?.Invoke(item, amount);
            return 0;
        }

        private void DispatchItemAdded()
        {
            Signal signal = GameSignals.ITEM_ADDED;
            signal.ClearParameters();
            signal.AddParameter("Inventory", this);
            signal.Dispatch();
        }

        private bool Add(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
        {
            // Check if any slot has the same item with count lower than max stack.
            for (int i = 0; i < _allSlots.Count; i++)
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
            for (int i = 0; i < _allSlots.Count; i++)
            {
                Slot slot = _allSlots[i];

                if(slot.SpawnInventoryItem(item, itemParameters))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveItem(ItemObject item, int amount)
        {
            var counter = 0;

            for (int j = 0; j < _allSlots.Count; j++)
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

        public int GetItemAmount(ItemObject item)
        {
            int amount = 0;

            for (int i = 0; i < _allSlots.Count; i++)
            {
                Slot slot = _allSlots[i];

                if (slot.ItemObject == item)
                    amount += slot.InventoryItem.Count;
            }

            return amount;
        }

        public bool Contains(ItemObject item, int amount)
        {
            var counter = amount;

            for (int i = 0; i < _allSlots.Count; i++)
            {
                Slot slot = _allSlots[i];
                //Debug.Log($"All slots Null?: {_allSlots == null}");
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
    }
}
