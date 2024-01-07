using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class GoldCurrency
	{
		public event Action ValueIncreased;
		public event Action ValueDecreased;
		
		private int _currentValue = 0;
		
		public int CurrentValue { get => _currentValue; 
		set
		{
			_currentValue = value;
			if(_currentValue < 0)
				_currentValue = 0;
		}}
		
		public void Reset()
		{
			_currentValue = 0;
			UpdateCurrencyDecrease();
		}
		
		public void Increment(int amount)
		{
			_currentValue += amount;
			UpdateCurrencyIncrease();
		}
		
		public void Decrement(int amount)
		{
			_currentValue -= amount;
			if(_currentValue < 0)
				_currentValue = 0;
				
			UpdateCurrencyDecrease();
		}
		
		public void UpdateCurrencyIncrease()
		{
			ValueIncreased?.Invoke();
		}
		public void UpdateCurrencyDecrease()
		{
			ValueDecreased?.Invoke();
		}
	}
}
