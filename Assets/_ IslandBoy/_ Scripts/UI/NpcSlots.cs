using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class NpcSlots : MonoBehaviour
	{
		[SerializeField] private GameObject _blacksmithSlot;
		[SerializeField] private GameObject _wizardSlot;
		
		private static NpcSlots _instance;
		private static int _settlerCount;
		
		public static NpcSlots Instance => _instance;
		public static int SettlerCount => _settlerCount;
		
		private void Awake() 
		{
			_settlerCount = 0;
			_instance = this;
		}
		
		public void UpdateBlacksmithSlot()
		{
			_blacksmithSlot.transform.GetChild(0).gameObject.SetActive(true);
			_blacksmithSlot.transform.GetChild(1).gameObject.SetActive(false);
			_settlerCount++;
		}
		
		public void UpdateWizardSlot()
		{
			_wizardSlot.transform.GetChild(0).gameObject.SetActive(true);
			_wizardSlot.transform.GetChild(1).gameObject.SetActive(false);
			_settlerCount++;
		}
	}
}
