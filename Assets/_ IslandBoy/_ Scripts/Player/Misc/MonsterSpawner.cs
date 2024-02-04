using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace IslandBoy
{
	public class MonsterSpawner : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private EntityRuntimeSet _entityRTS;
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private MobSpawnSettingObject _spawnSettings;
		[SerializeField] private int _maxMonsterSpawnRange;

		private void Awake()
		{
			GameSignals.DAY_START.AddListener(UnPauseMonsterSpawning);
			GameSignals.DAY_END.AddListener(PauseMonsterSpawning);
		}

		private void OnDestroy()
		{
			GameSignals.DAY_START.RemoveListener(UnPauseMonsterSpawning);
			GameSignals.DAY_END.RemoveListener(PauseMonsterSpawning);
		}
		
		private void OnEnable()
		{
			if(_spawnSettings.EntityListLength <= 0)
				return;
			
			// StartCoroutine(SpawnMonsterTimer());
		}
		
		private void OnDisable()
		{
			StopAllCoroutines();
			_entityRTS.Initialize();
		}
		
		private void PauseMonsterSpawning(ISignalParameters parameters)
		{
			StopAllCoroutines();
		}

		private void UnPauseMonsterSpawning(ISignalParameters parameters)
		{
			StartCoroutine(SpawnMonsterTimer());
		}

		private IEnumerator SpawnMonsterTimer()
		{
			yield return new WaitForSeconds(10);
			
			float chance = Random.Range(0,100f);
			
			if (chance < _spawnSettings.SpawnPercentPerSec && _entityRTS.ListSize < _spawnSettings.MaxMonsterCount)
			{
				// SpawnMonster(MonsterToSpawn());
			}

			StartCoroutine(SpawnMonsterTimer());
		}

		public void SpawnMonster(Entity monster, int maxSpawnRange)
		{
			var spawnPos = CalcSpawnPos(maxSpawnRange);

			// if (_wallTm.Tilemap.HasTile(Vector3Int.FloorToInt(spawnPos)) || _floorTm.Tilemap.HasTile(Vector3Int.FloorToInt(spawnPos)))
			// {
			// 	return;
			// }

			// if (Vector3.Distance(spawnPos, transform.position) < 8)
			// {
			// 	return;
			// }

			Spawn(monster, spawnPos);
		}

		private Entity MonsterToSpawn()
		{
			return _spawnSettings.GetRandomEntity;
		}

		private void Spawn(Entity monster, Vector2 pos)
		{
			Instantiate(monster, pos, Quaternion.identity);
		}

		private Vector2 CalcSpawnPos(int maxSpawnRange)
		{
			GraphNode startNode = AstarPath.active.GetNearest(_po.Position, NNConstraint.Default).node; 
 
			List<GraphNode> nodes = PathUtilities.BFS(startNode, maxSpawnRange); 
			Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0]; 
 
			return singleRandomPoint; 
		}
	}
}
