using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SkillCategory
{
	None,
	Mining, 
	Woodcutting,
	Gathering,
	Combat,
	Fishing
}
namespace IslandBoy
{
	public class ExperienceController : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private int _maxlevel;
		[SerializeField] private AnimationCurve _expPerLevel;
		
		private ExperienceView _experienceView;
		private ExperienceModel _experienceModel;
		public ExperienceModel ExperienceModel => _experienceModel;
		private TimeController _timeController;
		private bool _canIncrementExp = false;
		
		private void Awake()
		{
			_timeController = GetComponent<TimeController>();
			_po.PlayerExperience = this;
			var categories = new List<SkillCategory>(Enum.GetValues(typeof(SkillCategory)) as SkillCategory[]);
			_experienceModel = new(categories, _maxlevel, _expPerLevel);
			
			_experienceModel.OnMultiplierUpdated += OnMultiplierUpdated;
			GameSignals.CLICKABLE_DESTROYED.AddListener(AddExperience);
			GameSignals.DAY_START.AddListener(ResetMultiplier);
			GameSignals.HUNGER_RESTORED.AddListener(HungerRestored);
			GameSignals.PLAYER_DIED.AddListener(PlayerDied);
			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(EnableExpIncrement);
		}
		
		private void OnDestroy()
		{
			_experienceModel.OnMultiplierUpdated -= OnMultiplierUpdated;
			GameSignals.CLICKABLE_DESTROYED.RemoveListener(AddExperience);
			GameSignals.DAY_START.RemoveListener(ResetMultiplier);
			GameSignals.HUNGER_RESTORED.RemoveListener(HungerRestored);
			GameSignals.PLAYER_DIED.RemoveListener(PlayerDied);
			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(EnableExpIncrement);
		}
		
		private void Start()
		{
			_experienceView = FindObjectOfType<ExperienceView>();
		}
		
		private void EnableExpIncrement(ISignalParameters parameters)
		{
			_canIncrementExp = true;
		}
		
		private void PlayerDied(ISignalParameters parameters)
		{
			AddToMultiplier(-0.5f);
		}
		
		private void HungerRestored(ISignalParameters parameters)
		{
			AddToMultiplier(1);
		}
		
		private void ResetMultiplier(ISignalParameters parameters)
		{
			_experienceModel.ResetMultiplier();
		}
		
		private void AddExperience(ISignalParameters parameters)
		{
			if(!_canIncrementExp) return;
			if(!parameters.HasParameter("SkillCategory")) return;
			if(_timeController.NoMoreEnergy()) return;
			if(_experienceView == null)
                _experienceView = FindObjectOfType<ExperienceView>();

            SkillCategory skill = (SkillCategory)parameters.GetParameter("SkillCategory");
			int expAmount = (int)parameters.GetParameter("ExpAmount");
			
			if(skill != SkillCategory.None && expAmount > 0)
			{
				IncrementSkillExp(skill, expAmount);
				NotifyIncrementation(skill, expAmount);
			}
		}
		
		public void AddToMultiplier(float addedValue)
		{
			_experienceModel.AddToMultiplier(addedValue);
		}
		
		private void OnMultiplierUpdated()
		{
			_experienceView.UpdateMultiplierView(_experienceModel.ExpMultiplier);
		}
		
		private void NotifyIncrementation(SkillCategory skill, int expAmount)
		{
			_experienceView.NotifySkillExpGain(skill, expAmount);
		}
		
		private void IncrementSkillExp(SkillCategory skill, int expAmount)
		{
			// Communicate with model to increment skill
			_experienceModel.IncrementStoredExp(skill, expAmount);
		}
	}
}
