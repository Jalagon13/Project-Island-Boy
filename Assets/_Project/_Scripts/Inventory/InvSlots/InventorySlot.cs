using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public static event Action SlotClickedEvent;

        [SerializeField] private AudioClip _popSound;

        private MouseItemHolder _mouseItemHolder;
        private bool _inventoryOpen;
        private int _maxStack;

        public MouseItemHolder MouseItemHolder { set { _mouseItemHolder = value; } }
        public bool InventoryOpen { set { _inventoryOpen = value; } }
        public int MaxStack { set { _maxStack = value; } }
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_inventoryOpen) return;

            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (HasItem())
                    {
                        if(ItemObject == _mouseItemHolder.ItemObject)
                        {
                            if (ItemObject.Stackable)
                            {
                                TryToAddMouseStackToThisStack();
                            }
                            else
                            {
                                SwapThisItemAndMouseItem();
                            }
                        }
                        else
                        {
                            SwapThisItemAndMouseItem();
                        }
                    }
                    else
                    {
                        _mouseItemHolder.GiveItemToSlot(transform);
                        PlaySound();
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        GiveThisItemToMouseHolder();
                    }
                }
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (HasItem())
                    {
                        if(ItemObject == _mouseItemHolder.ItemObject)
                        {
                            if (ItemObject.Stackable)
                            {
                                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

                                if(thisInventoryItem.Count < _maxStack)
                                {
                                    _mouseItemHolder.InventoryItem.Count -= 1;
                                    thisInventoryItem.Count += 1;
                                    PlaySound();
                                }
                            }
                            else
                            {
                                SwapThisItemAndMouseItem();
                            }
                        }
                        else
                        {
                            SwapThisItemAndMouseItem();
                        }
                    }
                    else
                    {
                        if (_mouseItemHolder.ItemObject.Stackable)
                        {
                            if (!HasItem())
                            {
                                GameObject newItemGo = Instantiate(_mouseItemHolder.ItemGo, transform);
                                InventoryItem item = newItemGo.GetComponent<InventoryItem>();
                                item.Initialize(_mouseItemHolder.ItemObject, _mouseItemHolder.ItemObject.DefaultParameterList);
                                _mouseItemHolder.InventoryItem.Count -= 1;
                                PlaySound();
                            }
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            PlaySound();
                        }
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        if (ItemObject.Stackable)
                        {
                            BreakStackInHalf();
                        }
                        else
                        {
                            GiveThisItemToMouseHolder();
                        }
                    }
                }
            }

            SlotClickedEvent?.Invoke();
        }

        private void BreakStackInHalf()
        {
            var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

            if(thisInventoryItem.Count > 1)
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

        private void TryToAddMouseStackToThisStack()
        {
            if(ItemObject == _mouseItemHolder.ItemObject && ItemObject.Stackable)
            {
                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

                if(thisInventoryItem.Count < _maxStack)
                {
                    var countRef = thisInventoryItem.Count;
                    thisInventoryItem.Count += _mouseItemHolder.InventoryItem.Count;

                    if(thisInventoryItem.Count > _maxStack)
                    {
                        _mouseItemHolder.InventoryItem.Count -= _maxStack - countRef;
                        thisInventoryItem.Count = _maxStack;
                        PlaySound();
                    }
                    else if(thisInventoryItem.Count <= _maxStack)
                    {
                        _mouseItemHolder.InventoryItem.Count = 0;
                        PlaySound();
                    }
                }
            }
        }

        private void SwapThisItemAndMouseItem()
        {
            if(HasItem() && _mouseItemHolder.HasItem())
            {
                var thisItem = transform.GetChild(0);
                var mouseItem = _mouseItemHolder.transform.GetChild(0);

                thisItem.SetParent(_mouseItemHolder.transform, false);
                mouseItem.SetParent(transform, false);
                PlaySound();
            }
        }

        private void GiveThisItemToMouseHolder()
        {
            var item = transform.GetChild(0);
            item.SetParent(_mouseItemHolder.transform, false);
            PlaySound();
        }

        private bool HasItem()
        {
            return transform.childCount > 0;
        }

        private void PlaySound()
        {
            AudioManager.Instance.PlayClip(_popSound, false, true);
        }
    }
}
