using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class HousingControl : MonoBehaviour
	{
		[SerializeField] private NPCEntity _wizardNPC;
		
		private bool _wizardSpawned;
		
		private void Awake() 
		{
			GameSignals.DAY_END.AddListener(UpdateHousing);
		}

		private void OnDestroy() 
		{
			GameSignals.DAY_END.RemoveListener(UpdateHousing);
		}

		private void UpdateHousing(ISignalParameters parameters)
		{
			Bed[] allBeds = FindObjectsOfType<Bed>();
			List<Bed> validBeds = new(); // beds found in valid house
			List<Bed> nonValidBeds = new(); // beds found in non-valid setting
			
			//loop through all beds and find the ones that have a in a valid house.
			for(int i = 0; i < allBeds.Length; i++)
			{
				if (allBeds[i].InValidSpace())
					validBeds.Add(allBeds[i]);
				else
					nonValidBeds.Add(allBeds[i]);
			}
			
			// loop through all non valid beds and evict NPCs
			foreach (Bed bed in nonValidBeds)
			{
				
			}
			
			// loop through all valid beds and SpawnValidNPCs
			foreach (Bed bed in validBeds)
			{
				if(_wizardSpawned) continue;
				
				
			}
		}
		
		private void SpawnWizard(Vector2 spawnPosition)
		{
			// Instantiate
		}
	}
}
