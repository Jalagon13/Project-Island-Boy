using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public enum AccessoryType
    {
        None,
        Dash,
        Other
    }

    [CreateAssetMenu(fileName = "New Accessory Object", menuName = "Create Item/New Accessory Object")]
    public class AccessoryObject : ItemObject
    {
        [SerializeField] private AccessoryType _accessoryType;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _length; // how long effect lasts
        [SerializeField] private float _dashSpeed;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;
        public override AccessoryType AccessoryType => _accessoryType;
        public float Cooldown => _cooldown;
        public float Length => _length;
        public float DashSpeed => _dashSpeed;


        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            string description = "";
            switch (_accessoryType)
            {
                case AccessoryType.Dash:
                    description += $"* Press SPRINT to Dash!<br>";
                    break;
            }

            return "* Accessory<br>" + description + $"* { _cooldown} second cooldown";
        }
    }
}

// TODO:
// rename and create sprite for item
// change sprite icon under stat bars
// get rid of "Other" accessory type and temp accessory i created
// update accessory slot icon
