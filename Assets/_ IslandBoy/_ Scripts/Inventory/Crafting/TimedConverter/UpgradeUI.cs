using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class UpgradeUI : MonoBehaviour
	{
		[Header("Main Fields")]
		[SerializeField] private PlayerObject _po;
		[SerializeField] private ItemObject _upgradeStone;
		[SerializeField] private AudioClip _populateSound;
		[SerializeField] private AudioClip _craftSound;
		[Header("UI")]
		[SerializeField] private RectTransform _holder;
		[SerializeField] private GameObject _inventoryItemPrefab;
		[SerializeField] private Image _outputImage;
		[SerializeField] private TextMeshProUGUI _outputText;
		[SerializeField] private TextMeshProUGUI _ingredientText;
		[SerializeField] private TextMeshProUGUI _craftText;
		[Header("Buttons")]
		[SerializeField] private CraftBtn _craftBtn;

		private CraftingRecipeObject _recipeToDisplay;
		private ItemObject _originalItem;
		private Blacksmith _blacksmith;
		private MouseSlot _mouseItemHolder;
		private int _xpCost;

		private void OnEnable()
		{
			if (_holder == null) return;
			_holder.gameObject.SetActive(false);
		}

		public void PopulateRecipe(CraftingRecipeObject recipeObject, ItemObject originalItem, int xpCost)
		{
			_blacksmith = transform.parent.GetComponent<Blacksmith>();
			_originalItem = originalItem;
			_holder.gameObject.SetActive(true);
			_recipeToDisplay = recipeObject;
			_outputText.text = $"{_recipeToDisplay.OutputItem.Name}";
			_outputImage.sprite = recipeObject.OutputItem.UiDisplay;
			_xpCost = xpCost;

			MMSoundManagerSoundPlayEvent.Trigger(_populateSound, MMSoundManager.MMSoundManagerTracks.UI, default);

			UpdateTexts();
		}

		public void UpgradeButton()
		{
			_mouseItemHolder = _po.MouseSlot;
			int amount = _recipeToDisplay.OutputAmount;
			
			if (_mouseItemHolder.TryToCraftItem(_inventoryItemPrefab, _recipeToDisplay.OutputItem, amount))
			{
				_po.Inventory.RemoveItem(_originalItem, 1);
				
				foreach (ItemAmount ia in _recipeToDisplay.ResourceList)
				{
					_po.Inventory.RemoveItem(ia.Item, ia.Amount);
				}

				MMSoundManagerSoundPlayEvent.Trigger(_craftSound, MMSoundManager.MMSoundManagerTracks.UI, transform.position);
				GameSignals.ITEM_CRAFTED.Dispatch();
				
				// StartCoroutine(_blacksmith.ResetMenu());
				_blacksmith.UpdateUpgradeList(_originalItem);
				UpdateTexts();
			}
		}

		public void ResetCraftingUI()
		{
			UpdateTexts();
		}

		private void UpdateTexts()
		{
			UpdateIngTexts();
			UpdateButtons();
			UpgradeCraftText();
		}

		private void UpdateButtons()
		{
			if (CheckIfCanCraft())
			{
				_craftBtn.EnableButton();
			}
			else
			{
				_craftBtn.DisableButton();
			}
		}

		private void UpdateIngTexts()
		{
			int xpVal = PlayerGoldController.Instance.CurrencyValue;
			
			string ingText = "Recipe:<br>";
			
			// if(_po.Inventory.GetItemAmount(_originalItem) >= 1)
			// 	ingText += $"<color=white>{_originalItem.Name} [{_po.Inventory.GetItemAmount(_originalItem)}/{1}]<color=white><br>";
			// else
			// 	ingText += $"<color=red>{_originalItem.Name} [{_po.Inventory.GetItemAmount(_originalItem)}/{1}]<color=red><br>";
				
			if(_po.Inventory.GetItemAmount(_upgradeStone) >= 1)
				ingText += $"<color=white>{_upgradeStone.Name} [{_po.Inventory.GetItemAmount(_upgradeStone)}/{1}]<color=white><br>";
			else
				ingText += $"<color=red>{_upgradeStone.Name} [{_po.Inventory.GetItemAmount(_upgradeStone)}/{1}]<color=red><br>";
				
			ingText += xpVal >= _xpCost ? 
			$"<color=white>XP [{xpVal}/{_xpCost}]<color=white><br>" : 
			$"<color=red>XP [{xpVal}/{_xpCost}]<color=red><br>";
			
			foreach (var ia in _recipeToDisplay.ResourceList)
			{
				string text = $"{ ia.Item.Name} [{_po.Inventory.GetItemAmount(ia.Item)}/{ia.Amount}]";

				if(_po.Inventory.GetItemAmount(ia.Item) >= ia.Amount)
					ingText += $"<color=white>{text}<color=white><br>";
				else
					ingText += $"<color=red>{text}<color=red><br>";

			}

			_ingredientText.text = ingText;
		}

		private void UpgradeCraftText()
		{
			_craftText.text = $"Upgrade";
		}

		private bool CheckIfCanCraft()
		{
			bool hasAllIngredients = false;
			foreach (ItemAmount ia in _recipeToDisplay.ResourceList)
			{
				hasAllIngredients = _po.Inventory.Contains(ia.Item, ia.Amount);

				if (!hasAllIngredients) break;
			}
			
			bool hasEnoughXp = false;
			if(PlayerGoldController.Instance.CurrencyValue >= _xpCost)
				hasEnoughXp = true;
			
			bool hasOriginalItem = false;
			if(_po.Inventory.Contains(_originalItem, 1))
				hasOriginalItem = true;
				
			bool hasUpgradeStone = false;
			if(_po.Inventory.Contains(_upgradeStone, 1))
				hasUpgradeStone = true;

			bool resourceListZero = false;
			if (_recipeToDisplay.ResourceList.Count <= 0)
				resourceListZero = true;

			if(resourceListZero)
				return true;
				
			if(hasAllIngredients && hasEnoughXp && hasOriginalItem && hasUpgradeStone)
				return true;
				
			return false;
		}
	}
}
