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
            Stack<Vector3Int> tiles = new();
            Stack<Vector3Int> validPositions = new();
            tiles.Push(Vector3Int.FloorToInt(startPos));

            while(tiles.Count > 0)
            {
                var p = Vector3Int.FloorToInt(tiles.Pop());
                if(_floor.HasTile(p))
                {
                    if (_walls.HasTile(p) || HasDoor(p))
                    {
                        continue;
                    }
                    else if (!validPositions.Contains(p))
                    {
                        validPositions.Push(p);
                        tiles.Push(new Vector3Int(p.x - 1, p.y));
                        tiles.Push(new Vector3Int(p.x + 1, p.y));
                        tiles.Push(new Vector3Int(p.x, p.y - 1));
                        tiles.Push(new Vector3Int(p.x, p.y + 1));
                    }
                }
                else if(_walls.HasTile(p) || HasDoor(p))
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

        private bool HasDoor(Vector3Int pos)
        {
            var centerPos = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
            var colliders = Physics2D.OverlapCircleAll(centerPos, 0.1f);

            foreach (Collider2D col in colliders)
            {
                if(col.TryGetComponent(out Door door))
                    return true;
            }

            return false;
        }
    }
}
