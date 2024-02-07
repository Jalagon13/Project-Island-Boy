using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class Environment : MonoBehaviour
	{
		[SerializeField] private int _width;
		[SerializeField] private int _height;
		[SerializeField] private Vector3 _center;
		
		
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			AStarExtensions.Instance.ReScanNodeGraph(_width, _height, _center);
		}
	}
}
