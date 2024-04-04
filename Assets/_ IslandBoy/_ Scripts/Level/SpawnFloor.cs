using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace IslandBoy
{
	public class SpawnFloor : SerializedMonoBehaviour
	{
		[SerializeField] private TilemapObject _spawnFloorTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private EntityRuntimeSet _monsterRTS;
		[SerializeField] private Entity _slimeEntity;
		[SerializeField] private int _maxMonsterAmount;
		[SerializeField] private List<RscSpawnSetting> _rscSpawnSettings;
		
		private List<Vector2> _spawnPositions = new();
		private Stack<string> _clearedResources = new();
		
		[Serializable]
		public class RscSpawnSetting
		{
			public Resource RscToSpawn;
			public int SpawnAmount;
			public float DistancePreference;
		}
		
		private void Awake()
		{
			GameSignals.DAY_START.AddListener(RefreshResources);
			GameSignals.RESOURCE_CLEARED.AddListener(RegisterClearedResource);
			
			// Get the bounds of the Tilemap
			BoundsInt bounds = _spawnFloorTm.Tilemap.cellBounds;

			// Iterate over all cells in the Tilemap
			foreach (var position in bounds.allPositionsWithin)
			{
				// Check if the cell has a tile placed on it
				if (_spawnFloorTm.Tilemap.HasTile(position))
				{
					// Add the position to the list
					_spawnPositions.Add(new Vector2(position.x, position.y));
				}
			}
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_START.RemoveListener(RefreshResources);
			GameSignals.RESOURCE_CLEARED.AddListener(RegisterClearedResource);
		}
		
		private void Start()
		{
			RefreshResources(null);
			
			StartCoroutine(ResourceSpawner());
			StartCoroutine(EntitySpawner());
		}
		
		private IEnumerator EntitySpawner()
		{
			yield return new WaitForSeconds(10f);
			
			
			if(_monsterRTS.ListSize < _maxMonsterAmount)
			{
				var randPos = GetRandomPosition();
				
				Instantiate(_slimeEntity, randPos, Quaternion.identity);
			}
			
			StartCoroutine(EntitySpawner());
		}

		private IEnumerator ResourceSpawner()
		{
			yield return new WaitForSeconds(5f);
			
			if(_clearedResources.Count > 0)
			{
				string rscToSpawnName = _clearedResources.Peek();
				
				var randPos = GetRandomPosition();
				Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
				
				if(IsValidSpawnPosition(pos))
				{
					foreach(RscSpawnSetting entry in _rscSpawnSettings)
					{
						if(rscToSpawnName == entry.RscToSpawn.RscName)
						{
							SpawnResource(entry.RscToSpawn, pos);
							_clearedResources.Pop();
							Debug.Log("Rsc Re-generated");
						}
					}
				}
			}
			
			StartCoroutine(ResourceSpawner());
		}
		
		public void RegisterClearedResource(ISignalParameters parameters)
		{
			Resource rsc = (Resource)parameters.GetParameter("Resource");
			_clearedResources.Push(rsc.RscName);
		}
		
		public void RefreshResources(ISignalParameters parameters)
		{
			// Destroy all resources
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			
			// spawn resources
			foreach(RscSpawnSetting entry in _rscSpawnSettings)
			{
				for (int i = 0; i < entry.SpawnAmount; i++)
				{
					var randPos = GetRandomPosition();
					Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
					
					if(IsNearbyResources(new Vector2(pos.x, pos.y), entry.DistancePreference)) 
						continue;
					
					if(IsValidSpawnPosition(pos))
					{
						SpawnResource(entry.RscToSpawn, pos);
					}
				}
			}
		}
		
		private Vector2 GetRandomPosition()
		{
			int randomIndex = UnityEngine.Random.Range(0, _spawnPositions.Count);
			Vector2 randomPosition = _spawnPositions[randomIndex];
			
			return randomPosition;
		}
		
		private void SpawnResource(Resource rsc, Vector3Int spawnPos)
		{
			var r = Instantiate(rsc, spawnPos, Quaternion.identity);
			r.transform.SetParent(transform);
		}
		
		private bool IsValidSpawnPosition(Vector3Int pos)
		{
			if(_floorTm.Tilemap.HasTile(pos) || _wallTm.Tilemap.HasTile(pos) || !IsClear(pos))
			{
				return false;
			}
			
			return true;
		}
		
		private bool IsNearbyResources(Vector2 pos, float radiusToCheck)
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(pos + new Vector2(0.5f, 0.5f), radiusToCheck);
			
			foreach(Collider2D col in colliders)
			{
				if(col.gameObject.layer == 3 || col.CompareTag("RSC") || col.TryGetComponent(out FeetTag ft)) 
				{
					return true;
				}
			}
			
			return false;
		}
		
		public bool IsClear(Vector3Int pos)
		{
			var colliders = Physics2D.OverlapBoxAll(new Vector2(pos.x, pos.y) + new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 0);

			foreach(Collider2D col in colliders)
			{
				if(col.gameObject.layer == 3 || col.CompareTag("RSC") || col.TryGetComponent(out FeetTag ft)) 
				{
					return false;
				}
			}

			return true;
		}
	}
}
