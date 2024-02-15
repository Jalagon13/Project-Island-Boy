using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
    public class ArmorSlot : Slot
    {
        [SerializeField] private ItemParameter _defenseParameter;
        [SerializeField] private ItemParameter _durabilityParameter;
        private Image _image;
        private Sprite _armorSlotSprite;
        [SerializeField] private Sprite _blankSlotSprite;

        private int _defense;

        private void Awake()
        {
            GameSignals.EQUIP_ITEM.AddListener(EquipArmor);
            _image = GetComponent<Image>();
            _armorSlotSprite = _image.sprite;
        }

        private void OnDestroy()
        {
            GameSignals.EQUIP_ITEM.RemoveListener(EquipArmor);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift))
            {
                if (!_pr.Inventory.IsFull())
                {
                    UnEquipArmor();
                    MoveItemIntoInventory();
                }
                // TODO: play error sound if wasn't able to remove item
            }
            else if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject.ArmorType == _slotArmorType)
                    {
                        if (HasItem())
                        {
                            UnEquipArmor();
                            SwapThisItemAndMouseItem();
                            EquipArmor();
                        }
                        else
                        {
                            _mouseItemHolder.GiveItemToSlot(transform);
                            TooltipManager.Instance.Show(ItemObject.GetDescription(), ItemObject.Name);
                            PlaySound();
                            EquipArmor();
                        }
                    }
                }
                else
                {
                    if (HasItem())
                    {
                        UnEquipArmor();
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                    }
                }
            }

            GameSignals.SLOT_CLICKED.Dispatch();
        }

        private void EquipArmor(ISignalParameters parameter)
        {
            ItemObject item = parameter.GetParameter("item") as ItemObject;
            if (item.ArmorType != ArmorType.None && item.ArmorType == _slotArmorType && HasItem() && InventoryItem.Item == item)
                EquipArmor();
        }

        private void EquipArmor()
        {
            _defense = GetDefenseFromItem();
            _pr.AddDefense(_defense);
            _image.sprite = _blankSlotSprite;
        }

        private void UnEquipArmor()
        {
            _pr.AddDefense(-_defense);
            _defense = 0;
            _image.sprite = _armorSlotSprite;
        }

        private int GetDefenseFromItem()
        {
            var itemParams = InventoryItem.CurrentParameters;

            if (itemParams.Contains(_defenseParameter))
            {
                int index = itemParams.IndexOf(_defenseParameter);
                return (int)itemParams[index].Value;
            }
            return 0;
        }
    }
}
