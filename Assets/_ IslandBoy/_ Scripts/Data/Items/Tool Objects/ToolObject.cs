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

	public enum ToolTier
	{
		None,
		Wood,
		Stone,
		Iron
	}

	public enum NpcUpgradeType
	{
		None,
		Blacksmith,
		Knight,
		Wizard
	}

	[CreateAssetMenu(fileName = "New Tool", menuName = "Create Item/New Tool")]
	public class ToolObject : ItemObject
	{
		[Space(10)]
		[SerializeField] private ToolType _type;
		[SerializeField] private ToolTier _tier;
		// [Header("Upgrade Parameters")]
		// [SerializeField] private int _xpUpgradeCost;
		// [SerializeField] private CraftingRecipeObject _upgradeRecipe;

		public override ToolType ToolType => _type;
		public override ToolTier ToolTier => _tier;
		public override ArmorType ArmorType => _baseArmorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;
		// public CraftingRecipeObject UpgradeRecipe => _upgradeRecipe;
		// public int XpUpgradeCost => _xpUpgradeCost;

		public override void ExecutePrimaryAction(FocusSlotControl control)
		{
			
		}

		public override void ExecuteSecondaryAction(FocusSlotControl control)
		{

		}

		public override string GetDescription()
		{
			float hitValue = 0;
			float miningSpeed = 0;
			float damageMax = 0;
			float damageMin = 0;
			float energyPerSwing = 0;

			foreach (var item in DefaultParameterList)
			{
				switch (item.Parameter.ParameterName)
				{
					case "Hit":
						hitValue = item.Value;
						break;
					case "MiningSpeed":
					   	miningSpeed = item.Value;
					   	break;
					case "Damage Max":
						damageMax = item.Value;
						break;
					case "Damage Min":
						damageMin = item.Value;
						break;
					case "Energy Per Swing":
						energyPerSwing = item.Value;
						break;
				}
			}

			string upgradeText = UpgradeRecipe != null ? $"<br>* Upgradable" : string.Empty;

			if(energyPerSwing == 0)
				return $"{Description}<br>* {damageMin}-{damageMax} damage<br>* 1 energy per swing{upgradeText}";
			else
				return $"{Description}<br>* {damageMin}-{damageMax} damage<br>* {energyPerSwing} energy per swing{upgradeText}";

			
		}
	}
}
