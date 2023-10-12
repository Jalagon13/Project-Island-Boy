using System.Text;
using UnityEngine;

namespace IslandBoy
{
    public enum ToolType
    {
        None,
        Indifferent,
        Ax,
        Pickaxe,
        Sword,
        Shovel,
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

        public override void ExecuteAction(SelectedSlotControl control)
        {
            
        }

        public override string GetDescription()
        {
            //string powerDesc = string.Empty;
            string damageDesc = string.Empty;
            string attackSpeed = string.Empty;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    //case "Power":
                    //    powerDesc = $"• {item.Value} {ToolType} Power<br>";
                    //    break;
                    case "Damage":
                        damageDesc = $"• {item.Value} Damage<br>";
                        break;
                    case "AttackSpeed":
                        attackSpeed = $"• {item.Value}s Attack Speed<br>";
                        break;
                }
            }

            return $"{damageDesc}{attackSpeed}{Description}";
        }
    }
}
