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
        public override ArmorType ArmorType => _armorType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            return "ARMORRRR";
        }
    }
}