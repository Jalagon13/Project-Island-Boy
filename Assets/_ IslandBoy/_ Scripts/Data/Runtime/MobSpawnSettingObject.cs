using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	[CreateAssetMenu(fileName = "MobSpawnSetting")]
	public class MobSpawnSettingObject : ScriptableObject
	{
		[Range(0,100)]
		[SerializeField] private float _spawnPercentPerSec;
		[SerializeField] private int _maxMonsterCount;
		[SerializeField] private List<Entity> _entityList;
		
		public int MaxMonsterCount { get {return _maxMonsterCount; } }
		public float SpawnPercentPerSec { get {return _spawnPercentPerSec; } }
		public Entity GetRandomEntity { get{ return _entityList[Random.Range(0, _entityList.Count)]; } }
	}
}
