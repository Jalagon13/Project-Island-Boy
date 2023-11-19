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
            float hitValue = 0;

            foreach (var item in DefaultParameterList)
            {
                switch (item.Parameter.ParameterName)
                {
                    case "Hit":
                        hitValue = item.Value;
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
                    desc = $"• {hitValue} hits to trees<br>• {clickDistance} click distance<br>• upgrades into {_upgradeRecipe.OutputItem.Name}";
                    break;
                case ToolType.Pickaxe:
                    desc = $"• {hitValue} hits to rocks<br>• {clickDistance} click distance<br>• upgrades into {_upgradeRecipe.OutputItem.Name}";
                    break;
                case ToolType.Sword:
                    desc = $"• {hitValue} hits to creatures<br>• {clickDistance} click distance<br>• upgrades into {_upgradeRecipe.OutputItem.Name}";
                    break;
                case ToolType.Hammer:
                    desc = $"• {hitValue} hits to floors and walls<br>• {clickDistance} click distance<br>• upgrades into {_upgradeRecipe.OutputItem.Name}";
                    break;
            }

            return desc;
        }
    }
}
