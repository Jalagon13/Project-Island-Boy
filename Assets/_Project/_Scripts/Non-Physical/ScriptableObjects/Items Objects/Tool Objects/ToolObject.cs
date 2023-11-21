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
        [Space(10)]
        [SerializeField] private ToolType _type;
        [Header("Upgrade Parameters")]
        [SerializeField] private CraftingRecipeObject _upgradeRecipe;
        [SerializeField] private int _xpForUpgrade;

        public override ToolType ToolType => _type;
        public override ArmorType ArmorType => _baseArmorType;
        public CraftingRecipeObject UpgradeRecipe => _upgradeRecipe;
        public int XpForUpgrade => _xpForUpgrade;

        public override void ExecutePrimaryAction(FocusSlotControl control)
        {
            
        }

        public override void ExecuteSecondaryAction(FocusSlotControl control)
        {

        }

        public override string GetDescription()
        {
            float clickDistance = 0;
            float hitValue = 0;
            float damage = 0;

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
                    case "Damage":
                        damage = item.Value;
                        break;
                }
            }

            string toolTypeDesc = string.Empty;

            switch (_type)
            {
                case ToolType.Axe:
                    toolTypeDesc = $"• {hitValue} hits to trees<br>";
                    break;
                case ToolType.Pickaxe:
                    toolTypeDesc = $"• {hitValue} hits to rocks<br>";
                    break;
                case ToolType.Sword:
                    toolTypeDesc = $"• {hitValue} hits to creatures<br>";
                    break;
                case ToolType.Hammer:
                    toolTypeDesc = $"• {hitValue} hits to floors and walls<br>";
                    break;
            }

            string upgradeText = _upgradeRecipe != null ? $"<br>• upgrades into {_upgradeRecipe.OutputItem.Name}" : string.Empty;
            string damageText = $"{damage} damage<br>";

            return $"{toolTypeDesc}• {damageText}• {clickDistance} click distance{upgradeText}";
        }
    }
}
