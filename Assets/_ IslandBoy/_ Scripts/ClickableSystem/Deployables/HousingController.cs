using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class HousingController : SerializedMonoBehaviour
	{
		[SerializeField] private Dictionary<NPC, bool> _npcList = new(); // bool represents if NPC can move into town.
		
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
			
			List<NPC> movedOutNPCs = new(); // store list of NPCs moved out this update to prevent them from spawning again on the same update
			foreach (Bed bed in nonValidBeds) // loop through all non valid beds and move out NPCs
			{
				if(bed.Occupied)
				{
					movedOutNPCs.Add(bed.NPC);
					MoveOutNpc(bed);
				}
			}
			
			foreach (Bed bed in validBeds) // loop through all valid beds and move in npcs
			{
				foreach (KeyValuePair<NPC, bool> entry in _npcList)
				{
					if(NpcAvailableToMoveIn(bed, entry))
					{
						if(movedOutNPCs.Contains(entry.Key))
							continue;
							
						MoveInNpc(bed, entry.Key);
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
		
		private void MoveOutNpc(Bed bed)
		{
			EnableNpc(bed.NPC, false);
			bed.MoveOutNPC();
		}
		
		private void MoveInNpc(Bed bed, NPC npc)
		{
			EnableNpc(npc, true);
			bed.MoveInNPC(npc);
		}
		
		private bool NpcAvailableToMoveIn(Bed bed, KeyValuePair<NPC, bool> entry)
		{
			return !entry.Key.gameObject.activeInHierarchy && entry.Value && !bed.Occupied;
		}
		
		private void EnableNpc(NPC npc, bool _)
		{
			npc.gameObject.SetActive(_);
		}
	}
}
