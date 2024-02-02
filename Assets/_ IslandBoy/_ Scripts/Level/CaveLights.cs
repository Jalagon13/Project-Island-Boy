using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class CaveLights : MonoBehaviour
	{
		[SerializeField] private GameObject _normalLights;
		[SerializeField] private GameObject _levelCompleteLights;
		
		private void Awake() 
		{
			GameSignals.MONSTER_HEART_CLEARED.AddListener(UpdateLights);
		}
		
		private void UpdateLights(ISignalParameters parameters)
		{
			_normalLights.SetActive(false);
			_levelCompleteLights.SetActive(true);
			
			GameSignals.MONSTER_HEART_CLEARED.RemoveListener(UpdateLights);
		}
	}
}
