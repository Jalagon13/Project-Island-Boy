using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class TimeManager : MonoBehaviour
	{
		[SerializeField] private RectTransform _sunMoonMarker;
		[SerializeField] private Sprite _sunSprite;
		[SerializeField] private Sprite _moonSprite;
		[SerializeField] private bool _stopTime = false;
		[SerializeField] private MMF_Player _tutorialFeedback;
		
		public float CurrentDayRatio => _currentTimeOfTheDay / DayDurationInSeconds;
		public DayCycleHandler DayCycleHandler { get; set; }
		
		[Header("Time settings")]
		public float DayDurationInSeconds;
		public float StartingTime = 0.0f;
		
		private static TimeManager _instance;
		private float _currentTimeOfTheDay;
		private bool _isTicking;
		private bool _canStartNight;
		
		public static TimeManager Instance { get { return _instance; } } 
		
		private void Awake()
		{
			_instance = this;
			_isTicking = true;
			_currentTimeOfTheDay = StartingTime;
			_canStartNight = true;
		}
		
		private void OnEnable()
		{
			GameSignals.GAME_PAUSED.AddListener(Pause);
			GameSignals.GAME_UNPAUSED.AddListener(Resume);
			GameSignals.DAY_START.AddListener(OnDayStart);
			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(StartTime);
		}
		
		private void OnDisable()
		{
			GameSignals.GAME_PAUSED.RemoveListener(Pause);
			GameSignals.GAME_UNPAUSED.RemoveListener(Resume);
			GameSignals.DAY_START.RemoveListener(OnDayStart);
			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(StartTime);
		}
		
		private void Update()
		{
			if(_isTicking && !_stopTime)
			{
				_currentTimeOfTheDay += Time.deltaTime;
				
				if(DayCycleHandler != null)
					DayCycleHandler.Tick();
					
				if(_currentTimeOfTheDay > (DayDurationInSeconds * 0.53) && _canStartNight)
				{
					OnNightStart();
					_canStartNight = false;
				}
					
				if(_currentTimeOfTheDay > DayDurationInSeconds)
				{
					EndDay();
				}
			}
		}
		
		private void StartTime(ISignalParameters parameters)
		{
			_stopTime = false;
			_tutorialFeedback?.PlayFeedbacks();
		}
		
		private void OnDayStart(ISignalParameters parameters)
		{
			_canStartNight = true;
			_sunMoonMarker.GetComponent<Image>().sprite = _sunSprite;
			_currentTimeOfTheDay = 0;
			Resume(null);
		}
		
		private void OnNightStart()
		{
			_sunMoonMarker.GetComponent<Image>().sprite = _moonSprite;
			GameSignals.NIGHT_START.Dispatch();
		}
		
		private void EndDay()
		{
			Pause(null);
			Player.RESTED_STATUS = RestedStatus.Bad;
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
