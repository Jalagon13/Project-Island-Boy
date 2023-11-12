using System.Text;
using UnityEngine;

namespace IslandBoy
{
    public enum ToolType
    {
        None,
        Axe,
        Pickaxe,
        Sword,
        Hammer
    }

    [CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
    public class ToolObject : ItemObject
    {
        [field: SerializeField] public ToolType Type { get; set; }

        public override ToolType ToolType => Type;
        public override AmmoType AmmoType => _baseAmmoType;
        public override ArmorType ArmorType => _baseArmorType;
        public override GameObject AmmoPrefab => null;
        public override int ConsumeValue => 0;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        public override string GetDescription()
        {
            string powerDesc = string.Empty;

            float powerValue = 0;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "Power":
                        powerValue = item.Value;
                        powerDesc = $"• {item.Value} Damage<br>";
                        break;
                }
            }

            string desc = string.Empty;

            switch (Type)
            {
                case ToolType.Axe:
                    desc = $"• {powerValue} hit damage to trees{Description}";
                    break;
                case ToolType.Pickaxe:
                    desc = $"• {powerValue} hit damage to rocks{Description}";
                    break;
                case ToolType.Sword:
                    desc = $"• {powerValue} hit damage to creatures{Description}";
                    break;
                case ToolType.Hammer:
                    desc = $"• {powerValue} hit damage to floors and walls{Description}";
                    break;
            }

            return desc;
        }
    }
}
