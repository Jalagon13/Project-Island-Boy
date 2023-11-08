using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Consumable", menuName = "Create Item/New Consumable")]
    public class ConsumableObject : ItemObject
    {
        [SerializeField] private PlayerStatType _consumeType;
        [SerializeField] private AudioClip _consumeSound;
        [SerializeField] private int _value;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => _value;
        public PlayerStatType ConsumeType => _consumeType;
        public AudioClip ConsumeSound => _consumeSound;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {

        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;

            switch (_consumeType)
            {
                case PlayerStatType.Health:
                    control.Player.HealHp(_value);
                    break;

                case PlayerStatType.Energy:
                    control.Player.HealNrg(_value);
                    break;

                case PlayerStatType.Mana:
                    control.Player.HealMp(_value);
                    break;
            }
        }

        public override string GetDescription()
        {
            return $"+{_value} {_consumeType}<br>Right Click to consume<br>{Description}";
        }
    }
}
