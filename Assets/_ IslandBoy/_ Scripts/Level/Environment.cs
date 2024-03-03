using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace IslandBoy
{
	public class Environment : MonoBehaviour
	{
		[SerializeField] private int _width;
		[SerializeField] private int _height;
		[SerializeField] private Vector3 _center;
		[Header("BGM/A")]
		[SerializeField] private MMF_Player _enviornmentSoundFeedback;
		
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private void OnDisable()
		{
			_enviornmentSoundFeedback?.StopFeedbacks();
			StopAllCoroutines();
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			AStarExtensions.Instance.ReScanNodeGraph(_width, _height, _center);
			Debug.Log("Test");
			yield return new WaitForSeconds(.75f);
			_enviornmentSoundFeedback?.PlayFeedbacks();
			// yield return new WaitForSeconds(.25f);
			// _enviornmentSoundFeedback?.StopFeedbacks();
			// yield return new WaitForSeconds(.25f);
			// _enviornmentSoundFeedback?.PlayFeedbacks();
			GameSignals.SCENE_TRANSITION_END.Dispatch();
		}
	}
}
