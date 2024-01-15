using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
		[SerializeField] private Clickable _iron;
		[SerializeField] private TileBase _rscTile;
		
		private int[,] _map;
		
		private Transform _resourceHolder;
		private List<Vector2Int> _wallSpots;
		
		private void Awake()
		{
			_resourceHolder = transform.GetChild(2);
			_wallSpots = new();
		}
		
		private void Start()
		{
			GenerateWallsFloors();
			GenerateResourceNodes();
		}
		
		#region Map Generation
		
		[Button("Generate")]
		private void GenerateWallsFloors()
		{
			Reset();
			GenerateBlob();
			GenerateMap();
			SmoothMap();	
			CreateTiles();
			GenerateResourceNodes();
		}

		[Button("ResetTiles")]
		private void Reset()
		{
			_resourceHolder = transform.GetChild(2);
			_floorTilemap.ClearAllTiles();
			_wallTilemap.ClearAllTiles();
			_wallSpots.Clear();
			
			if(_resourceHolder.transform.childCount > 0)
			{
				foreach (Transform transform in _resourceHolder.transform)
				{
					Destroy(transform.gameObject);
				}
			}
		}
		
		void GenerateBlob()
		{
			Vector3Int centerPosition = new(_width / 2, _height / 2);

			for (int x = -_centerBlobRadius; x <= _centerBlobRadius; x++)
			{
				for (int y = -_centerBlobRadius; y <= _centerBlobRadius; y++)
				{
					Vector3Int tilePosition = new Vector3Int(centerPosition.x + x, centerPosition.y + y, centerPosition.z);

					if (IsInsideBlob(tilePosition, centerPosition))
					{
						_floorTilemap.SetTile(tilePosition, _floorTile);
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
						PlaceTile(_wallTilemap, _wallTile, x, y);
						_wallSpots.Add(new(x, y));
					}
					else
					{
						// Instantiate and position an air tile
						PlaceTile(_wallTilemap, null, x, y);
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
		
		private void GenerateResourceNodes()
		{
			int clumpCount = 10;
			int nodesPerClumpAvg = 20;
			
			for (int clump = 0; clump < clumpCount; clump++)
			{
				Vector2 clumpPosition = _wallSpots[Random.Range(0, _wallSpots.Count - 1)];
				int nodeCount = Random.Range(nodesPerClumpAvg - 5, nodesPerClumpAvg + 5);

				for (int node = 0; node < nodesPerClumpAvg; node++)
				{
					Vector2 offset = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
					Vector2 nodePosition = clumpPosition + offset;

					// Ensure the node is within the grid boundaries
					nodePosition.x = Mathf.Clamp(nodePosition.x, 0, _width - 1);
					nodePosition.y = Mathf.Clamp(nodePosition.y, 0, _height - 1);

					// Instantiate the resource node prefab at the calculated position
					// var rsc = Instantiate(_iron, nodePosition, Quaternion.identity);
					// rsc.transform.SetParent(_resourceHolder);
					PlaceTile(_wallTilemap, _rscTile, (int)nodePosition.x, (int)nodePosition.y);
				}
			}
		}
	}
}
