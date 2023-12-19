using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IslandBoy
{
	public class OverrideGridGraph : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		private AstarPath _ap;

		private void OnEnable()
		{
			StartCoroutine(SceneStartup());
		}
		
		private IEnumerator SceneStartup()
		{
			yield return new WaitForSeconds(1.25f);

			GetComponent<AstarPath>().Scan();
		}
	}
}
