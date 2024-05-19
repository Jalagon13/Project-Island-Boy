using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  IslandBoy
{
	public class Category
	{
		public SkillCategory Skill;
		public int StoredExp; // stored Exp that will be added to the level system at the end of the day
		private LevelSystem _levelSystem;
		public LevelSystem LevelSystem => _levelSystem;
		
		public Category(SkillCategory skill, int maxLevel, AnimationCurve expPerLevel)
		{
			Skill = skill;
			_levelSystem = new(maxLevel, expPerLevel);
			StoredExp = 0;
		}
	}

	public class ExperienceModel
	{
		public event Action OnMultiplierUpdated;
		private List<Category> _skillCategories = new();
		private float _expMultiplier = 1f;
		
		public float ExpMultiplier => _expMultiplier;
		public List<Category> SkillCategories => _skillCategories;
		
		public ExperienceModel(List<SkillCategory> categories, int maxLevel, AnimationCurve expPerLevel)
		{
			foreach(SkillCategory skill in categories)
			{
				// Create an internal category
				Category skillCategory = new(skill, maxLevel, expPerLevel);
				_skillCategories.Add(skillCategory);
			}
		}
		
		public void ResetMultiplier()
		{
			_expMultiplier = 1f;
			OnMultiplierUpdated?.Invoke();
		}
		
		public void AddToMultiplier(float multiplier)
		{
			_expMultiplier += multiplier;
			OnMultiplierUpdated?.Invoke();
		}
		
		public void IncrementStoredExp(SkillCategory skill, int expAmount)
		{
			Category category = GetSkillCategory(skill);
			
			if(category != null)
			{
				category.StoredExp += expAmount;
			}
			else
			{
				Debug.LogError($"Skill: {skill} could not be found");
			}
		}
		
		public Category GetSkillCategory(SkillCategory skill)
		{
			foreach (Category item in _skillCategories)
			{
				if(item.Skill == skill)
				{
					return item;
				}
			}
			
			return null;
		}
	}
}

