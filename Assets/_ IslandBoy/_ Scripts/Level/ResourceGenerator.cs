using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
	public class ResourceGenerator : MonoBehaviour
	{
		[SerializeField] private TilemapObject _grassTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private Entity _surfaceSlimeEntity;
		[SerializeField] private List<RscSpawnSetting> _rscSpawnSettings;
		
		private List<Vector2> _spawnPositions = new();
		private int _maxSurfaceSlimes = 8;
		private int _currentSlimes;
		
		[Serializable]
		public class RscSpawnSetting
		{
			public Clickable ClickableToSpawn;
			public int SpawnAmount;
		}
		
		private void Awake()
		{
			GameSignals.DAY_START.AddListener(RefreshResources);
			GameSignals.DAY_END.AddListener(StopMonsterSpawn);
		}
		
		private void OnEnable()
		{
			//StartCoroutine(Refresh());
			_currentSlimes = 0;
			StartCoroutine(SpawnSlimes());
		}
		
		private void OnDisable()
		{
			StopAllCoroutines();
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_START.RemoveListener(RefreshResources);
            GameSignals.DAY_END.RemoveListener(StopMonsterSpawn);
        }
		
		private void Start()
		{
			// Gather all spawn tile positions
			BoundsInt bounds = _grassTm.Tilemap.cellBounds;
			
			foreach (var position in bounds.allPositionsWithin)
			{
				if(_grassTm.Tilemap.HasTile(position))
				{
					_spawnPositions.Add(new Vector2(position.x, position.y));
				}
			}
			//if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Surface"))
                Refresh();
        }
		
		private IEnumerator SpawnSlimes()
		{
			yield return new WaitForSeconds(8);
			
			var randPos = GetRandomPosition();
			Vector3Int pos = new((int)randPos.x, (int)randPos.y, 0);
			
			if(IsValidSpawnPosition(pos) && _currentSlimes < _maxSurfaceSlimes)
			{
				SpawnClickable(_surfaceSlimeEntity, pos, appendToLevel:false);
				_currentSlimes++;
			}
			
			StartCoroutine(SpawnSlimes());
		}
		
		private void RefreshResources(ISignalParameters parameters)
		{
			Refresh();
		}

		private void StopMonsterSpawn(ISignalParameters parameters)
		{
			StopAllCoroutines();
		}
		
		private void Refresh()
		{
			// Destroy all resources
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			
			// spawn resources
			foreach(RscSpawnSetting entry in _rscSpawnSettings)
			{
				// choose a number between half and x1.5 the spawn amount
				int spawnAmount = UnityEngine.Random.Range(entry.SpawnAmount, entry.SpawnAmount * 3) / 2;

                for (int i = 0; i < spawnAmount; i++)
				{
					tryAgain:
					var randPos = GetRandomPosition();
					Vector3Int pos = new Vector3Int((int)randPos.x, (int)randPos.y, 0);
					
					if(IsValidSpawnPosition(pos))
					{
						SpawnClickable(entry.ClickableToSpawn, pos);
					}
					else
					{
						goto tryAgain;
					}
				}
			}
		}
		
		private Clickable SpawnClickable(Clickable clickable, Vector3Int spawnPos, bool appendToLevel = true)
		{
			var r = Instantiate(clickable, spawnPos, Quaternion.identity);
			
			if(appendToLevel)
				r.transform.SetParent(transform);
				
			return r;
		}
		
		private Vector2 GetRandomPosition()
		{
			int randomIndex = UnityEngine.Random.Range(0, _spawnPositions.Count);
			Vector2 randomPosition = _spawnPositions[randomIndex];
			
			return randomPosition;
		}
		
		private bool IsValidSpawnPosition(Vector3Int pos)
		{
			if(_floorTm.Tilemap.HasTile(pos) || _wallTm.Tilemap.HasTile(pos) || !IsClear(pos))
			{
				return false;
			}
			
			return true;
		}
		
		public bool IsClear(Vector3Int pos)
		{
			var colliders = Physics2D.OverlapBoxAll(new Vector2(pos.x, pos.y) + new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 0);

			foreach(Collider2D col in colliders)
			{
				if(col.gameObject.layer == 3 || col.CompareTag("RSC") || col.CompareTag("NPC") || col.TryGetComponent(out FeetTag ft))
				{
					return false;
				}
			}

			return true;
		}
	}
}
