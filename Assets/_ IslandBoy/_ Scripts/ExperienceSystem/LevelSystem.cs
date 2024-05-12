using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class LevelSystem
	{
		public event Action OnLevelUp;
		public int _maxlevel;
		private int _currentLevel;
		private int _currentExp;
		public int[] _expPerLevel;
		public int MaxLevel => _maxlevel;
		public int CurrentLevel => _currentLevel;
		public int CurrentExp => _currentExp;
		public int[] ExpPerLevel => _expPerLevel;
		
		public LevelSystem(int maxLevel, AnimationCurve expPerLevel)
		{
			_expPerLevel = new int[maxLevel];
			_maxlevel = maxLevel;
			_currentLevel = 0;
			_currentExp = 0;
			
			for (int i = 0; i < _maxlevel; i++)
			{
				int yValue = (int)expPerLevel.Evaluate(i);
				_expPerLevel[i] = yValue;
			}
		}
		
		public void GainExperience(int exp)
		{
			_currentExp += exp;
			
			if(_currentExp >= _expPerLevel[_currentLevel + 1] && _currentLevel < _maxlevel)
			{
				LevelUp();
			}
		}
		
		private void LevelUp()
		{
			_currentLevel++;
			
			OnLevelUp?.Invoke();
		}
	}
}
