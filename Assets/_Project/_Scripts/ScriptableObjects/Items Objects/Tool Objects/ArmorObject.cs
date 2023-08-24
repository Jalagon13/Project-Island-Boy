using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public enum ArmorType
    {
        None,
        Head,
        Chest,
        Legs
    }

    [CreateAssetMenu(fileName = "New Armor Object", menuName = "Create Item/New Armor Object")]
    public class ArmorObject : ItemObject
    {
        [SerializeField] private ArmorType _armorType;

        public override ToolType ToolType => _baseToolType;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _armorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            return "ARMORRRR";
        }
    }
}
