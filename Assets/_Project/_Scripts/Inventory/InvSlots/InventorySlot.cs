using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class InventorySlot : Slot
    {
        public static event Action SlotClickedEvent;

        public override void OnPointerClick(PointerEventData eventData)
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
                        TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                        PlaySound();
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                    }
                }
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (HasItem())
                    {
                        if (CanEquipAugment())
                        {
                            InventoryItem.InitializeAugment(_mouseItemHolder.ItemObject as AugmentObject);
                            _mouseItemHolder.DeleteMouseItem();
                        }
                        else if(ItemObject == _mouseItemHolder.ItemObject)
                        {
                            if (ItemObject.Stackable)
                            {
                                var thisInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();

                                if(thisInventoryItem.Count < _maxStack)
                                {
                                    _mouseItemHolder.InventoryItem.Count -= 1;
                                    thisInventoryItem.Count += 1;
                                    
                                    if(_mouseItemHolder.HasItem())
                                        TooltipManager.Instance.Hide();
                                    else
                                        TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);

                                    PlaySound();
                                }
                            }
                            else
                            {
                                SwapThisItemAndMouseItem();
                                TooltipManager.Instance.Hide();
                            }
                        }
                        else
                        {
                            SwapThisItemAndMouseItem();
                            TooltipManager.Instance.Hide();
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

                                TooltipManager.Instance.Hide();
                                PlaySound();
                            }
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
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
                            TooltipManager.Instance.Hide();
                        }
                        else
                        {
                            GiveThisItemToMouseHolder();
                            TooltipManager.Instance.Hide();
                        }
                    }
                }
            }
            SlotClickedEvent?.Invoke();
        }

        // expand on these conditions later
        private bool CanEquipAugment()
        {
            return _mouseItemHolder.ItemObject is AugmentObject && 
                ItemObject is ToolObject && 
                InventoryItem.AugmentsOnItem < 3;
        }
    }
}
