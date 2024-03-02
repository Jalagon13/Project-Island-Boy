using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
	public class FootStepPlayer : MonoBehaviour
	{
		[SerializeField] private MMF_Player _footStepFeedback;
		
		private bool _moving;
		
		private void OnEnable()
		{
			GameSignals.PLAYER_IS_MOVING.AddListener(Moving);
			GameSignals.PLAYER_IS_NOT_MOVING.AddListener(NotMoving);
		}

		private void OnDisable()
		{
			GameSignals.PLAYER_IS_MOVING.RemoveListener(Moving);
			GameSignals.PLAYER_IS_NOT_MOVING.RemoveListener(NotMoving);
		}
		
		private void Moving(ISignalParameters parameters)
		{
			if(!_moving)
			{
				_moving = true;
				Debug.Log(_moving);
				StartCoroutine(WalkingRoutine());
			}
		}
		
		private IEnumerator WalkingRoutine()
		{
			_footStepFeedback?.PlayFeedbacks();
			yield return new WaitForSeconds(0.28f);
			StartCoroutine(WalkingRoutine());
		}
		
		private void NotMoving(ISignalParameters parameters)
		{
			if(_moving)
			{
				_moving = false;
				Debug.Log(_moving);
				StopAllCoroutines();
			}
		}
	}
}
