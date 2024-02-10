using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class LevelEntranceLower : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private InitialSpawnPosition _isp;
		
		public void GoDownStairs()
		{
			_isp = transform.parent.GetChild(0).GetComponent<InitialSpawnPosition>();
			_isp.SetSpawnPos(_po.Position);
			
			CaveManager.Instance.DescendFloor();
		}
	}
}
