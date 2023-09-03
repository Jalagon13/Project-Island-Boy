using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Augment", menuName = "Create Item/New Augment")]
    public class AugmentObject : ItemObject
    {
        public override ToolType ToolType => _baseToolType;

        public override AmmoType AmmoType => _baseAmmoType;

        public override ArmorType ArmorType => _baseArmorType;

        public override GameObject AmmoPrefab => null;

        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            return string.Empty;
        }
    }
}
