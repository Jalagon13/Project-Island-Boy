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
        [SerializeField] private CraftingRecipeObject _upgradeRecipe;
        [SerializeField] private int _xpForUpgrade;

        public override ToolType ToolType => Type;
        public override ArmorType ArmorType => _baseArmorType;
        public override int ConsumeValue => 0;
        public CraftingRecipeObject UpgradeRecipe => _upgradeRecipe;
        public int XpForUpgrade => _xpForUpgrade;

        public override void ExecutePrimaryAction(SelectedSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(SelectedSlotControl control)
        {

        }

        public override string GetDescription()
        {
            float clickDistance = 0;
            float powerValue = 0;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "Power":
                        powerValue = item.Value;
                        break;
                    case "ClickDistance":
                        clickDistance = item.Value;
                        break;
                }
            }

            string desc = string.Empty;

            switch (Type)
            {
                case ToolType.Axe:
                    desc = $"� {powerValue} hits to trees<br>� {clickDistance} click distance<br>� can be upgraded by the Miner";
                    break;
                case ToolType.Pickaxe:
                    desc = $"� {powerValue} hits to rocks<br>� {clickDistance} click distance<br>� can be upgraded by the Miner";
                    break;
                case ToolType.Sword:
                    desc = $"� {powerValue} hits to creatures<br>� {clickDistance} click distance<br>� can be upgraded by the Miner";
                    break;
                case ToolType.Hammer:
                    desc = $"� {powerValue} hits to floors and walls<br>� {clickDistance} click distance<br>� can be upgraded by the Miner";
                    break;
            }

            return desc;
        }
    }
}
