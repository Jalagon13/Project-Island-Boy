using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public abstract class ItemObject : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public Sprite UiDisplay { get; private set; }
		[field: SerializeField] public bool Stackable { get; private set; }
		[field: SerializeField] public CraftingRecipeObject UpgradeRecipe { get; private set; }
		[field: SerializeField] public int XpUpgradeCost { get; private set; }
		[field: TextArea]
		[field: SerializeField] public string Description { get; private set; }
		[field: SerializeField] public string SubDescription { get; private set; }

		[field: SerializeField] public List<ItemParameter> DefaultParameterList { get; set; }

		public abstract ToolType ToolType { get; }
		public abstract ToolTier ToolTier { get; }
		public abstract ArmorType ArmorType { get; }
		public abstract AccessoryType AccessoryType { get; }
		public abstract void ExecutePrimaryAction(FocusSlotControl control);
		public abstract void ExecuteSecondaryAction(FocusSlotControl control);
		public abstract string GetDescription();

		protected ToolType _baseToolType = ToolType.None;
		protected ToolTier _baseToolTier = ToolTier.None;
		protected ArmorType _baseArmorType = ArmorType.None;
		protected AccessoryType _baseAccessoryType = AccessoryType.None;
		protected string textBlueColor = "#b3e6ff";
		protected string textHpColor = "#ffbebe";
		protected string textManaColor = "#dcbeff";
		protected string textEnergyColor = "#ffe7b8";

		protected string GetDescriptionBreak() // returns description with line breaks
		{
			string description = "";
			if (!string.IsNullOrWhiteSpace(Description))
				description += $"{Description}<br>";
			
			if (!string.IsNullOrWhiteSpace(SubDescription))
				description += $"<color={textBlueColor}>{SubDescription}</color={textBlueColor}><br>";

			return description;
		}
	}

	[Serializable]
	public struct ItemParameter : IEquatable<ItemParameter>
	{
		public ItemParameterObject Parameter;
		public float Value;

		public bool Equals(ItemParameter other)
		{
			return other.Parameter == Parameter;
		}
	}
}
