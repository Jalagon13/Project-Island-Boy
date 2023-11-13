using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Spell Object", menuName = "Create Item/New Spell Object")]
    public class SpellObject : ItemObject
    {
        [SerializeField] private int _manaCostPerCast;
        [SerializeField] private GameObject _spell;

        public override ToolType ToolType => _baseToolType;

        public override ArmorType ArmorType => _baseArmorType;

        public override GameObject AmmoPrefab => null;

        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {
            // check if there is enough mana to cast spell

            // inject the release behavior into the charge system

            // execute the charge on release
            DispatchItemCharging();
        }

        private void DispatchItemCharging()
        {
            Action<float> behavior = SpellReleaseBehavior;
            Signal signal = GameSignals.ITEM_CHARGING;
            signal.ClearParameters();
            signal.AddParameter("ReleaseBehavior", behavior);
            signal.Dispatch();
        }

        // note to self: remember to think about like where to put direction calculation at. If it's in the charge control or in spell object.

        public void SpellReleaseBehavior(float chargePercentage)
        {
            // instantiate the Spell Gameobject here.

            // set up the spell with the charge percentage

            // and then in each spell, just take the chargePercentage and do what ever it needs on spell instantiate. 
        }

        public override string GetDescription()
        {
            return string.Empty;
        }
    }
}
