using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IslandBoy
{
	public class Confiner : MonoBehaviour
	{
		private PolygonCollider2D _confiner;
		
		private void Awake() 
		{
			_confiner = GetComponent<PolygonCollider2D>();
		}
		
		private void OnEnable() 
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			Signal signal = GameSignals.CONFINER_UPDATED;
			signal.ClearParameters();
			signal.AddParameter("Confiner", _confiner);
			signal.Dispatch();
		}
	}
}
