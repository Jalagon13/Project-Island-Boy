using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	// dynamicalls alters pathfinding area
	public class GroundCollider : MonoBehaviour
	{
		private bool _applicationQuitting;

		private void OnEnable()
		{
			StartCoroutine(UpdatePathfinding());
		}

		private void OnDisable()
		{
			if (_applicationQuitting) return;

			Bounds updateBounds = new(transform.parent.position, new(2, 2, 1));
			
			if(AstarPath.active != null)
			{
				AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
			}
		}

		// private void OnDestroy()
		// {
		// 	if (_applicationQuitting) return;

		// 	StartCoroutine(UpdatePathfinding());
		// }

		private void OnApplicationQuit()
		{
			_applicationQuitting = true;
		}

		private IEnumerator UpdatePathfinding()
		{
			Bounds updateBounds = new(transform.parent.position, new(2, 2, 1));
			
			if(AstarPath.active != null)
			{
				AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
			}
			
			yield return new WaitForEndOfFrame();
			
			
		}
	}
}
