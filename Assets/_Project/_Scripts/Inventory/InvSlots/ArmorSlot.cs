using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class ArmorSlot : Slot
    {
        [SerializeField] private ArmorType _slotArmorType;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
            {
                if (_mouseItemHolder.HasItem())
                {
                    if (_mouseItemHolder.ItemObject.ArmorType == _slotArmorType)
                    {
                        if (HasItem())
                        {
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
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                        UnEquipArmor();
                    }
                }
            }
        }

        private void EquipArmor()
        {
            // clear current armor values
            // set armor values
        }

        private void UnEquipArmor()
        {
            // clear current armor values
        }
    }
}
