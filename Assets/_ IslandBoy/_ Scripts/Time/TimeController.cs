using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class TimeController : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private float _maxTime;
		
		private TimeView _timeView;
		private float _currentTime;
		public DayCycleHandler DayCycleHandler { get; set; }
		public float CurrentDayRatio => _currentTime / _maxTime;
		private bool _canIncrementTime = false;
		
		private void Awake()
		{
			_po.TimeController = this;
			_currentTime = 0;
			
			GameSignals.CLICKABLE_DESTROYED.AddListener(IncrementTime);
			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(EnableTimeIncrement);
			GameSignals.DAY_END.AddListener(OnDayEnd);
			GameSignals.DAY_START.AddListener(OnDayStart);
		}
		
		private void OnDestroy()
		{
			GameSignals.CLICKABLE_DESTROYED.RemoveListener(IncrementTime);
			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(EnableTimeIncrement);
			GameSignals.DAY_END.RemoveListener(OnDayEnd);
			GameSignals.DAY_START.RemoveListener(OnDayStart);
		}
		
		private IEnumerator Start()
		{
			// Future note to self: This may cause some issues when creating a scene loading bootstrap
			_timeView = FindObjectOfType<TimeView>();
			if (_timeView != null)
			{
				_timeView.Initialize();
				UpdateView();
			}
			yield return new WaitForEndOfFrame();
            if (_timeView != null)
                DayCycleHandler.Tick();
		}
		
		public bool NoMoreEnergy()
		{
			return _currentTime >= _maxTime;
		}
		
		private void EnableTimeIncrement(ISignalParameters parameters)
		{
			_canIncrementTime = true;
		}
		
		private void IncrementTime(ISignalParameters parameters)
		{
			if(!_canIncrementTime) return;
			
			SkillCategory skill = (SkillCategory)parameters.GetParameter("SkillCategory");
			int baseTimeAmount = (int)parameters.GetParameter("TimeAmount");
			int expAmount = (int)parameters.GetParameter("ExpAmount");
			
			if(skill != SkillCategory.None && expAmount > 0)
			{
				// Efficency Calculation
				int currentSkillLevel = _po.PlayerExperience.ExperienceModel.GetSkillCategory(skill).LevelSystem.CurrentLevel;
				float baseTakeAwayMultiplier = 0.1f;
				float subAmount = baseTakeAwayMultiplier * currentSkillLevel;
				float totalTimeToAdd = baseTimeAmount - subAmount;
				
				_currentTime += totalTimeToAdd;
				DayCycleHandler.Tick();
				
				if(_currentTime > _maxTime)
				{
					_currentTime = _maxTime;
					UpdateView();
					// End of day stuff here
				}
				
				UpdateView();
			}
		}
		
		private void UpdateView()
		{
			_timeView.UpdateTime(_currentTime, _maxTime);
		}
		
		private void OnDayStart(ISignalParameters parameters)
		{
			_currentTime = 0;
			DayCycleHandler.Tick();
			UpdateView();
		}
		
		private void OnDayEnd(ISignalParameters parameters)
		{
			
		}
	}
}
