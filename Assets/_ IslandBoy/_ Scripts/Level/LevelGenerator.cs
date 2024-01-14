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
		
		private int[,] _map;
		
		private void Start()
		{
			Generate();
		}
		
		[Button("Generate")]
		private void Generate()
		{
			Reset();
			GenerateBlob();
			GenerateMap();
			SmoothMap();	
			CreateTiles();
		}

		[Button("ResetTiles")]
		private void Reset()
		{
			_floorTilemap.ClearAllTiles();
			_wallTilemap.ClearAllTiles();
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
					}
					else
					{
						// Instantiate and position a floor tile
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
	}
}
