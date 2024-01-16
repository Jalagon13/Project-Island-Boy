using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
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
		[SerializeField] private int _borderLength;
		[SerializeField] private int _centerBlobRadius;
		[SerializeField] private int _iterations;
		[Range(0, 1)]
		[SerializeField] private float _noiseDensity;
		[SerializeField] private int _width = 100;
		[SerializeField] private int _height = 100;
		[Header("Resource Generation")]
		[SerializeField] private Clickable _pot;
		[SerializeField] private TileBase _rscTile;
		
		private int[,] _map;
		
		private Transform _resourceHolder;
		private List<Vector2Int> _wallSpots;
		private List<Vector2Int> _spaceSpots;
		
		private void Awake()
		{
			Reset();
		}
		
		private void Start()
		{
			Generate();
			GenerateNodeResources();
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
		}

		[Button("ResetTiles")]
		private void Reset()
		{
			_resourceHolder = transform.GetChild(2);
			_floorTilemap.ClearAllTiles();
			_wallTilemap.ClearAllTiles();
			_wallSpots = new();
			_wallSpots.Clear();
			_spaceSpots = new();
			_spaceSpots.Clear();
			
			if(_resourceHolder.childCount > 0)
			{
				foreach (Transform transform in _resourceHolder.transform)
				{
					if(Application.isEditor)
						DestroyImmediate(transform.gameObject);
					else
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
				GenerateClump(iterations, Random.Range(_borderLength, _width / 2), Random.Range(_borderLength, _height / 2));
				GenerateClump(iterations, Random.Range(_width / 2, _width - _borderLength), Random.Range(_borderLength, _height / 2));
				GenerateClump(iterations, Random.Range(_width / 2, _width - _borderLength), Random.Range(_height / 2, _height - _borderLength));
				GenerateClump(iterations, Random.Range(_borderLength, _width / 2), Random.Range(_height / 2, _height - _borderLength));
			}
		}
		
		private void GenerateClump(int iterations, int xPos, int yPos)
		{
			for (int clump = 0; clump < iterations; clump++)
			{
				Vector3Int offset = new Vector3Int(Random.Range(-3, 3), Random.Range(-3, 3));
				GenerateBlob(new Vector3Int(xPos, yPos) + offset, Random.Range(1, 2), _wallTilemap, _rscTile);
			}
		}
		
		private void GenerateNodeResources()
		{
			for (int i = 0; i < 20; i++)
			{
				var pos = _spaceSpots[Random.Range(0, _spaceSpots.Count)];
				var pot = Instantiate(_pot, new(pos.x, pos.y), quaternion.identity);
				pot.transform.SetParent(_resourceHolder);
			}
		}
	}
}
