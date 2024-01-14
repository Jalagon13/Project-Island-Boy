using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEngine;

namespace IslandBoy
{
	public class NavmeshSceneChange : MonoBehaviour
	{
		private void OnEnable()
		{
			StartCoroutine(Delay());
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			NavMeshSurface nms = GetComponent<NavMeshSurface>();
			nms.hideEditorLogs = true;
			nms.BuildNavMeshAsync();
		}
	}
}
