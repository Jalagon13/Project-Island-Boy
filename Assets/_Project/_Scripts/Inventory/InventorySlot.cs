using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        public static event Action SlotClickedEvent; 

        private MouseItemHolder _mouseItemHolder;
        private bool _inventoryOpen;
        private int _maxStack;

        public MouseItemHolder MouseItemHolder { set { _mouseItemHolder = value; } }
        public bool InventoryOpen { set { _inventoryOpen = value; } }
        public int MaxStack { set { _maxStack = value; } }
        public ItemObject Item
        { 
            get 
            {
                if (ThisSlotHasItem())
                {
                    var inventoryItem = transform.GetChild(0);

                    InventoryItem item = inventoryItem.GetComponent<InventoryItem>();

                    return item.Item;
                }

                return null;
            } 
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_inventoryOpen) return;

            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (ThisSlotHasItem())
                    {
                        if(Item == _mouseItemHolder.ItemObject)
                        {
                            if (Item.Stackable)
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
                    }
                }
                else
                {
                    if (ThisSlotHasItem())
                    {
                        GiveThisItemToMouseHolder();
                    }
                }
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (ThisSlotHasItem())
                    {
                        if(Item == _mouseItemHolder.ItemObject)
                        {
                            if (Item.Stackable)
                            {
                                
                                
                                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

                                if(thisInventoryItem.Count < _maxStack)
                                {
                                    _mouseItemHolder.InventoryItem.Count -= 1;
                                    thisInventoryItem.Count += 1;
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
                            if (!ThisSlotHasItem())
                            {
                                GameObject newItemGo = Instantiate(_mouseItemHolder.ItemGo, transform);
                                InventoryItem item = newItemGo.GetComponent<InventoryItem>();
                                item.Initialize(_mouseItemHolder.ItemObject, _mouseItemHolder.ItemObject.DefaultParameterList);
                                _mouseItemHolder.InventoryItem.Count -= 1;
                            }
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                        }
                    }
                }
                else
                {
                    if (ThisSlotHasItem())
                    {
                        if (Item.Stackable)
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
            }
            else
            {
                GiveThisItemToMouseHolder();
            }
        }

        private void TryToAddMouseStackToThisStack()
        {
            if(Item == _mouseItemHolder.ItemObject && Item.Stackable)
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
                    }
                    else if(thisInventoryItem.Count <= _maxStack)
                    {
                        _mouseItemHolder.InventoryItem.Count = 0;
                    }
                }
            }
        }

        private void SwapThisItemAndMouseItem()
        {
            if(ThisSlotHasItem() && _mouseItemHolder.HasItem())
            {
                var thisItem = transform.GetChild(0);
                var mouseItem = _mouseItemHolder.transform.GetChild(0);

                thisItem.SetParent(_mouseItemHolder.transform, false);
                mouseItem.SetParent(transform, false);
            }
            else
            {
                Debug.LogError($"SwapThisItemAndMouseItem callback from [{name}]. Trying to swap items but missing 1 or more items");
            }
        }

        private void GiveThisItemToMouseHolder()
        {
            var item = transform.GetChild(0);
            item.SetParent(_mouseItemHolder.transform, false);
        }

        private bool ThisSlotHasItem()
        {
            return transform.childCount > 0;
        }
    }
}
