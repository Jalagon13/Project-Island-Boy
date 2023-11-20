using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public abstract class Slot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] protected PlayerReference _pr;
        [SerializeField] protected GameObject _inventoryItemPrefab;
        [SerializeField] protected AudioClip _popSound;
        [SerializeField] protected bool _isChestSlot; // BROOKE

        protected MouseSlot _mouseItemHolder;
        protected int _maxStack;
        protected bool _inventoryOpen;
        protected bool _chestOpen; // BROOKE

        public MouseSlot MouseItemHolder { get { return _mouseItemHolder; } }
        public bool InventoryOpen { set { _inventoryOpen = value; } }

        public ToolType ToolType
        {
            get
            {
                return HasItem() ? GetComponentInChildren<InventoryItem>().Item.ToolType : ToolType.None;
            }
        }

        public bool ChestOpen { set { _chestOpen = value; } } // BROOKE
        public ItemObject ItemObject
        {
            get
            {
                if (!HasItem()) return null;

                InventoryItem item = GetComponentInChildren<InventoryItem>();
                return item.Item;
            }
        }
        public InventoryItem InventoryItem
        {
            get
            {
                if (!HasItem()) return null;

                InventoryItem item = GetComponentInChildren<InventoryItem>();
                return item;
            }
        }
        public List<ItemParameter> CurrentParameters
        {
            get
            {
                if (!HasItem()) return new();

                var inventoryItem = transform.GetChild(0);
                return inventoryItem.GetComponent<InventoryItem>().CurrentParameters;
            }
        }

        private void OnEnable()
        {
            _mouseItemHolder = _pr.Inventory.MouseItemHolder;
            _maxStack = _pr.Inventory.MaxStack;
            if (_isChestSlot) _chestOpen = true; // BROOKE
        }

        public abstract void OnPointerClick(PointerEventData eventData);

        protected void BreakStackInHalf()
        {
            var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

            if (thisInventoryItem.Count > 1)
            {
                var smallHalf = thisInventoryItem.Count / 2;
                var bigHalf = thisInventoryItem.Count - (thisInventoryItem.Count / 2);

                _mouseItemHolder.CreateMouseItem(transform.GetChild(0).gameObject, thisInventoryItem.Item);

                thisInventoryItem.Count = smallHalf;
                _mouseItemHolder.InventoryItem.Count = bigHalf;
                PlaySound();
            }
            else
            {
                GiveThisItemToMouseHolder();
            }
        }

        protected void TryToAddMouseStackToThisStack(int maxStack)
        {
            if (ItemObject == _mouseItemHolder.ItemObject && ItemObject.Stackable)
            {
                InventoryItem item = GetComponentInChildren<InventoryItem>();

                if (item.Count < maxStack)
                {
                    var countRef = item.Count;
                    item.Count += _mouseItemHolder.InventoryItem.Count;

                    if (item.Count > maxStack)
                    {
                        _mouseItemHolder.InventoryItem.Count -= maxStack - countRef;
                        item.Count = maxStack;
                        TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                        PlaySound();
                    }
                    else if (item.Count <= maxStack)
                    {
                        _mouseItemHolder.InventoryItem.Count = 0;
                        TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                        PlaySound();
                    }
                }
            }
        }

        protected void SwapThisItemAndMouseItem()
        {
            if (HasItem() && _mouseItemHolder.HasItem())
            {
                var thisItem = transform.GetChild(0);
                var mouseItem = _mouseItemHolder.transform.GetChild(0);

                thisItem.SetParent(_mouseItemHolder.transform, false);
                mouseItem.SetParent(transform, false);
                PlaySound();
            }
        }

        protected void MoveItemIntoChest() // BROOKE ---------------------------------------------------------
        {
            if (HasItem())
            {
                var item = transform.GetChild(0);

                ChestInvSlot chestItem = new ChestInvSlot();
                chestItem.OutputItem = item.GetComponent<InventoryItem>().Item;
                chestItem.OutputAmount = item.GetComponent<InventoryItem>().Count;

                Signal signal = GameSignals.ADD_ITEMS_TO_CHEST;
                signal.ClearParameters();
                signal.AddParameter("itemsToAdd", new List<ChestInvSlot> { chestItem });
                signal.AddParameter("itemObj", item.gameObject);
                signal.Dispatch();

                PlaySound();
            }
        }

        protected void MoveItemIntoInventory()
        {
            Debug.Log("CHEST SLOT");

            if (HasItem())
            {
                var item = transform.GetChild(0);

                ChestInvSlot chestItem = new ChestInvSlot();
                chestItem.OutputItem = item.GetComponent<InventoryItem>().Item;
                chestItem.OutputAmount = item.GetComponent<InventoryItem>().Count;

                Signal signal = GameSignals.ADD_ITEM_TO_INVENTORY_FROM_CHEST;
                signal.ClearParameters();
                signal.AddParameter("itemToAdd", chestItem);
                signal.AddParameter("itemObj", item.gameObject);
                signal.Dispatch();

                PlaySound();
            }
        }
        // BROOKE ---------------------------------------------------------

        protected void GiveThisItemToMouseHolder()
        {
            var item = transform.GetChild(0);
            item.SetParent(_mouseItemHolder.transform, false);
            PlaySound();
        }

        public bool HasItem()
        {
            if(transform.childCount <= 0 ) return false;

            InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();

            return inventoryItem != null;
        }

        protected void PlaySound()
        {
            AudioManager.Instance.PlayClip(_popSound, false, true, 0.75f);
        }

        public bool SpawnInventoryItem(ItemObject item, List<ItemParameter> itemParameters = null, int count = 1)
        {
            if(transform.childCount == 0)
            {
                GameObject inventoryItemGo = Instantiate(_inventoryItemPrefab, transform);
                InventoryItem invItem = inventoryItemGo.GetComponent<InventoryItem>();
                invItem.Initialize(item, itemParameters, count);

                return true;
            }

            return false;
        }

        public void ClearSlot()
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
        }

        public int GetMaxStack() // BROOKE -------------
        {
            return _maxStack;
        }

        public void SetCount(int num)
        {
            transform.GetChild(0).GetComponent<InventoryItem>().Count = num;
        } // BROOKE ------------------------------------
    }
}
