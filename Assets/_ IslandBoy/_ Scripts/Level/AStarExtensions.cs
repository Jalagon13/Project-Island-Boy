using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

namespace IslandBoy
{
	public class AStarExtensions : MonoBehaviour
	{
		private static AStarExtensions _instance;
		
		public static AStarExtensions Instance => _instance;
		
		private void Awake()
		{
			_instance = this;
		}
		
		public void UpdatePathfinding(Vector3Int target, Vector3 size)
		{
			StartCoroutine(Delay(target.x, target.y, target.z, size));
		}
		
		public void UpdatePathfinding(Vector3 target, Vector3 size)
		{
			StartCoroutine(Delay(target.x, target.y, target.z, size));
		}
		
		private static IEnumerator Delay(float targetx, float targety, float targetz, Vector3 size)
		{
			yield return new WaitForSeconds(0.15f);
			
			Bounds bounds = new()
			{
				center = new(targetx, targety, targetz),
				size = size
			};
			var guo = new GraphUpdateObject(bounds);
			AstarPath.active.UpdateGraphs(guo);
		}
	}
}
