using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class ForageableGenerator : SerializedMonoBehaviour
    {
        [SerializeField] private Tilemap gameFloorTm;
        [SerializeField] private Tilemap _wallTm;
        [SerializeField] private Tilemap _floorTm;
        [SerializeField] private TileBase placementTile; // Tile to place forageables on
        [SerializeField] private Dictionary<GameObject, double> forageables = new();

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
                    if (!canSpawnOnTile(x, y)) // skip if there's no floor tile
                        test += "-";//continue;
                    else
                        test += "*";
                    /*
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
                    */
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

                    Vector3Int cellPos = new Vector3Int(sx, sy);
                    surroundings.Add(gameFloorTm.GetInstantiatedObject(cellPos));
                }
            }
            return surroundings;
        }
    }
}
