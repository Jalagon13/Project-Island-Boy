using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEngine;

namespace IslandBoy
{
	public class NavmeshSceneChange : MonoBehaviour
	{
		// private void Start()
		// {
		// 	NavMeshSurface nms = GetComponent<NavMeshSurface>();
		// 	nms.BuildNavMeshAsync();
		// }
		
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			NavMeshSurface nms = GetComponent<NavMeshSurface>();
			Debug.Log(nms == null);
			nms.BuildNavMeshAsync();
		}
	}
}
