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

		public override ToolType ToolType => _type;
		public override ArmorType ArmorType => _baseArmorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;
		public CraftingRecipeObject UpgradeRecipe => _upgradeRecipe;

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
			float damage = 0;

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
					case "Damage":
						damage = item.Value;
						break;
				}
			}

			string upgradeText = _upgradeRecipe != null ? $"* Next upgrade: {_upgradeRecipe.OutputItem.Name}" : string.Empty;

			return $"* {hitValue} per hit<br>* {miningSpeed} mining speed<br>* {damage} damage<br>{upgradeText}";
		}
	}
}
