using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace IslandBoy
{
	public class StatsViewCanvas : MonoBehaviour
	{
		[SerializeField] private MMF_Player _tutorialFeedback;
		
		private void Start()
		{
			
		}
		
		private void Awake()
		{
			GameSignals.ENABLE_STARTING_MECHANICS.AddListener(EnableStatsView);
		}
		
		private void OnDestroy()
		{
			GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(EnableStatsView);
		}
		
		private void EnableStatsView(ISignalParameters parameters)
		{
			_tutorialFeedback?.PlayFeedbacks();
		}
	}
}
