using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class CaveLevel : MonoBehaviour
	{
		private void OnEnable() 
		{
			GameSignals.MONSTER_KILLED.AddListener(IncrementMonsterMeter);
		}
		
		private void OnDisable()
		{
			GameSignals.MONSTER_KILLED.AddListener(IncrementMonsterMeter);
		}
		
		private void IncrementMonsterMeter(ISignalParameters parameters)
		{
			// _currentQuota++;
		}
	}
}
