using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
	public class Surface : MonoBehaviour
	{
		[SerializeField] private PlayerObject _po;
		[SerializeField] private EntityRuntimeSet _entityRTS;
		[SerializeField] private TilemapObject _floorTilemapObject;
		[SerializeField] private TilemapObject _wallTilemapObject;
		[Header("Crab Spawn")]
		[SerializeField] private Entity _crabEntity;
		[SerializeField] private List<Transform> _crabSpawnPositions;
		[Header("Ghost Spawn")]
		[SerializeField] private Entity _ghostEntity;
		[SerializeField] private List<Transform> _ghostSpawnPositions;
		
		private bool _enabledStartingUI;
		private bool _isNight;
		private bool _hasSpawnedNightMonsters;
		
		private void Awake() 
		{
			// GameSignals.DAY_START.AddListener(DayMonsterHandle);
			// GameSignals.NIGHT_START.AddListener(NightMonsterHandle);
			// GameSignals.ENABLE_STARTING_MECHANICS.AddListener(DayMonsterHandle);
		}
		
		private void OnDestroy() 
		{
			// GameSignals.DAY_START.RemoveListener(DayMonsterHandle);
			// GameSignals.NIGHT_START.RemoveListener(NightMonsterHandle);
			// GameSignals.ENABLE_STARTING_MECHANICS.RemoveListener(DayMonsterHandle);
		}
		
		private void OnEnable()
		{
			if(!_enabledStartingUI)
			{
				StartCoroutine(Delay());
			}
			
			if(_isNight && !_hasSpawnedNightMonsters)
			{
				// SpawnNightMonsters();
			}
		}
		
		private IEnumerator Delay()
		{
			yield return new WaitForSeconds(3f);
			_enabledStartingUI = true;
			GameSignals.ENABLE_STARTING_MECHANICS.Dispatch();
		}
		
		private IEnumerator Start() 
		{
			yield return new WaitForSeconds(.5f);
			// DayMonsterHandle(null);
		}
		
		private void DayMonsterHandle(ISignalParameters parameters)
		{
			_isNight = false;
			_hasSpawnedNightMonsters = false;
			ClearMonsters();
			SpawnDayMonsters();
		}
		
		private void NightMonsterHandle(ISignalParameters parameters)
		{
			_isNight = true;
			if(!gameObject.activeInHierarchy) return;
			
			SpawnNightMonsters();
		}
		
		private void SpawnNightMonsters()
		{
			_hasSpawnedNightMonsters = true;
			
			foreach (Transform tf in _ghostSpawnPositions)
			{
				Vector3 randomSpawnPoint = tf.position + (Vector3)Random.insideUnitCircle * 5;
				
				if(Vector2.Distance(randomSpawnPoint, _po.Position) < 10)
					continue;
				
				Instantiate(_ghostEntity, randomSpawnPoint, Quaternion.identity);
			}
		}
		
		private void SpawnDayMonsters()
		{
			foreach (Transform tf in _crabSpawnPositions)
			{
				Vector3 randomSpawnPoint = tf.position + (Vector3)Random.insideUnitCircle * 3;
				
				if(!_floorTilemapObject.Tilemap.HasTile(Vector3Int.FloorToInt(randomSpawnPoint)) && !_wallTilemapObject.Tilemap.HasTile(Vector3Int.FloorToInt(randomSpawnPoint)))
				{
					Instantiate(_crabEntity, randomSpawnPoint, Quaternion.identity);
				}
			}
		}
		
		private void ClearMonsters()
		{
			if(_entityRTS.ListSize > 0)
			{
				foreach (Entity entity in _entityRTS.Items)
				{
					Destroy(entity.gameObject);
				}
				
				_entityRTS.Initialize();
			}
		}
	}
}
