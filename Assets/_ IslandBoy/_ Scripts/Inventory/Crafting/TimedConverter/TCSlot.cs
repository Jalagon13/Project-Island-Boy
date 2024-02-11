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
		private CraftingRecipeObject _recipe;
		private CraftSlotImageHover _hoverImage;
		private Image _outputImage;
		private TextMeshProUGUI _amountText;
		private ItemObject _originalItem;
		private Action<CraftingRecipeObject> _refreshAction;

		public void Initialize(CraftingRecipeObject recipe, Action<CraftingRecipeObject> refreshAction, ItemObject originalItem = null)
		{
			SetGlobals(recipe, originalItem);
			_recipe = recipe;
			_refreshAction = refreshAction;
		}

		private void SetGlobals(CraftingRecipeObject recipe, ItemObject originalItem)
		{
			_outputImage = transform.GetChild(0).GetComponent<Image>();
			_hoverImage = transform.GetChild(0).GetComponent<CraftSlotImageHover>();
			_amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
			_recipe = recipe;
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
				blacksmith.RefreshCraftingUI(_recipe, _originalItem);
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
