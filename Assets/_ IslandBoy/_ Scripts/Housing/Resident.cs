using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	[Serializable]
	public class Resident
	{
		[Required]
		public NPC NPC;
		public string Name;
		[SerializeField] 
		private bool _unlocked; // NPCs need to be unlocked to move in to your town.
		private Bed _bed = null;

		public bool Unlocked => _unlocked;
		public Bed Bed { get { return _bed; } }

		public void Unlock()
		{
			_unlocked = true;
		}
		
		public void SetPosition(Vector2 pos)
		{
			NPC.gameObject.SetActive(false);
			NPC.gameObject.transform.position = pos;
			NPC.GetComponent<NPCStateManager>().SetHomePoint(pos);
			NPC.gameObject.SetActive(true);
		}

		public void SetBed(Bed bed)
        {
			_bed = bed;
			NPC.UnlockShop();
        }

		public void RemoveBed()
		{
			_bed = null;
			if (NPC != null)
				NPC.CloseShop();
		}
	}
}
