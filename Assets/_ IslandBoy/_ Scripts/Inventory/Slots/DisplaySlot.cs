using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
	public class DisplaySlot : MonoBehaviour
	{
		[SerializeField] private ItemObject _displayItem;
		[SerializeField] private int _displayAmount;
		
		private Image _image;
		private TextMeshProUGUI _amountText;
		
		private void Awake()
		{
			_image = GetComponent<Image>();
			_amountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		}
		
		private void Start()
		{
			_image.sprite = _displayItem.UiDisplay;
			_amountText.text = _displayAmount.ToString();
			GetComponent<RscSlotImageHover>().SetCustomDescription(_displayItem.Description, _displayItem.Name);
		}
	}
}
