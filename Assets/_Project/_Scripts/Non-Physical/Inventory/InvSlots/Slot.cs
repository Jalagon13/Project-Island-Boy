using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public abstract class Slot : MonoBehaviour, IPointerClickHandler
    {
        public EventHandler OnSlotClicked;

        [SerializeField] protected PlayerReference _pr;
        [SerializeField] protected AudioClip _popSound;
        [SerializeField] protected AudioClip _runeEquipSound;

        protected MouseItemHolder _mouseItemHolder;
        protected int _maxStack;
        protected bool _inventoryOpen;

        public MouseItemHolder MouseItemHolder { get { return _mouseItemHolder; } }
        public bool InventoryOpen { set { _inventoryOpen = value; } }
        public ItemObject ItemObject
        {
            get
            {
                if (!HasItem()) return null;

                var inventoryItem = transform.GetChild(0);
                InventoryItem item = inventoryItem.GetComponent<InventoryItem>();
                return item.Item;
            }
        }
        public InventoryItem InventoryItem
        {
            get
            {
                if (!HasItem()) return null;

                var inventoryItem = transform.GetChild(0);
                return inventoryItem.GetComponent<InventoryItem>();
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

        private void Awake()
        {
            _mouseItemHolder = _pr.Inventory.MouseItemHolder;
            _maxStack = _pr.Inventory.MaxStack;
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

        protected void TryToAddMouseStackToThisStack()
        {
            if (ItemObject == _mouseItemHolder.ItemObject && ItemObject.Stackable)
            {
                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

                if (thisInventoryItem.Count < _maxStack)
                {
                    var countRef = thisInventoryItem.Count;
                    thisInventoryItem.Count += _mouseItemHolder.InventoryItem.Count;

                    if (thisInventoryItem.Count > _maxStack)
                    {
                        _mouseItemHolder.InventoryItem.Count -= _maxStack - countRef;
                        thisInventoryItem.Count = _maxStack;
                        TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                        PlaySound();
                    }
                    else if (thisInventoryItem.Count <= _maxStack)
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

        protected void GiveThisItemToMouseHolder()
        {
            var item = transform.GetChild(0);
            item.SetParent(_mouseItemHolder.transform, false);
            PlaySound();
        }

        protected bool HasItem()
        {
            return transform.childCount > 0;
        }

        protected void PlaySound()
        {
            AudioManager.Instance.PlayClip(_popSound, false, true, 0.75f);
        }
    }
}
