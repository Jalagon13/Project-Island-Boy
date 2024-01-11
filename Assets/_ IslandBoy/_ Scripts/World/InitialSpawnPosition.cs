using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class InitialSpawnPosition : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private bool _onInitialEnable = true;
		
		private void OnEnable()
		{
			if(!_onInitialEnable) return;
			
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			
			_po.GameObject.transform.position = transform.position;
			_onInitialEnable = false;
		}
	}
}
