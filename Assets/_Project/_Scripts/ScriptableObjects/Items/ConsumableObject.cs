using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public enum ConsumeType
    {
        None,
        Health,
        Energy,
        Mana
    }

    [CreateAssetMenu(fileName = "New Consumable", menuName = "Create Item/New Consumable")]
    public class ConsumableObject : ItemObject
    {
        [SerializeField] private ConsumeType _consumeType;
        [SerializeField] private AudioClip _consumeSound;
        [SerializeField] private int _value;

        public override ToolType ToolType => ToolType.Ax;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            switch (_consumeType) 
            { 
                case ConsumeType.Energy:
                    if (control.EnergyBar.CurrentValue >= control.EnergyBar.MaxValue || control.EnergyBar.InCoolDown) 
                        return;
                    break;
                case ConsumeType.Health:
                    if(control.HealthBar.CurrentValue >= control.HealthBar.MaxValue || control.HealthBar.InCoolDown) 
                        return;
                    break;
            }

            AudioManager.Instance.PlayClip(_consumeSound, false, false);
            control.PR.SelectedSlot.InventoryItem.Count--;
            control.RestoreStat(_consumeType, _value);
        }
    }
}
