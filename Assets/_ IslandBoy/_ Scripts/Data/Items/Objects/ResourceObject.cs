using UnityEngine;

namespace IslandBoy
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
    public class ResourceObject : ItemObject
    {
        public override ToolType ToolType => _baseToolType;
        public override ToolTier ToolTier => _baseToolTier;
        public override ArmorType ArmorType => _baseArmorType;
        public override AccessoryType AccessoryType => _baseAccessoryType;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            float damage = 0;
            string damageStr = "";

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "Damage Max":
                        damage = item.Value;
                        break;
                }
            }

            if (damage > 0)
                damageStr += $"<br>* {damage} damage";

            return $"{GetDescriptionBreak()}<color={textBlueColor}>* Material{damageStr}</color={textBlueColor}>";
        }
    }
}
