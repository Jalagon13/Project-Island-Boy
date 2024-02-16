using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class TCSlot : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		private CraftingRecipeObject _recipe;
		private CraftSlotImageHover _hoverImage;
		private Image _outputImage;
		private TextMeshProUGUI _amountText;
		private ItemObject _originalItem;
		private Action<CraftingRecipeObject> _refreshAction;
		private int _xpCost;

		public void Initialize(CraftingRecipeObject recipe, Action<CraftingRecipeObject> refreshAction, ItemObject originalItem = null)
		{
			SetGlobals(recipe, originalItem, 5);
			_recipe = recipe;
			_refreshAction = refreshAction;
		}
		
		public void Initialize(ItemObject item)
		{
			SetGlobals(item.UpgradeRecipe, item, item.XpUpgradeCost);
			
			_outputImage.color = new(1, 1, 1, _po.Inventory.Contains(_originalItem, 1) ? 1f : 0.4f);
			_recipe = item.UpgradeRecipe;
			_refreshAction = null;
		}

		private void SetGlobals(CraftingRecipeObject recipe, ItemObject originalItem, int xpCost)
		{
			_outputImage = transform.GetChild(0).GetComponent<Image>();
			_hoverImage = transform.GetChild(0).GetComponent<CraftSlotImageHover>();
			_amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
			_recipe = recipe;
			_xpCost = xpCost;
			_originalItem = originalItem;
			_outputImage.sprite = recipe.OutputItem.UiDisplay;
			_hoverImage.SetItemDescription(recipe.OutputItem);
			_amountText.text = recipe.OutputAmount == 1 ? string.Empty : recipe.OutputAmount.ToString();
		}

		public void DisplayCraftingUI() // connected to this button
		{
			var blacksmith = transform.parent.parent.GetComponent<Blacksmith>();
			
			if(blacksmith != null && _originalItem != null)
			{
				blacksmith.RefreshCraftingUI(_recipe, _originalItem, _xpCost);
				return;
			}
			_refreshAction?.Invoke(_recipe);
			// var tc = transform.parent.parent.parent.GetComponent<TimedConverter>();
			
			// if(tc != null)
			// {
			// 	tc.RefreshCraftingUI(_recipe);
			// 	return;
			// }
		}
	}
}
