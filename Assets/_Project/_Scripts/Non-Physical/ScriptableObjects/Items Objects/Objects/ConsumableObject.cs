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

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => _value;
        public ConsumeType ConsumeType => _consumeType;
        public AudioClip ConsumeSound => _consumeSound;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {

        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {
            if (PointerHandler.IsOverLayer(5)) return;
            
            DispatchTryConsumeItem();
        }

        private void DispatchTryConsumeItem()
        {
            Signal signal = GameSignals.CONSUME_ITEM_TRY;
            signal.ClearParameters();
            signal.AddParameter("ConsumeItem", this);
            signal.Dispatch();
        }

        public override string GetDescription()
        {
            return $"• Can be eaten<br>+ {_value} {_consumeType}<br>{Description}";
        }
    }
}
