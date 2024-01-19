using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class LevelEntranceUpper : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		
		private LevelGenerator _lg;
		
		public void GoUpStairs()
		{
			_lg = transform.parent.parent.GetComponent<LevelGenerator>();
			_lg.SpawnPosition.SetSpawnPos(_po.Position);
			
			CaveManager.Instance.AscendFloor();
		}
	}
}
