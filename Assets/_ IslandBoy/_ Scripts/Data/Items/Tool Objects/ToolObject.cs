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
		[Header("Upgrade Parameters")]
		[SerializeField] private NpcUpgradeType _npcUpgradeType = NpcUpgradeType.None;
		[SerializeField] private CraftingRecipeObject _upgradeRecipe;

		public override ToolType ToolType => _type;
		public override ArmorType ArmorType => _baseArmorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;
		public CraftingRecipeObject UpgradeRecipe => _upgradeRecipe;
		public NpcUpgradeType NpcUpgradeType => _npcUpgradeType;

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
				}
			}

			string upgradeText = _upgradeRecipe != null && _npcUpgradeType != NpcUpgradeType.None ? $"* Can be Upgraded" : string.Empty;

			return $"{Description}<br>* {hitValue} per hit<br>* {damageMin}-{damageMax} damage<br>{upgradeText}";
		}
	}
}
