using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class ArmorSlot : Slot
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ArmorType _slotArmorType;
        [SerializeField] private ItemParameter _defenseParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private int _defense;
        private static int _counterThreshold = 20; // for every x amount of damage taken, reduce durability by 1
        private int _counter; // keeps track of damage taken;

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
                        GiveThisItemToMouseHolder();
                        TooltipManager.Instance.Hide();
                        UnEquipArmor();
                    }
                }
            }
        }

        public void DamageArmor(int incomingDamage) // connected to PlayerEntity
        {
            if (InventoryItem == null) return;

            _counter += incomingDamage;

            int decreaseAmount = 0;

            while(_counter >= _counterThreshold)
            {
                _counter -= _counterThreshold;
                decreaseAmount++;
            }

            DecreaseDurability(decreaseAmount);
        }

        private void DecreaseDurability(int decreaseAmount) 
        {
            if (InventoryItem == null || decreaseAmount == 0) return;

            var itemParams = InventoryItem.CurrentParameters;

            int index = itemParams.IndexOf(_durabilityParameter);
            float newValue = itemParams[index].Value - decreaseAmount;
            itemParams[index] = new ItemParameter
            {
                Parameter = _durabilityParameter.Parameter,
                Value = newValue
            };

            InventoryItem.UpdateDurabilityCounter();

            StartCoroutine(FrameDelay());
        }

        private IEnumerator FrameDelay()
        {
            yield return new WaitForEndOfFrame();

            if (!HasItem())
            {
                UnEquipArmor();
            }
        }

        private void EquipArmor()
        {
            _defense = GetDefenseFromItem();
            _pr.AddDefense(_defense);
        }

        private void UnEquipArmor()
        {
            _pr.AddDefense(-_defense);
            _defense = 0;
            _counter = 0;
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
