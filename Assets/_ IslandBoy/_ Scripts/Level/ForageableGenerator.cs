using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
	public class ForageableGenerator : MonoBehaviour
	{
		[SerializeField] private Tilemap _grassTm;
		[SerializeField] private TilemapObject _wallTm;
		[SerializeField] private TilemapObject _floorTm;
		[SerializeField] private List<GameObject> forageables = new();

		private BoundsInt _bounds;

		private void Awake()
		{
			GameSignals.DAY_START.AddListener(RefreshBiome);
		}
		
		private void OnDestroy()
		{
			GameSignals.DAY_START.RemoveListener(RefreshBiome);
		}

		void Start()
		{
			_grassTm.CompressBounds();
			_bounds = _grassTm.cellBounds;
		}

		private void RefreshBiome(ISignalParameters parameters)
		{
			StartCoroutine(Delay());
		}

		private IEnumerator Delay()
		{
			yield return new WaitForEndOfFrame();
			
			// string test = "";
			for (int x = _bounds.min.x; x < _bounds.max.x; x++)
			{
				for (int y = _bounds.min.y; y < _bounds.max.y; y++)
				{
					Vector3Int tilePosition = new Vector3Int(x, y, 0);

					// Access the tile at the current position
					TileBase tile = _grassTm.GetTile(tilePosition);

					if (tile != null)
					{
						// Do something with the tile or its position
						if(CanSpawnRscHere(tilePosition))
						{
							var forageableToSpawn = forageables[Random.Range(0, forageables.Count - 1)];
							var rsc = forageableToSpawn.GetComponent<Resource>();
							// if(rsc.SpawnRate > Random.Range(0, 100))
							// {
							// 	//Debug.Log($"Spawning {rsc.name}");
							// 	Instantiate(forageableToSpawn, tilePosition, Quaternion.identity);
							// }
						}
					}
					
					
					// // look at all surrounding tiles
					// List<GameObject> surroundings = getSurroundings(x,y);
					// int tile = Random.Range(0, surroundings.Count);
					// if (surroundings.Count == 0 || surroundings[tile] == null) // get random resource if empty tile chosen
					// {
					// 	int spawnOb = Random.Range(0, forageables.Count);
					// 	// spawn object if probability met
					// 	if (Random.Range(0, forageables[spawnOb].GetComponent<Resource>().SpawnRate) < forageables[spawnOb].GetComponent<Resource>().SpawnRate)
					// 		Instantiate(forageables[spawnOb], new Vector3(x, y), Quaternion.identity);
					// }
					// else // attempt to spawn chosen tile if not empty tile
					// {
					// 	if (Random.Range(0, surroundings[tile].GetComponent<Resource>().SpawnRate) < surroundings[tile].GetComponent<Resource>().SpawnRate)
					// 		Instantiate(surroundings[tile], new Vector3(x, y), Quaternion.identity);
					// }
				}
			}
		}
		
		private bool CanSpawnRscHere(Vector3Int target)
		{
			return IsClear(new(target.x + 0.5f, target.y + 0.5f)) && !_wallTm.Tilemap.HasTile(target) && !_floorTm.Tilemap.HasTile(target);
		}
		
		private bool IsClear(Vector2 pos)
		{
			var colliders = Physics2D.OverlapBoxAll(pos, new Vector2(0.5f, 0.5f), 0);

			foreach(Collider2D col in colliders)
			{
				if(col.gameObject.layer == 3) 
					return false;
			}

			return true;
		}
		/*
		private bool canSpawnOnTile(int x, int y)
		{
			Vector3Int cellPos = new Vector3Int(x, y);
			// Find out if there is a tile on floor, it is not a floor, it is not a wall, and if there is no resource on it
			if (!_grassTm.HasTile(cellPos) || _floorTm.Tilemap.HasTile(cellPos) || _wallTm.Tilemap.HasTile(cellPos))
				return false;
			Collider2D[] cols = Physics2D.OverlapPointAll(new Vector2(x,y));
			foreach(Collider2D c in cols)
			{
				if (c.TryGetComponent<Resource>(out var rsc))
					return false; // means theres a resouce at the space
			}
			return true;
		}

		private List<GameObject> getSurroundings(int x, int y)
		{
			List<GameObject> surroundings = new List<GameObject>();
			for (int sx = x - 1; sx <= x + 1; sx++)
			{
				for (int sy = y - 1; sy <= y + 1; sy++)
				{
					// skip if out of bounds or is the middle tile or is not a forageable
					if (sx < _bounds.min.x || sx > _bounds.max.x ||
						sy < _bounds.min.y || sy < _bounds.max.y ||
						(sx == x && sy == y)) // add latter later
						continue;

					if (canSpawnOnTile(sx, sy))
						surroundings.Add(null);
					else
					{
						Collider2D[] cols = Physics2D.OverlapPointAll(new Vector2(x, y));
						foreach (Collider2D c in cols)
						{
							if (c.gameObject.tag == "RSC" &&
								c.gameObject.GetComponent<Resource>() != null &&
								c.gameObject.GetComponent<Resource>().SpawnRate > 0) // may chage to have bool for spawn naturally
								surroundings.Add(c.gameObject);
						}
					}
				}
			}
			return surroundings;
		}*/
	}
}
