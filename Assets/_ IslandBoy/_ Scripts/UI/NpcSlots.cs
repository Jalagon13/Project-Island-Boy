using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class NpcSlots : MonoBehaviour
	{
		[SerializeField] private GameObject _blacksmithSlot;
		[SerializeField] private GameObject _wizardSlot;
		[SerializeField] private GameObject _knightSlot;
		
		private static NpcSlots _instance;
		//private static int _settlerCount;
		private static List<string> _settlers = new List<string>();
		
		public static NpcSlots Instance => _instance;
		public static int SettlerCount => _settlers.Count;
		
		private void Awake() 
		{
			_instance = this;
		}
		
		public void UpdateBlacksmithSlot()
		{
			_blacksmithSlot.transform.GetChild(0).gameObject.SetActive(true);
			_blacksmithSlot.transform.GetChild(1).gameObject.SetActive(false);

			if (!_settlers.Contains("Blacksmith"))
				_settlers.Add("Blacksmith");
		}
		
		public void UpdateWizardSlot()
		{
			_wizardSlot.transform.GetChild(0).gameObject.SetActive(true);
			_wizardSlot.transform.GetChild(1).gameObject.SetActive(false);
			if (!_settlers.Contains("Wizard"))
				_settlers.Add("Wizard");
		}
		
		public void UpdateKnightSlot()
		{
			_knightSlot.transform.GetChild(0).gameObject.SetActive(true);
			_knightSlot.transform.GetChild(1).gameObject.SetActive(false);
			if (!_settlers.Contains("Knight"))
				_settlers.Add("Knight");
		}

		public void RemoveSlot(string npc)
        {
			if (_settlers.Contains(npc))
				_settlers.Remove(npc);

			switch (npc)
			{
				case "Blacksmith":
					_blacksmithSlot.transform.GetChild(0).gameObject.SetActive(false);
					_blacksmithSlot.transform.GetChild(1).gameObject.SetActive(true);
					break;
				case "Knight":
					_knightSlot.transform.GetChild(0).gameObject.SetActive(false);
					_knightSlot.transform.GetChild(1).gameObject.SetActive(true);
					break;
				case "Wizard":
					_wizardSlot.transform.GetChild(0).gameObject.SetActive(false);
					_wizardSlot.transform.GetChild(1).gameObject.SetActive(true);
					break;
			}
		}
	}
}
