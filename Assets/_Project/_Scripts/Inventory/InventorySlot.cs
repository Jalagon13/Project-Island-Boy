using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _inventoryItemPrefab;
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
                        if(ThisItemObject() == _mouseItemHolder.MouseItemObject())
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
                        if(ThisItemObject() == _mouseItemHolder.MouseItemObject())
                        {
                            if (ThisItemObject().Stackable)
                            {
                                var mouseInventoryItem = _mouseItemHolder.transform.GetChild(0).GetComponent<InventoryItem>();
                                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

                                if(thisInventoryItem.Count < _maxStack)
                                {
                                    mouseInventoryItem.Count -= 1;
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
                        if (_mouseItemHolder.MouseItemObject().Stackable)
                        {
                            CreateItem(_mouseItemHolder.MouseItemObject());
                            var mouseInventoryItem = _mouseItemHolder.transform.GetChild(0).GetComponent<InventoryItem>();
                            mouseInventoryItem.Count -= 1;
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

        private void CreateItem(ItemObject item)
        {
            if (!ThisSlotHasItem())
            {
                GameObject newItemGo = Instantiate(_inventoryItemPrefab, transform);
                IInventoryItemInitializer inventoryItem = newItemGo.GetComponent<IInventoryItemInitializer>();
                inventoryItem.Initialize(item);
            }
        }

        private void BreakStackInHalf()
        {
            var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

            if(thisInventoryItem.Count > 1)
            {
                var smallHalf = thisInventoryItem.Count / 2;
                var bigHalf = thisInventoryItem.Count - (thisInventoryItem.Count / 2);

                _mouseItemHolder.CreateMouseItem(thisInventoryItem.Item);
                var mouseInventoryItem = _mouseItemHolder.transform.GetChild(0).GetComponent<InventoryItem>();

                thisInventoryItem.Count = smallHalf;
                mouseInventoryItem.Count = bigHalf;
            }
            else
            {
                GiveThisItemToMouseHolder();
            }
        }

        private void TryToAddMouseStackToThisStack()
        {
            if(ThisItemObject() == _mouseItemHolder.MouseItemObject() && ThisItemObject().Stackable)
            {
                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();
                var mouseInventoryItem = _mouseItemHolder.transform.GetChild(0).GetComponent<InventoryItem>();

                if(thisInventoryItem.Count < _maxStack)
                {
                    var countRef = thisInventoryItem.Count;
                    thisInventoryItem.Count += mouseInventoryItem.Count;

                    if(thisInventoryItem.Count > _maxStack)
                    {
                        mouseInventoryItem.Count -= _maxStack - countRef;
                        thisInventoryItem.Count = _maxStack;
                    }
                    else if(thisInventoryItem.Count <= _maxStack)
                    {
                        mouseInventoryItem.Count = 0;
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
                var item = inventoryItem.GetComponent<IInventoryItemInitializer>();

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
