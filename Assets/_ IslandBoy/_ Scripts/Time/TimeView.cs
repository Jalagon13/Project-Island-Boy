using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
	public class TimeView : MonoBehaviour
	{
		[SerializeField] private Image _fillImage;
		[SerializeField] private RectTransform _dayTransitionPanel;
		[SerializeField] private MMF_Player _UpdateTimeFeedbacks;
		
		private void Awake()
		{
			GameSignals.DAY_END.AddListener(ExecuteEndDay);
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_END.RemoveListener(ExecuteEndDay);
		}
		
		public void Initialize()
		{
			_dayTransitionPanel.gameObject.SetActive(false);
		}
		
		private void ExecuteEndDay(ISignalParameters parameters)
		{
			_dayTransitionPanel.gameObject.SetActive(true);
		}
		
		public void UpdateTime(float maxTime, float currentTime)
		{
			_fillImage.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(0, currentTime, maxTime));
			_UpdateTimeFeedbacks?.PlayFeedbacks();
			// Test this next time you open this up and then make the XP stuff too
		}
		
		public void EndDay() // Connected to end day button
		{
			GameSignals.DAY_END.Dispatch();
		}
		
		public void StartDay() // Connected to start day button
		{
			GameSignals.DAY_START.Dispatch();
			
			_dayTransitionPanel.gameObject.SetActive(false);
		}
	}
}
