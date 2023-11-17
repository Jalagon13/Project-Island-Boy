using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class InventorySlot : Slot
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            //if (!_inventoryOpen) return;
            if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && _chestOpen) // BROOKE --------------
            {
                Debug.Log("shift key was pressed"); // TEST
                if (_isChestSlot) MoveItemIntoInventory();
                else MoveItemIntoChest();
            }
            else if (eventData.button == PointerEventData.InputButton.Left) // BROOKE ---------------------------
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (HasItem())
                    {
                        if(ItemObject == _mouseItemHolder.ItemObject)
                        {
                            if (ItemObject.Stackable)
                            {
                                TryToAddMouseStackToThisStack(_maxStack);
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
                        if(ItemObject == _mouseItemHolder.ItemObject)
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

            GameSignals.SLOT_CLICKED.Dispatch();
        }

        public void RegisterSlotEvent()
        {

        }
    }
}
