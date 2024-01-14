using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace IslandBoy
{
	public class Blacksmith : MonoBehaviour
	{
		[Header("Blacksmith Parameters")]
		[SerializeField] private PlayerObject _po;
		[SerializeField] private GameObject _tcSlotPrefab;
		[SerializeField] private RectTransform _slotHolder;
		[SerializeField] private RectTransform _upgradePanelHolder;
		[SerializeField] private UpgradeUI _upgradeUI;
		
		private Dictionary<ItemObject, CraftingRecipeObject>_upgradeDatabase = new();

		private void Awake()
		{
			GameSignals.INVENTORY_CLOSE.AddListener(CloseUI);
		}
		
		private void OnEnable()
		{
			StartCoroutine(ResetMenu());
		}

		private void OnDestroy()
		{
			GameSignals.INVENTORY_CLOSE.RemoveListener(CloseUI);
		}
		
		public void UpdateUpgradeDatabase()
		{
			_upgradePanelHolder.gameObject.SetActive(false);
			_upgradeDatabase.Clear();
			
			foreach (Slot item in _po.Inventory.InventorySlots)
			{
				if(item.ItemObject is ToolObject)
				{
					ToolObject tool = (ToolObject)item.ItemObject;
					
					if(tool.UpgradeRecipe != null)
					{
						_upgradeDatabase.Add(tool, tool.UpgradeRecipe);
					}
				}
			}
			
			if(_po.MouseSlot.ItemObject is ToolObject)
			{
				ToolObject tool = (ToolObject)_po.MouseSlot.ItemObject;
				
				if(tool.UpgradeRecipe != null)
				{
					_upgradeDatabase.Add(tool, tool.UpgradeRecipe);
				}
			}
		}
		
		public IEnumerator ResetMenu()
		{
			yield return new WaitForEndOfFrame();
			ResetCraftSlots();
			SetUpRecipes();
		}

		public void RefreshCraftingUI(CraftingRecipeObject recipe, ItemObject originalItem)
		{
			_upgradeUI.PopulateRecipe(recipe, originalItem);
		}

		private void SetUpRecipes()
		{
			UpdateUpgradeDatabase();
			
			foreach (var upgradeRecipe in _upgradeDatabase)
			{
				GameObject cs = Instantiate(_tcSlotPrefab, _slotHolder.transform);
				TCSlot tcSlot = cs.GetComponent<TCSlot>();
				tcSlot.Initialize(upgradeRecipe.Value, upgradeRecipe.Key);
			}
		}

		private void ResetCraftSlots()
		{
			if (_slotHolder.transform.childCount > 0)
			{
				foreach (Transform child in _slotHolder.transform)
				{
					Destroy(child.gameObject);
				}
			}
		}
		
		private void CloseUI(ISignalParameters parameters) => DisableUI();

		public void EnableUI() => gameObject.SetActive(true);

		public void DisableUI() => gameObject.SetActive(false);
	}
}
