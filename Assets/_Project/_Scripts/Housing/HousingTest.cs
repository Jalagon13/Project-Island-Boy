using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class HousingTest : MonoBehaviour
    {
        [SerializeField] private Tilemap _floor;
        [SerializeField] private Tilemap _walls;

        private Vector2 _startPos = Vector2.zero;
        private int _houseSpaceCounter;

        public void TestHousing()
        {
            FloodFill(_startPos);
        }

        private void FloodFill(Vector2 pos)
        {
            Stack<TileBase> tiles = new();
            var floorTile = _floor.GetTile(Vector3Int.FloorToInt(pos));
            tiles.Push(floorTile);

            while(tiles.Count > 0)
            {
                TileBase t = tiles.Pop();
                if(t != null)
                {
                    if (!_walls.HasTile(Vector3Int.FloorToInt(pos)))
                    {
                        _houseSpaceCounter++;
                        tiles.Push(_floor.GetTile(Vector3Int.FloorToInt(new Vector2(pos.x - 1, pos.y))));
                        tiles.Push(_floor.GetTile(Vector3Int.FloorToInt(new Vector2(pos.x + 1, pos.y))));
                        tiles.Push(_floor.GetTile(Vector3Int.FloorToInt(new Vector2(pos.x, pos.y - 1))));
                        tiles.Push(_floor.GetTile(Vector3Int.FloorToInt(new Vector2(pos.x, pos.y + 1))));
                    }
                    else
                    {
                        // there is a floor tile but there is a wall on top of it
                    }
                }
                else
                {
                    // no floor tile in this position
                }
            }

            Debug.Log($"valid housing spaces: {_houseSpaceCounter}");
        }
    }
}
