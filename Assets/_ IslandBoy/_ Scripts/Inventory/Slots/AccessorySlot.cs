using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class AccessorySlot : Slot
    {
        private AccessoryType _type = AccessoryType.None;
        private Image _image;
        private Sprite _accessorySlotSprite;
        [SerializeField] private Sprite _blankSlotSprite;

        private void Awake()
        {
            GameSignals.EQUIP_ITEM.AddListener(EquipAccessory);
            _isAccessorySlot = true;
            _image = GetComponent<Image>();
            _accessorySlotSprite = _image.sprite;
        }

        private void OnDestroy()
        {
            GameSignals.EQUIP_ITEM.RemoveListener(EquipAccessory);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift))
            {
                if (!_pr.Inventory.IsFull())
                {
                    UnequipAccessory();
                    MoveItemIntoInventory();
                }
                // TODO: play error sound if wasn't able to remove item
            }
            else if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject.AccessoryType != AccessoryType.None)
                    {
                        if (HasItem())
                        {
                            UnequipAccessory();
                            SwapThisItemAndMouseItem();
                            EquipAccessory();
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                            PlaySound();
                            EquipAccessory();
                        }
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        UnequipAccessory(); 
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                    }
                }
            }

            GameSignals.SLOT_CLICKED.Dispatch();
        }

        private void EquipAccessory(ISignalParameters parameter)
        {
            ItemObject item = parameter.GetParameter("item") as ItemObject;
            if (item.AccessoryType != AccessoryType.None && HasItem() && InventoryItem.Item == item)
                EquipAccessory();
        }

        private void EquipAccessory()
        {
            _type = transform.GetChild(0).GetComponent<InventoryItem>().Item.AccessoryType;
            switch (_type)
            {
                case AccessoryType.Dash:
                    _pr.GameObject.GetComponent<Accessories>().EnableDash();
                    break;
            }
            _image.sprite = _blankSlotSprite;
        }

        private void UnequipAccessory()
        {
            switch (_type)
            {
                case AccessoryType.Dash:
                    _pr.GameObject.GetComponent<Accessories>().DisableDash();
                    break;
            }
            _type = AccessoryType.None;
            _image.sprite = _accessorySlotSprite;
        }
    }
}
