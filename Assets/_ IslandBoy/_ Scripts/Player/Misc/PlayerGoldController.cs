using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace IslandBoy
{
	public class PlayerGoldController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _goldViewText;
		
		[Button]
		private void PlusHundred()
		{
			AddCurrency(100);
		}
		
		private static GoldCurrency _currency = new();
		
		public static int CurrencyValue { get { return _currency.CurrentValue; } }
		
		private void Start()
		{
			if(_currency != null)
			{
				_currency.ValueChanged += OnCurrencyChanged;
			}
		}
		
		private void OnDestroy()
		{
			if(_currency != null)
			{
				_currency.ValueChanged -= OnCurrencyChanged;
			}
		}
		
		public static void AddCurrency(int amount)
		{
			_currency?.Increment(amount);
		}
		
		public static void SubtractCurrency(int amount)
		{
			_currency?.Decrement(amount);
		}
		
		private void UpdateView()
		{
			if(_currency == null)
				return;
			
			if(_goldViewText != null)
			{
				_goldViewText.text = _currency.CurrentValue.ToString();
			}
		}
		
		private void OnCurrencyChanged()
		{
			UpdateView();
		}
	}
}
