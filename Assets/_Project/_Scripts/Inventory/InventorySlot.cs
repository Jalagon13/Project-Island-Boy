using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        private MouseItemHolder _mouseItemHolder;
        private bool _inventoryOpen;
        private int _maxStack;

        public MouseItemHolder MouseItemHolder { set { _mouseItemHolder = value; } }
        public bool InventoryOpen { set { _inventoryOpen = value; } }
        public int MaxStack { set { _maxStack = value; } }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_inventoryOpen) return;

            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (ThisSlotHasItem())
                    {
                        if(ThisItemObject() == _mouseItemHolder.ItemObject)
                        {
                            if (ThisItemObject().Stackable)
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
                        if(ThisItemObject() == _mouseItemHolder.ItemObject)
                        {
                            if (ThisItemObject().Stackable)
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
                        if (ThisItemObject().Stackable)
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
            if(ThisItemObject() == _mouseItemHolder.ItemObject && ThisItemObject().Stackable)
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

        private ItemObject ThisItemObject()
        {
            if (ThisSlotHasItem())
            {
                var inventoryItem = transform.GetChild(0);

                InventoryItem item = inventoryItem.GetComponent<InventoryItem>();

                return item.Item;
            }

            Debug.LogError($"GetThisItemObject callback from [{name}]. Can not get Item because there is no item.");
            return null;
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
