using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class LevelEntranceUpper : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private InitialSpawnPosition _isp;
		
		public void GoUpStairs()
		{
			_isp = transform.parent.GetChild(0).GetComponent<InitialSpawnPosition>();
			_isp.SetSpawnPos(_po.Position);
			
			CaveManager.Instance.AscendFloor();
		}
	}
}
