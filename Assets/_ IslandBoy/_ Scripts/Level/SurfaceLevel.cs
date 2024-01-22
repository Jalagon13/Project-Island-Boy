using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class SurfaceLevel : MonoBehaviour
	{
		
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			AStarExtensions.Instance.GenerateBarriers();
		}
	}
}
