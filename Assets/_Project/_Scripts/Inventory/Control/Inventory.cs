using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandBoy
{
    public class Inventory : MonoBehaviour
    {
        public static event Action AddItemEvent;

        [SerializeField] private PlayerReference _pr;
        [SerializeField] private GameObject _inventoryItemPrefab;
        [SerializeField] private GameObject _itemPickupSignPrefab;
        [SerializeField] private AudioClip _popSound;
        [SerializeField] private int _maxStack;
        [SerializeField] private InventorySlot[] _inventorySlots;

        private MouseItemHolder _mouseItemHolder;
        private Dictionary<ItemObject, ItemPickupSign> _itemPickups = new();

        public InventorySlot[] InventorySlots { get { return _inventorySlots; } }
        public int MaxStack { get { return _maxStack; } }

        private void Awake()
        {
            _pr.Inventory = this;
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

        public int AddItem(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
        {
            // return leftover if there is.
            for (int i = 0; i < amount; i++)
            {
                bool added = Add(item, amount, itemParameters);

                if (!added)
                {
                    int leftOver = amount - i;
                    StartCoroutine(SpawnPickupSign(item, amount - leftOver));
                    return leftOver;
                }
            }

            StartCoroutine(SpawnPickupSign(item, amount));
            return 0;
        }

        private IEnumerator SpawnPickupSign(ItemObject item, int amount)
        {
            yield return new WaitForSeconds(Random.Range(0, 0.01f));

            AudioManager.Instance.PlayClip(_popSound, false, true);

            if (_itemPickups.ContainsKey(item))
            {
                if (_itemPickups.TryGetValue(item, out ItemPickupSign pickup))
                {
                    pickup.Refresh(amount);
                }
            }
            else
            {
                GameObject itemPickupSign = Instantiate(_itemPickupSignPrefab);
                ItemPickupSign pickup = itemPickupSign.GetComponent<ItemPickupSign>();
                pickup.Initialize(amount, item.Name, () => { _itemPickups.Remove(item); });
                _itemPickups.Add(item, pickup);
            }

            AddItemEvent?.Invoke();
        }

        private bool Add(ItemObject item, int amount, List<ItemParameter> itemParameters = null)
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
                    return true;
                }
            }

            // Find an empty slot
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];

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

            for (int j = 0; j < _inventorySlots.Length; j++)
            {
                if (counter >= amount) return;

                InventorySlot slot = _inventorySlots[j];
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

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                InventorySlot slot = _inventorySlots[i];
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

        private void SpawnInventoryItem(ItemObject item, InventorySlot slot, List<ItemParameter> itemParameters)
        {
            GameObject inventoryItemGo = Instantiate(_inventoryItemPrefab, slot.transform);
            InventoryItem invItem = inventoryItemGo.GetComponent<InventoryItem>();
            invItem.Initialize(item, itemParameters);
        }
    }
}
