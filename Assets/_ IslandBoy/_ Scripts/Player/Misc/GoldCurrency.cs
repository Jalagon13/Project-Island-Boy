using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class GoldCurrency
	{
		public event Action ValueChanged;
		
		private int _currentValue = 0;
		
		public int CurrentValue { get => _currentValue; 
		set
		{
			_currentValue = value;
			if(_currentValue < 0)
				_currentValue = 0;
				
			UpdateCurrency();
		}}
		
		public void Increment(int amount)
		{
			_currentValue += amount;
			UpdateCurrency();
		}
		
		public void Decrement(int amount)
		{
			_currentValue -= amount;
			if(_currentValue < 0)
				_currentValue = 0;
				
			UpdateCurrency();
		}
		
		public void UpdateCurrency()
		{
			ValueChanged?.Invoke();
		}
	}
}
