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

        public void TestHousing()
        {
            FloodFill(_startPos);
        }

        private void FloodFill(Vector2 startPos)
        {
            Stack<Vector2> tiles = new();
            Stack<Vector2> validPositions = new();
            tiles.Push(startPos);

            while(tiles.Count > 0)
            {
                Vector2 p = tiles.Pop();
                if(_floor.HasTile(Vector3Int.FloorToInt(p)))
                {
                    if (_walls.HasTile(Vector3Int.FloorToInt(p)))
                    {
                        continue;
                    }
                    else if (!validPositions.Contains(p))
                    {
                        validPositions.Push(p);
                        tiles.Push(new Vector2(p.x - 1, p.y));
                        tiles.Push(new Vector2(p.x + 1, p.y));
                        tiles.Push(new Vector2(p.x, p.y - 1));
                        tiles.Push(new Vector2(p.x, p.y + 1));
                    }
                }
                else if(_walls.HasTile(Vector3Int.FloorToInt(p)))
                {
                    continue;
                }
                else
                {
                    Debug.Log($"There is an empty space at {p} therefore this is not a house");
                    return;
                }
            }

            Debug.Log($"This is valid housing! # of spaces: {validPositions.Count}");
            return;
        }
    }
}
