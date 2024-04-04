using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IslandBoy
{
	public class SpawnFloor : SerializedMonoBehaviour
	{
		[SerializeField] private TilemapObject _spawnFloorTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private Dictionary<Resource, int> _spawnDict;
		
		private List<Vector2> _spawnPositions = new();
		
		private void Awake()
		{
			GameSignals.DAY_START.AddListener(RefreshResources);
			
			// Get the bounds of the Tilemap
			BoundsInt bounds = _spawnFloorTm.Tilemap.cellBounds;

			// Iterate over all cells in the Tilemap
			foreach (var position in bounds.allPositionsWithin)
			{
				// Get the position in world coordinates
				Vector3Int cellPosition = new Vector3Int(position.x, position.y, position.z);
				Vector3 worldPosition = _spawnFloorTm.Tilemap.CellToWorld(cellPosition);

				// Add the world position to the list
				_spawnPositions.Add(worldPosition);
			}
		}
		
		private void Start()
		{
			StartCoroutine(ResourceSpawner());
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_START.RemoveListener(RefreshResources);
		}
		
		private IEnumerator ResourceSpawner()
		{
			yield return new WaitForSeconds(2f);
			
			Debug.Log("Trying to Spawn Resource TEXT");
			
			StartCoroutine(ResourceSpawner());
		}
		
		public void RefreshResources(ISignalParameters parameters)
		{
			// Destroy all resources
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			
			// spawn resources
			foreach(KeyValuePair<Resource, int> entry in _spawnDict)
			{
				for (int i = 0; i < entry.Value; i++)
				{
					var spawnPosition = CalcSpawnPosition();
				}
			}
		}
		
		private Vector2 CalcSpawnPosition()
		{
			
		}
		
		private void SpawnResource(Resource rsc)
		{
			
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
				if(col.gameObject.layer == 3 || col.CompareTag("RSC") || col.TryGetComponent(out FeetTag ft)) 
				{
					return false;
				}
			}

			return true;
		}
	}
}
