using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace IslandBoy
{
	public class TransitionScene : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private string _nextScene;
		[SerializeField] private bool _enableColliderSceneSwitch;
		[SerializeField] private MMF_Player _transitionFeedback;
		[SerializeField] private Transform _returnPoint;
		
		private bool _isReturnPoint;
		private Collider2D _transitionCollider;
		
		private void Awake()
		{
			_transitionCollider = GetComponent<Collider2D>();
		}
		
		private void OnEnable()
		{
			if(!_isReturnPoint) return;
			
			
			_isReturnPoint = false;
			
			StartCoroutine(ColliderDelay());
		}
		
		private IEnumerator ColliderDelay()
		{
			yield return new WaitForSeconds(.5f);
			
			_po.GameObject.transform.SetPositionAndRotation(_returnPoint.position, Quaternion.identity);
			yield return new WaitForSeconds(.5f);
			if(_transitionCollider != null)
				_transitionCollider.enabled = true;
		}
		
		private void OnTriggerEnter2D(Collider2D other) 
		{
			if(!_enableColliderSceneSwitch) return;
			
			FeetTag ct = other.GetComponent<FeetTag>();
			
			if(ct != null)
			{
				PlayTransitionFeedback();
			}
		}
		
		public void PlayTransitionFeedback()
		{
			GameSignals.PLAYER_IS_NOT_MOVING.Dispatch();
			GameSignals.SCENE_TRANSITION_START.Dispatch();
			_transitionFeedback?.PlayFeedbacks();
		}
		
		public void SwitchScene()
		{
			if(_transitionCollider != null)
				_transitionCollider.enabled = false;
			
			LevelControl.CaveLevelToLoad = 0;
			
			_isReturnPoint = true;
			
			Signal signal = GameSignals.CHANGE_SCENE;
			signal.ClearParameters();
			signal.AddParameter("NextScene", _nextScene);
			signal.Dispatch();
		}
	}
}
