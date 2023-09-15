using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class PortalSlot : Slot
    {
        public event EventHandler OnSlotComplete;

        private ItemObject _itemReq;
        private int _amountReq;
        private bool _slotComplete;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_slotComplete) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject == _itemReq)
                    {
                        if (HasItem())
                        {
                            TryToAddMouseStackToThisStack(_amountReq);
                        }
                        else
                        {
                            FillSlot();
                        }
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {

            }

            if (SlotComplete())
            {
                _slotComplete = true;
                OnSlotComplete?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FillSlot()
        {
            int mouseSlotCount = _mouseItemHolder.InventoryItem.Count;
            int amountNeeded = _amountReq;

            if (mouseSlotCount >= amountNeeded)
            {
                _mouseItemHolder.InventoryItem.Count -= _amountReq;
                _pr.Inventory.SpawnInventoryItem(_mouseItemHolder.ItemObject, this, null);
                InventoryItem.Count = _amountReq;
            }
            else
            {
                _mouseItemHolder.GiveItemToSlot(transform);
            }

            PlaySound();
        }

        private bool SlotComplete()
        {
            if(HasItem())
            {
                if (InventoryItem.Count >= _amountReq && InventoryItem.Item == _itemReq)
                    return true;
            }

            return false;
        }

        public void Initialize(ItemObject item, int amount)
        {
            Image image = transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            RscSlotImageHover rsih = transform.GetChild(0).GetComponent<RscSlotImageHover>();

            image.sprite = item.UiDisplay;
            text.text = amount.ToString();
            rsih.OutputItem = item;

            _itemReq = item;
            _amountReq = amount;
        }
    }
}
