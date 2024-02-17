using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IslandBoy
{
	public class TransitionScene : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private string _nextScene;
		[SerializeField] private bool _enableColliderSceneSwitch;
		
		private Vector2 _returnPosition;
		private bool _isReturnPoint;
		
		private void OnEnable()
		{
			if(!_isReturnPoint) return;
			
			_po.GameObject.transform.position = _returnPosition;
			_isReturnPoint = false;
		}
		
		private void OnTriggerEnter2D(Collider2D other) 
		{
			if(!_enableColliderSceneSwitch) return;
			
			FeetTag ct = other.GetComponent<FeetTag>();
			
			if(ct != null)
			{
				SwitchScene();
			}
		}
		
		public void SwitchScene()
		{
			LevelControl.CaveLevelToLoad = 0;
			
			_returnPosition = _po.Position;
			_isReturnPoint = true;
			
			Signal signal = GameSignals.CHANGE_SCENE;
			signal.ClearParameters();
			signal.AddParameter("NextScene", _nextScene);
			signal.Dispatch();
		}
	}
}
