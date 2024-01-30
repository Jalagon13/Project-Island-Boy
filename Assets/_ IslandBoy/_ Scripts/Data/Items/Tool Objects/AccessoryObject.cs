using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public enum AccessoryType
    {
        None,
        Dash
    }

    [CreateAssetMenu(fileName = "New Accessory Object", menuName = "Create Item/New Accessory Object")]
    public class AccessoryObject : ItemObject
    {
        [SerializeField] private AccessoryType _accessoryType;

        public override ToolType ToolType => _baseToolType;
        public override ArmorType ArmorType => _baseArmorType;
        public override AccessoryType AccessoryType => _accessoryType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            //string upgradeText = _upgradeRecipe != null ? $"* Next upgrade: {_upgradeRecipe.OutputItem.Name}" : string.Empty;

            //return $"* {hitValue} per hit<br>* {miningSpeed} mining speed<br>* {damage} damage<br>{upgradeText}";

            /*foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "Hit":
                        hitValue = item.Value;
                        break;
                    case "MiningSpeed":
                        miningSpeed = item.Value;
                        break;
                    case "Damage":
                        damage = item.Value;
                        break;
                }
            }*/

            return _accessoryType + " Accessory";
        }
    }
}
