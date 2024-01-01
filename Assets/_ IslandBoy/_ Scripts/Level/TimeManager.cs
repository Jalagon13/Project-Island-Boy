using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class TimeManager : MonoBehaviour
	{
		public float CurrentDayRatio => _currentTimeOfTheDay / DayDurationInSeconds;
		public DayCycleHandler DayCycleHandler { get; set; }
		
		[Header("Time settings")]
		public float DayDurationInSeconds;
		public float StartingTime = 0.0f;
		
		private static TimeManager _instance;
		private float _currentTimeOfTheDay;
		private bool _isTicking;
		
		public static TimeManager Instance { get { return _instance; } } 
		
		private void Awake()
		{
			_instance = this;
			_isTicking = true;
			_currentTimeOfTheDay = StartingTime;
		}
		
		private void OnEnable()
		{
			GameSignals.GAME_PAUSED.AddListener(Pause);
			GameSignals.GAME_UNPAUSED.AddListener(Resume);
			GameSignals.DAY_START.AddListener(OnDayStart);
		}
		
		private void OnDisable()
		{
			GameSignals.GAME_PAUSED.RemoveListener(Pause);
			GameSignals.GAME_UNPAUSED.RemoveListener(Resume);
			GameSignals.DAY_START.RemoveListener(OnDayStart);
		}
		
		private void Update()
		{
			if(_isTicking)
			{
				_currentTimeOfTheDay += Time.deltaTime;
				
				if(DayCycleHandler != null)
					DayCycleHandler.Tick();
					
				if(_currentTimeOfTheDay > DayDurationInSeconds)
				{
					EndDay();
				}
			}
		}
		
		private void OnDayStart(ISignalParameters parameters)
		{
			_currentTimeOfTheDay = 0;
			Resume(null);
		}
		
		private void EndDay()
		{
			Pause(null);
			GameSignals.DAY_END.Dispatch();
		}
		
		private void Pause(ISignalParameters parameters)
		{
			_isTicking = false;
		}
		
		private void Resume(ISignalParameters parameters)
		{
			_isTicking = true;
		}
	}
}
