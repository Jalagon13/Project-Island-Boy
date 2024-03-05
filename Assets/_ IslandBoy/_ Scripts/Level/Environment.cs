using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
	public class Environment : MonoBehaviour
	{
		[SerializeField] private int _width;
		[SerializeField] private int _height;
		[SerializeField] private Vector3 _center;
		[Header("BGM/A")]
		[SerializeField] private MMF_Player _enviornmentSoundFeedback;
		
		private bool _firstTimeLoad;
		
		private void Start()
		{
			if(!_firstTimeLoad)
			{
				_firstTimeLoad = true;
				StartCoroutine(Delay());
			}
		}
		
		private void OnEnable()
		{
			// SceneManager.sceneLoaded += OnSceneLoaded;
			
			if(_firstTimeLoad)
			{
				StartCoroutine(Delay());
			}
			
		}
		
		private void OnDisable()
		{
			// SceneManager.sceneLoaded -= OnSceneLoaded;
			
			_enviornmentSoundFeedback?.StopFeedbacks();
			StopAllCoroutines();
		}
		
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			
			AStarExtensions.Instance.ReScanNodeGraph(_width, _height, _center);
			
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
