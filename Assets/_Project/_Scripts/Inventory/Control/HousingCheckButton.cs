using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class HousingCheckButton : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;

        private void CheckHousing(Vector2 startPos)
        {
            Stack<Vector3Int> tiles = new();
            List<Vector3Int> floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
            tiles.Push(Vector3Int.FloorToInt(startPos));

            // checks if this is an enclosed space with flooring everywhere
            while (tiles.Count > 0)
            {
                var p = Vector3Int.FloorToInt(tiles.Pop());

                if (_tmr.FloorTilemap.HasTile(p))
                {
                    if (_tmr.WallTilemap.HasTile(p) || HasDoor(p))
                    {
                        continue;
                    }
                    else if (!floorTilePositions.Contains(p))
                    {
                        floorTilePositions.Add(p);
                        tiles.Push(new Vector3Int(p.x - 1, p.y));
                        tiles.Push(new Vector3Int(p.x + 1, p.y));
                        tiles.Push(new Vector3Int(p.x, p.y - 1));
                        tiles.Push(new Vector3Int(p.x, p.y + 1));
                    }
                }
                else if (_tmr.WallTilemap.HasTile(p) || HasDoor(p))
                {
                    continue;
                }
                else
                {
                    Debug.Log($"There is an empty space at {p} therefore {startPos} is not a house");
                    return;
                }
            }

            // first check how many free floor spaces are there in the floorTilePositions list
            List<Vector3Int> clearFloorTiles = new();
            foreach (var pos in floorTilePositions)
            {
                if (TileAreaClear(pos))
                {
                    clearFloorTiles.Add(pos);
                }
            }

            // if minimum number of clear floor tiles is below the set minimum, this is not a valid housing
            int minClearFloorTileAmount = 5;
            if (clearFloorTiles.Count < minClearFloorTileAmount)
            {
                Debug.Log($"Not enough free space in this closed area (clear floor tiles count: " +
                    $"{clearFloorTiles.Count} | minimum clear tiles needed: {minClearFloorTileAmount}) therefore {startPos} is not a house");
                return;
            }

            // find a random position of the clear floor tiles and spawn the NPC there
            //if (_minerNpcSpawned) return;

            //var randFloorTile = clearFloorTiles[Random.Range(0, clearFloorTiles.Count)];
            //var spawnNpcPosition = new Vector2(randFloorTile.x + 0.5f, randFloorTile.y + 0.5f);

            //Debug.Log($"{startPos} is valid housing! # of spaces: {clearFloorTiles.Count}");
            //Instantiate(_minerNpcPrefab, spawnNpcPosition, Quaternion.identity);
            //_minerNpcSpawned = true;
            return;
        }

        private bool HasDoor(Vector3Int pos)
        {
            var centerPos = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
            var colliders = Physics2D.OverlapCircleAll(centerPos, 0.1f);

            foreach (Collider2D col in colliders)
            {
                if (col.TryGetComponent(out Door door))
                    return true;
            }

            return false;
        }



        private bool TileAreaClear(Vector3Int pos)
        {
            var colliders = Physics2D.OverlapCircleAll(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.25f);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.layer == 3)
                    return false;
            }

            return true;
        }

    }
}
