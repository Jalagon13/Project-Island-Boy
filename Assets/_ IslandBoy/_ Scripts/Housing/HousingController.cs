using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class HousingController : SerializedMonoBehaviour
	{
		[InfoBox("Unlocked means NPC is capable of moving in and out town")]
		[SerializeField] private List<Resident> _residents = new();
		
		private static HousingController _i;
		
		public static HousingController Instance { get { return _i; }} // used to access unlock method anywhere
		
		private void Awake() 
		{
			if(_i == null)
				_i = this;
				
			GameSignals.DAY_END.AddListener(UpdateHousing);
		}

		private void OnDestroy() 
		{
			GameSignals.DAY_END.RemoveListener(UpdateHousing);
		}
		
		public void UnlockNpc(string npcName)
		{
			foreach (Resident resident in _residents)
			{
				if(npcName == resident.Name)
				{
					resident.Unlock();
					return;
				}
			}
			
			Debug.LogError($"You are trying to unlock an NPC '{npcName}' that is already unlocked or doesn't exist");
		}

		private void UpdateHousing(ISignalParameters parameters)
		{
			Bed[] allBeds = FindObjectsOfType<Bed>();
			List<Bed> validBeds = new(); // beds found in valid house
			List<Bed> nonValidBeds = new(); // beds found in non-valid setting
			
			for(int i = 0; i < allBeds.Length; i++) //loop through all beds and find the ones that have a in a valid house.
			{
				if (allBeds[i].InValidSpace())
					validBeds.Add(allBeds[i]);
				else
					nonValidBeds.Add(allBeds[i]);
			}
			
			// List<Resident> movedOutResidents = new(); // store list of NPCs moved out this update to prevent them from spawning again on the same update
			// foreach (Bed bed in nonValidBeds) // loop through all non valid beds and move out NPCs
			// {
			// 	if(bed.Occupied)
			// 	{
			// 		movedOutResidents.Add(bed.Resident);
			// 		MoveOutNpc(bed);
			// 	}
			// }
			
			foreach (Bed noValBed in nonValidBeds)
			{
				if(noValBed.Occupied)
				{
					noValBed.MoveOutNPC();
				}
			}
			
			foreach (Bed bed in validBeds) // loop through all valid beds and move in npcs
			{
				foreach (Resident resident in _residents)
				{
					// if(movedOutResidents.Contains(resident))
					// 	continue;
					
					if(NpcAvailableToMoveIn(bed, resident))
					{
						MoveInNpc(bed, resident);
					}
				}
			}
			
			// loop through all valid beds and put NPCs to their starting spots
			foreach (Bed bed1 in validBeds)
			{
				if(bed1.Occupied)
				{
					bed1.ResetNpc();
				}
			}
		}
		
		// private void MoveOutNpc(Bed bed)
		// {
		// 	DispatchResidentSignal($"{bed.Resident.Name} has moved out of your town!");
		// 	EnableNpc(bed.Resident, false);
		// 	bed.MoveOutNPC();
		// }
		
		private void MoveInNpc(Bed bed, Resident resident)
		{
			DispatchResidentSignal($"{resident.Name} has moved into your town!");
			// EnableNpc(resident, true);
			bed.MoveInNPC(resident);
		}
		
		private void DispatchResidentSignal(string message)
		{
			Signal signal = GameSignals.RESIDENT_UPDATE;
			signal.ClearParameters();
			signal.AddParameter("Message", message);
			signal.Dispatch();
		}
		
		private bool NpcAvailableToMoveIn(Bed bed, Resident resident)
		{
			return resident.NPC.IsFree /* && resident.Unlocked */ && !bed.Occupied && resident.Bed == null;
		}
		
		// private void EnableNpc(Resident resident, bool _)
		// {
		// 	resident.NPC.gameObject.SetActive(_);
		// }
	}
}
