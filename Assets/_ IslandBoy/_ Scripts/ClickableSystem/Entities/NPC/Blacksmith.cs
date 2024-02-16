using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace IslandBoy
{
	public class Blacksmith : MonoBehaviour
	{
		[Header("Blacksmith Parameters")]
		[SerializeField] private PlayerObject _po;
		// [SerializeField] private NpcUpgradeType _upgradeType;
		[SerializeField] private GameObject _tcSlotPrefab;
		[SerializeField] private RectTransform _slotHolder;
		[SerializeField] private RectTransform _upgradePanelHolder;
		[SerializeField] private UpgradeUI _upgradeUI;
		[SerializeField] private List<ItemObject> _startingUpgradeList;
		
		private List<ItemObject> _persistantUpgradeList;

		private void Awake()
		{
			_persistantUpgradeList = new();
			_persistantUpgradeList = _startingUpgradeList;
			
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
			_persistantUpgradeList.Clear();
			_persistantUpgradeList = new();
			
			if(_po.MouseSlot.ItemObject is ToolObject)
			{
				ItemObject item = _po.MouseSlot.ItemObject;
				
				if(item.UpgradeRecipe != null)
				{
					if(!_persistantUpgradeList.Contains(item) /* && tool.NpcUpgradeType == _upgradeType */)
						_persistantUpgradeList.Add(item);
				}
			}
			
			foreach (Slot item in _po.Inventory.InventorySlots)
			{
				if(item.ItemObject is ToolObject)
				{
					ItemObject i = _po.MouseSlot.ItemObject;
					
					if(i.UpgradeRecipe != null)
					{
						if(!_persistantUpgradeList.Contains(i) /* && tool.NpcUpgradeType == _upgradeType */)
						_persistantUpgradeList.Add(i);
					}
				}
			}
		}
		
		private IEnumerator ResetMenu()
		{
			yield return new WaitForEndOfFrame();
			ResetCraftSlots();
			SetUpRecipes();
		}
		
		public void UpdateUpgradeList(ItemObject originalItem)
		{
			if(_persistantUpgradeList.Contains(originalItem))
			{
				var index = _persistantUpgradeList.IndexOf(originalItem);
				_persistantUpgradeList[index] = originalItem.UpgradeRecipe.OutputItem;
				
				StartCoroutine(ResetMenu());
			}
		}

		public void RefreshCraftingUI(CraftingRecipeObject recipe, ItemObject originalItem, int xpCost)
		{
			_upgradeUI.PopulateRecipe(recipe, originalItem, xpCost);
		}

		private void SetUpRecipes()
		{
			//UpdateUpgradeDatabase();
			
			foreach (ItemObject item in _persistantUpgradeList)
			{
				GameObject cs = Instantiate(_tcSlotPrefab, _slotHolder.transform);
				TCSlot tcSlot = cs.GetComponent<TCSlot>();
				tcSlot.Initialize(item);
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
