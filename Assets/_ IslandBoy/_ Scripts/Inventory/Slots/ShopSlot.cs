using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IslandBoy
{
	public class ShopSlot : Slot
	{
		[Header("Shop Slot")]
		[Required]
		[SerializeField] private MMF_Player _buyFeedback;
		[SerializeField] private ItemObject _sellItem;
		[SerializeField] private int _sellAmount;
		[SerializeField] private int _cost;
		
		private Image _image;
		private TextMeshProUGUI _itemText;
		private TextMeshProUGUI _amountText;
		private TextMeshProUGUI _costText;
		
		private void Awake()
		{
			_image = transform.GetChild(0).GetComponent<Image>();
			_itemText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
			_amountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
			_costText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
		}
		
		private void Start()
		{
			_image.sprite = _sellItem.UiDisplay;
			_itemText.text = _sellItem.Name;
			_amountText.text = _sellAmount.ToString();
			_costText.text = $"${_cost}";
		}
		
		public override void OnPointerClick(PointerEventData eventData)
		{
			if(eventData.button == PointerEventData.InputButton.Left)
			{
				if(_mouseItemHolder.HasItem())
				{
					if(_mouseItemHolder.ItemObject != _sellItem || !_mouseItemHolder.ItemObject.Stackable)
					{
						return;
					}
				}
				
				// if(PlayerGoldController.Instance.CurrencyValue >= _cost)
				// {
				// 	ExecuteTransaction();
				// }
			}
		}
		
		private void ExecuteTransaction()
		{
			_buyFeedback?.PlayFeedbacks();
			
			if(_mouseItemHolder.ItemObject == _sellItem)
				_mouseItemHolder.InventoryItem.Count += _sellAmount;
			else
				_mouseItemHolder.CreateMouseItem(_inventoryItemPrefab, _sellItem, _sellAmount);
		}
	}
}
