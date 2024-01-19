using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class ForageableGenerator : SerializedMonoBehaviour
    {
        public Tilemap gameFloor;
        public TileBase placementTile; // Tile to place forageables on
        public Dictionary<GameObject, double> forageables = new();

        private BoundsInt bounds;

        void Start()
        {
            GameSignals.DAY_START.AddListener(RefreshBiome);
            gameFloor.CompressBounds();
            bounds = gameFloor.cellBounds;
        }

        private void RefreshBiome(ISignalParameters parameters)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    // skip if tile is occupied already
                    if (!canSpawnOnTile(x, y))
                        continue;
                    // look at all surrounding tiles
                    List<GameObject> surroundings = getSurroundings(x,y);
                    GameObject referenceTile = surroundings[Random.Range(0, surroundings.Count)];
                    if (referenceTile == null)
                    {
                        // choose random forageable
                    }
                    else
                    {

                    }
                }
            }
        }

        private bool canSpawnOnTile(int x, int y)
        {
            Vector3Int cellPos = new Vector3Int(x, y, 0);
            return gameFloor.GetTile(cellPos) == placementTile && gameFloor.GetInstantiatedObject(cellPos) == null;// figure out what empty tile returns
        }

        private List<GameObject> getSurroundings(int x, int y)
        {
            List<GameObject> surroundings = new List<GameObject>();
            for (int sx = x - 1; sx <= x + 1; sx++)
            {
                for (int sy = y - 1; sy <= y + 1; sy++)
                {
                    // skip if out of bounds or is the middle tile or is not a forageable
                    if (sx < bounds.min.x || sx > bounds.max.x ||
                        sy < bounds.min.y || sy < bounds.max.y ||
                        (sx == x && sy == y)) // add latter later
                        continue;

                    Vector3Int cellPos = new Vector3Int(sx, sy, 0);
                    surroundings.Add(gameFloor.GetInstantiatedObject(cellPos));
                }
            }
            return surroundings;
        }
    }
}
