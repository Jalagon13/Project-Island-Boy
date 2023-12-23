using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace IslandBoy
{
	public class MonsterSpawner : MonoBehaviour
	{
		[SerializeField] private EntityRuntimeSet _entityRTS;
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private MobSpawnSettingObject _forestSpawnSetting;

		private void Awake()
		{
			GameSignals.DAY_START.AddListener(UnPauseMonsterSpawning);
			GameSignals.DAY_END.AddListener(PauseMonsterSpawning);
			GameSignals.CHANGE_SCENE.AddListener(UpdateEntitySet);
		}

		private void OnDestroy()
		{
			GameSignals.DAY_START.RemoveListener(UnPauseMonsterSpawning);
			GameSignals.DAY_END.RemoveListener(PauseMonsterSpawning);
			GameSignals.CHANGE_SCENE.RemoveListener(UpdateEntitySet);
		}

		private void Start()
		{
			StartCoroutine(SpawnMonsterTimer());
		}

		public void MonsterSpawnDebugButton()
		{
			SpawnMonster();
		}

		private void UpdateEntitySet(ISignalParameters parameters)
		{
			_entityRTS.Initialize();
			
			// Update S
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
			yield return new WaitForSeconds(1);
			if (Random.Range(0,100f) < _forestSpawnSetting.SpawnPercentPerSec && _entityRTS.ListSize < _forestSpawnSetting.MaxMonsterCount)
			{
				SpawnMonster();
			}

			StartCoroutine(SpawnMonsterTimer());
		}

		private void SpawnMonster()
		{
			var spawnPos = CalcSpawnPos();

			if (_wallTm.Tilemap.HasTile(Vector3Int.FloorToInt(spawnPos)) || _floorTm.Tilemap.HasTile(Vector3Int.FloorToInt(spawnPos)))
			{
				return;
			}

			if (Vector3.Distance(spawnPos, transform.position) < 8)
			{
				return;
			}

			Spawn(MonsterToSpawn(), spawnPos);
		}

		private Entity MonsterToSpawn()
		{
			return _forestSpawnSetting.GetRandomEntity;
		}

		private void Spawn(Entity monster, Vector2 pos)
		{
			Instantiate(monster, pos, Quaternion.identity);
		}

		private Vector2 CalcSpawnPos()
		{
			GraphNode startNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;

			List<GraphNode> nodes = PathUtilities.BFS(startNode, 35);
			Vector3 singleRandomPoint = PathUtilities.GetPointsOnNodes(nodes, 1)[0];

			return singleRandomPoint;
		}
	}
}
