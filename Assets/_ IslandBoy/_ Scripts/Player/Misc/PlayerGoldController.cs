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
		
		private static int _changeAmount;
		
		private static GoldCurrency _currency = new();
		
		public static int CurrencyValue { get { return _currency.CurrentValue; } }
		
		private void Start()
		{
			if(_currency != null)
			{
				_currency.ValueIncreased += OnCurrencyIncrease;
				_currency.ValueDecreased += OnCurrencyDecrease;
			}
		}
		
		private void OnDestroy()
		{
			if(_currency != null)
			{
				_currency.ValueIncreased += OnCurrencyIncrease;
				_currency.ValueDecreased += OnCurrencyDecrease;
			}
		}
		
		public static void AddCurrency(int amount)
		{
			_changeAmount = amount;
			_currency?.Increment(amount);
		}
		
		public static void SubtractCurrency(int amount)
		{
			_changeAmount = amount;
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
		
		private void OnCurrencyIncrease()
		{
			PopupMessage.Create(transform.position, $"+${_changeAmount}", Color.green, Vector2.up, 1f);
			
			UpdateView();
		}
		
		private void OnCurrencyDecrease()
		{
			PopupMessage.Create(transform.position, $"-${_changeAmount}", Color.red, Vector2.up, 1f);
			
			UpdateView();
		}
	}
}
