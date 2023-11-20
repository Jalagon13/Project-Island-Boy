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

        private SelectedSlotControl _selectedSlotControl;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;
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
                    if (!control.Player.CanHealHp()) return;
                    break;

                case PlayerStatType.Energy:
                    if (!control.Player.CanHealNrg()) return;
                    
                    break;

                case PlayerStatType.Mana:
                    if (!control.Player.CanHealMp()) return;
                    break;
            }

            _selectedSlotControl = control;
            DispatchItemCharging();
        }

        private void DispatchItemCharging()
        {
            Action<float> chargeReleaseBehavior = ConsumeItem;
            Signal signal = GameSignals.ITEM_CHARGING;
            signal.ClearParameters();
            signal.AddParameter("ReleaseBehavior", chargeReleaseBehavior);
            signal.Dispatch();
        }

        private void ConsumeItem(float chargePercentage)
        {
            if (chargePercentage < 1) return;

            switch (_consumeType)
            {
                case PlayerStatType.Health:
                    _selectedSlotControl.Player.HealHp(_value);
                    break;

                case PlayerStatType.Energy:
                    _selectedSlotControl.Player.HealNrg(_value);
                    GameSignals.ENERGY_RESTORED.Dispatch();
                    break;

                case PlayerStatType.Mana:
                    _selectedSlotControl.Player.HealMp(_value);
                    break;
            }

            AudioManager.Instance.PlayClip(_consumeSound, false, true);
        }

        public override string GetDescription()
        {
            return $"+{_value} {_consumeType}<br>Hold Right Click to consume<br>{Description}";
        }
    }
}
