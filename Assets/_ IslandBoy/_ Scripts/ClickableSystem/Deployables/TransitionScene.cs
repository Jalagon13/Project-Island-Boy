using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

namespace IslandBoy
{
	public class TransitionScene : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private string _nextScene;
		
		private Vector2 _returnPosition;
		private bool _isReturnPoint;
		
		private void OnEnable()
		{
			if(!_isReturnPoint) return;
			
			_po.GameObject.transform.position = _returnPosition;
			_isReturnPoint = false;
		}
		
		public void SwitchScene()
		{
			_returnPosition = _po.Position;
			_isReturnPoint = true;
			
			Signal signal = GameSignals.CHANGE_SCENE;
			signal.ClearParameters();
			signal.AddParameter("NextScene", _nextScene);
			signal.Dispatch();
		}
	}
}
