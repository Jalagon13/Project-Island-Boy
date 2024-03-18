using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public enum ArmorType
	{
		None,
		Head,
		Chest,
		Legs
	}

	[CreateAssetMenu(fileName = "New Armor Object", menuName = "Create Item/New Armor Object")]
	public class ArmorObject : ItemObject
	{
		[SerializeField] private ArmorType _armorType;
		[SerializeField] private List<Sprite> _spriteList;

		public override ToolType ToolType => _baseToolType;
		public override ToolTier ToolTier => _baseToolTier;
		public override ArmorType ArmorType => _armorType;
		public override AccessoryType AccessoryType => _baseAccessoryType;
		public List<Sprite> SpriteList => _spriteList;

		public override void ExecutePrimaryAction(FocusSlotControl control)
		{
			
		}

		public override void ExecuteSecondaryAction(FocusSlotControl control)
		{

		}

		public override string GetDescription()
		{
			float defense = 0;
			
			foreach (var item in DefaultParameterList)
			{
				switch (item.Parameter.ParameterName)
				{
					case "Defense":
						defense = item.Value;
						break;
				}
			}
			string upgradeText = UpgradeRecipe != null ? $"<br>* Upgradable" : string.Empty;
			return $"{GetDescriptionBreak()}<color={textBlueColor}>* {_armorType} Armor<br>* {defense} defense{upgradeText}</color={textBlueColor}>";
		}
	}
}
