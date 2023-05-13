using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Inventory : MonoBehaviour
    {
        public static event Action AddItemEvent;

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _inventoryItemPrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private int _maxStack;
        [SerializeField] private InventorySlot[] _inventorySlots;

        private MouseItemHolder _mouseItemHolder;

        public InventorySlot[] InventorySlots { get { return _inventorySlots; } }
        public int MaxStack { get { return _maxStack; } }

        private void Awake()
        {
            _pr.PlayerInventory = this;
            _mouseItemHolder = transform.GetChild(2).GetComponent<MouseItemHolder>();
        }

        private void Start()
        {
            foreach (InventorySlot slot in _inventorySlots)
            {
                slot.MouseItemHolder = _mouseItemHolder;
                slot.MaxStack = _maxStack;
            }
        }

        public bool AddItem(ItemObject item, List<ItemParameter> itemParameters = null)
        {
            // Check if any slot has the same item with count lower than max stack.
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null &&
                    itemInSlot.Item == item &&
                    itemInSlot.Count < _maxStack &&
                    itemInSlot.Item.Stackable == true)
                {
                    itemInSlot.IncrementCount();
                    OnAddItem();
                    return true;
                }
            }

            // Find an empty slot
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                
                if(slot.transform.childCount == 0)
                {
                    SpawnInventoryItem(item, slot, itemParameters);
                    OnAddItem();
                    return true;
                }
            }

            return false;
        }

        public bool Contains(ItemObject item, int amount)
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot == null)
                    continue;

                if(itemInSlot.Item == item && itemInSlot.Count >= amount)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnAddItem()
        {
            AddItemEvent?.Invoke();
            AudioManager.Instance.PlayClip(_popSound, false, true);
        }

        private void SpawnInventoryItem(ItemObject item, InventorySlot slot, List<ItemParameter> itemParameters)
        {
            GameObject inventoryItemGo = Instantiate(_inventoryItemPrefab, slot.transform);
            InventoryItem invItem = inventoryItemGo.GetComponent<InventoryItem>();
            invItem.Initialize(item, itemParameters);
        }
    }
}
