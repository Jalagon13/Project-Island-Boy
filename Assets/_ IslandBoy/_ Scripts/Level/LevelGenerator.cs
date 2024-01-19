using System;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace IslandBoy
{
	public class LevelGenerator : MonoBehaviour
	{
		[Header("Level Generation")]
		[SerializeField] private Tilemap _wallTilemap;
		[SerializeField] private Tilemap _floorTilemap;
		[SerializeField] private TileBase _floorTile;
		[SerializeField] private TileBase _wallTile;
		[SerializeField] private InitialSpawnPosition _isp;
		[SerializeField] private NavMeshSurface _navMesh;
		[SerializeField] private int _borderLength;
		[SerializeField] private int _centerBlobRadius;
		[SerializeField] private int _iterations;
		[Range(0, 1)]
		[SerializeField] private float _noiseDensity;
		[SerializeField] private int _width = 100;
		[SerializeField] private int _height = 100;
		[Header("Resource Generation")]
		
		[SerializeField] private bool _generateResources = true;
		[SerializeField] private Clickable _pot;
		[SerializeField] private Clickable _stone;
		[SerializeField] private Clickable _staircase; 
		[SerializeField] private TileBase _rscTile;
		
		private int[,] _map;
		
		private GameObject _resourceHolder;
		private List<Vector2Int> _wallSpots;
		private List<Vector2Int> _spaceSpots;
		
		private Vector3Int[] _directions = {
			new(0, 1, 0),  // Up
			new(0, -1, 0), // Down
			new(-1, 0, 0), // Left
			new(1, 0, 0),  // Right
			new(-1, 1, 0),  // Top Left
			new(1, 1, 0),   // Top Right
			new(-1, -1, 0), // Bottom Left
			new(1, -1, 0)   // Bottom Right
		};
		
		public InitialSpawnPosition SpawnPosition => _isp;
		
		private void Awake()
		{
			Generate();
		}
		
		private void OnEnable()
		{
			LevelControl.DeployParent = _resourceHolder;
		}
		
		private void OnDisable()
		{
			LevelControl.DeployParent = null;
		}
		
		#region Map Generation
		
		[Button("Generate")]
		private void Generate()
		{
			Reset();
			GenerateBlob(new Vector3Int(_width / 2, _height / 2), _centerBlobRadius, _floorTilemap, _floorTile);
			GenerateMap();
			SmoothMap();	
			GenerateWallResources();
			CreateTiles();
			GenerateResources();
			UpdateNavMesh();
		}
		
		public void UpdateNavMesh() => StartCoroutine(UpdateNM());
		
		private IEnumerator UpdateNM()
		{
			yield return new WaitForEndOfFrame();
			
			if(_navMesh != null)
			{
				_navMesh.hideEditorLogs = true;
				_navMesh.UpdateNavMesh(_navMesh.navMeshData);
			}
		}
		
		private void GenerateResources()
		{
			if(_generateResources)
			{
				GenerateStaircases();
				GenerateNodeResources(_stone, 35);
				GenerateNodeResources(_pot, 50);
			}
		}

		[Button("ResetTiles")]
		private void Reset()
		{
			_resourceHolder = transform.GetChild(2).gameObject;
			_floorTilemap.ClearAllTiles();
			_wallTilemap.ClearAllTiles();
			_wallSpots = new();
			_wallSpots.Clear();
			_spaceSpots = new();
			_spaceSpots.Clear();
			
			if(_resourceHolder.transform.childCount > 0)
			{
				foreach (Transform transform in _resourceHolder.transform)
				{
					Destroy(transform.gameObject);
				}
			}
		}
		
		private void GenerateBlob(Vector3Int pos, int radius, Tilemap tilemap, TileBase tilebase)
		{
			for (int x = -radius; x <= radius; x++)
			{
				for (int y = -radius; y <= radius; y++)
				{
					Vector3Int tilePosition = new Vector3Int(pos.x + x, pos.y + y, pos.z);

					if (IsInsideBlob(tilePosition, pos))
					{
						tilemap.SetTile(tilePosition, tilebase);
					}
				}
			}
		}
		
		bool IsInsideBlob(Vector3Int tilePosition, Vector3Int centerPosition)
		{
			return Vector3Int.Distance(tilePosition, centerPosition) <= _centerBlobRadius;
		}
		
		private void GenerateMap()
		{
			_map = new int[_width, _height];

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					var pos = new Vector3Int(x, y);
					
					if(_floorTilemap.HasTile(pos))
					{
						_map[x, y] = 0;
					}
					else
					{
						if(x < _borderLength || x > (_width - _borderLength) || y < _borderLength || y > (_height - _borderLength))
						{
							_map[x, y] = 1;
						}
						else
						{
							float rand = Random.value;
							_map[x, y] = (rand < _noiseDensity) ? 1 : 0;
							
							if(_map[x, y] == 1)
							{
								_wallSpots.Add(new(x, y));
							}
						}
					}
				}
			}
		}

		private void SmoothMap()
		{
			for (int i = 0; i < _iterations; i++)
			{
				for (int x = 0; x < _width; x++)
				{
					for (int y = 0; y < _height; y++)
					{
						int surroundingWalls = GetSurroundingWallCount(x, y);
						if (surroundingWalls > 4)
							_map[x, y] = 1;
						else if (surroundingWalls < 4)
							_map[x, y] = 0;
					}
				}
			}
		}
		
		private int GetSurroundingWallCount(int gridX, int gridY)
		{
			int wallCount = 0;
			for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
			{
				for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
				{
					if (IsInsideMap(neighborX, neighborY))
					{
						if (neighborX != gridX || neighborY != gridY)
						{
							wallCount += _map[neighborX, neighborY];
						}
					}
					else
					{
						// Consider out-of-bounds positions as walls
						wallCount++;
					}
				}
			}
			return wallCount;
		}
		
		private bool IsInsideMap(int x, int y)
		{
			return x >= 0 && x < _width && y >= 0 && y < _height;
		}
		
		private void CreateTiles()
		{
			// Assuming you have a prefab for a wall and a floor tile
			// Make sure to set up your Tilemap or GameObject accordingly

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					if (_map[x, y] == 1)
					{
						// Instantiate and position a wall tile
						if(!_wallTilemap.HasTile(new(x,y)))
						{
							PlaceTile(_wallTilemap, _wallTile, x, y);
						}
					}
					else
					{
						// Instantiate and position an air tile
						PlaceTile(_wallTilemap, null, x, y);
						_spaceSpots.Add(new(x, y));
					}
					
					PlaceFloorTile(x, y);
				}
			}
		}
		
		private void PlaceTile(Tilemap tilemap, TileBase tile, int x, int y)
		{
			Vector3Int pos = new(x, y);
			tilemap.SetTile(pos, tile);
		}
		
		private void PlaceFloorTile(int x , int y)
		{
			Vector3Int pos = new Vector3Int(x, y);
			if(!_floorTilemap.HasTile(pos))
			{
				PlaceTile(_floorTilemap, _floorTile, x, y);
			}
		}
		
		#endregion
		
		private void GenerateWallResources()
		{
			int clumpsPerQuadrant = 5;
			int iterations = 3;
			
			for (int i = 0; i < clumpsPerQuadrant; i++)
			{
				GenerateClump(iterations, GetRandomPositionBR());
				GenerateClump(iterations, GetRandomPositionBL());
				GenerateClump(iterations, GetRandomPositionTR());
				GenerateClump(iterations, GetRandomPositionTL());
			}
		}
		
		private void GenerateClump(int iterations, Vector3Int pos)
		{
			for (int clump = 0; clump < iterations; clump++)
			{
				Vector3Int offset = new Vector3Int(Random.Range(-3, 3), Random.Range(-3, 3));
				GenerateBlob(pos + offset, Random.Range(1, 2), _wallTilemap, _rscTile);
			}
		}
		
		private Vector3Int GetRandomPositionBR()
		{
			return new(Random.Range(_borderLength, _width / 2), Random.Range(_borderLength, _height / 2));
		}
		
		private Vector3Int GetRandomPositionBL()
		{
			return new(Random.Range(_width / 2, _width - _borderLength), Random.Range(_borderLength, _height / 2));
		}
		
		private Vector3Int GetRandomPositionTR()
		{
			return new(Random.Range(_width / 2, _width - _borderLength), Random.Range(_height / 2, _height - _borderLength));
		}
		
		private Vector3Int GetRandomPositionTL()
		{
			return new(Random.Range(_borderLength, _width / 2), Random.Range(_height / 2, _height - _borderLength));
		}
		
		private void GenerateNodeResources(Clickable rsc, int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				var pos = _spaceSpots[Random.Range(0, _spaceSpots.Count)];
				if(!IsClear(new(pos.x, pos.y)))
					continue;
				
				var pot = Instantiate(rsc, new(pos.x, pos.y), quaternion.identity);
				pot.transform.SetParent(_resourceHolder.transform);
				_spaceSpots.Remove(pos);
			}
		}
		
		private void GenerateStaircases()
		{
			GenerateStairs(GetRandomPositionBR());
			GenerateStairs(GetRandomPositionBL());
			GenerateStairs(GetRandomPositionTR());
			GenerateStairs(GetRandomPositionTL());
		}
		
		private void GenerateStairs(Vector3Int pos)
		{
			if(_wallTilemap.HasTile(pos))
			{
				_wallTilemap.SetTile(pos, null);
			}
			
			foreach (var direction in _directions)
			{
				var neighborPosition = pos + direction;
				
				if(_wallTilemap.HasTile(neighborPosition))
				{
					_wallTilemap.SetTile(neighborPosition, null);
				}
			}
			
			var stairs = Instantiate(_staircase, pos, Quaternion.identity);
			stairs.transform.SetParent(_resourceHolder.transform);
			_spaceSpots.Remove((Vector2Int)pos);
		}
		
		private bool IsClear(Vector3Int position)
		{
			var colliders = Physics2D.OverlapBoxAll(new(position.x + 0.5f, position.y + 0.5f), new Vector2(0.5f, 0.5f), 0);

			foreach(Collider2D col in colliders)
			{
				if(col.gameObject.layer == 3) 
					return false;
			}

			return true;
		}
		
	}
}
