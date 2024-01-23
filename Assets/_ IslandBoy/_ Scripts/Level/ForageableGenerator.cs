using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class ForageableGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap gameFloorTm;
        [SerializeField] private Tilemap _wallTm;
        [SerializeField] private Tilemap _floorTm;
        [SerializeField] private List<GameObject> forageables = new();

        private BoundsInt _bounds;

        void Start()
        {
            GameSignals.DAY_START.AddListener(RefreshBiome);
            gameFloorTm.CompressBounds();
            _bounds = gameFloorTm.cellBounds;
        }

        private void RefreshBiome(ISignalParameters parameters)
        {
            string test = "";
            for (int x = _bounds.min.x; x < _bounds.max.x; x++)
            {
                for (int y = _bounds.min.y; y < _bounds.max.y; y++)
                {
                    // skip if tile is occupied already
                    if (!canSpawnOnTile(x, y))
                    {
                        test += "-";
                        continue;
                    }
                    else 
                        test += "*";
                    // look at all surrounding tiles
                    List<GameObject> surroundings = getSurroundings(x,y);
                    int tile = Random.Range(0, surroundings.Count);
                    if (surroundings.Count == 0 || surroundings[tile] == null) // get random resource if empty tile chosen
                    {
                        int spawnOb = Random.Range(0, forageables.Count);
                        // spawn object if probability met
                        if (Random.Range(0, forageables[spawnOb].GetComponent<Resource>().SpawnRate) < forageables[spawnOb].GetComponent<Resource>().SpawnRate)
                            Instantiate(forageables[spawnOb], new Vector3(x, y), Quaternion.identity);
                    }
                    else // attempt to spawn chosen tile if not empty tile
                    {
                        if (Random.Range(0, surroundings[tile].GetComponent<Resource>().SpawnRate) < surroundings[tile].GetComponent<Resource>().SpawnRate)
                            Instantiate(surroundings[tile], new Vector3(x, y), Quaternion.identity);
                    }
                }
                test += '\n';
            }
            Debug.Log(test);
        }

        private bool canSpawnOnTile(int x, int y)
        {
            Vector3Int cellPos = new Vector3Int(x, y);
            // Find out if there is a tile on floor, it is not a floor, it is not a wall, and if there is no resource on it
            if (!gameFloorTm.HasTile(cellPos) || _floorTm.HasTile(cellPos) || _wallTm.HasTile(cellPos))
                return false;
            Collider2D[] cols = Physics2D.OverlapPointAll(new Vector2(x,y));
            foreach(Collider2D c in cols)
            {
                if (c.gameObject.tag == "RSC")
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
        }
    }
}
