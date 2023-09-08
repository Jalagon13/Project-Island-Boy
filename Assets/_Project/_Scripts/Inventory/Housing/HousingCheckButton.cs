using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class HousingCheckButton : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private GameObject _minerNpcPrefab;

        private bool _minerSpawned;

        public void CheckHousing()
        {
            if (_minerSpawned)
                return;

            // checks if this is an enclosed space with flooring everywhere
            Stack<Vector3Int> tilesToCheck = new();
            List<Vector3Int> floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
            tilesToCheck.Push(Vector3Int.FloorToInt(transform.root.position));
            while (tilesToCheck.Count > 0)
            {
                var p = tilesToCheck.Pop();

                if (_tmr.FloorTilemap.HasTile(p))
                {
                    if (_tmr.WallTilemap.HasTile(p) || HasDoor(p))
                    {
                        continue;
                    }
                    else if (!floorTilePositions.Contains(p))
                    {
                        floorTilePositions.Add(p);
                        tilesToCheck.Push(new Vector3Int(p.x - 1, p.y));
                        tilesToCheck.Push(new Vector3Int(p.x + 1, p.y));
                        tilesToCheck.Push(new Vector3Int(p.x, p.y - 1));
                        tilesToCheck.Push(new Vector3Int(p.x, p.y + 1));
                    }
                }
                else if (_tmr.WallTilemap.HasTile(p) || HasDoor(p))
                {
                    continue;
                }
                else
                {
                    Debug.Log($"There is an empty space at {p} therefore {transform.root.position} is not a house");
                    return;
                }
            }

            // first check how many free floor spaces are there in the floorTilePositions list
            List<Vector3Int> freeFloorSpaces = new();
            foreach (Vector3Int pos in floorTilePositions)
            {
                if (TileAreaClear(pos))
                {
                    freeFloorSpaces.Add(pos);
                }
            }

            // if minimum number of clear floor tiles is below the set minimum, this is not a valid housing
            int minClearFloorTileAmount = 5;
            if (freeFloorSpaces.Count < minClearFloorTileAmount)
            {
                Debug.Log($"Not enough free space in this closed area (clear floor tiles count: " +
                    $"{freeFloorSpaces.Count} | minimum clear tiles needed: {minClearFloorTileAmount}) therefore " +
                    $"{transform.root.position} is not a house");
                return;
            }

            // find a random position of the clear floor tiles and spawn the NPC there
            Vector3Int randFloorSpace = freeFloorSpaces[Random.Range(0, freeFloorSpaces.Count)];
            Vector2 spawnNpcPosition = new(randFloorSpace.x + 0.5f, randFloorSpace.y + 0.5f);

            Debug.Log($"{transform.root.position} is valid housing! # of spaces: {freeFloorSpaces.Count}");
            Instantiate(_minerNpcPrefab, spawnNpcPosition, Quaternion.identity);
            _minerSpawned = true;
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
