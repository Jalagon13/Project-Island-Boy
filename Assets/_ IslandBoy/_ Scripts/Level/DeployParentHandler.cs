using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class DeployParentHandler : MonoBehaviour
	{
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private void OnDisable()
		{
			LevelControl.SetDP(null);
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			LevelControl.SetDP(gameObject);
		}
	}
}
