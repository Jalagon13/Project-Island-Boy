using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Bed : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;

        private bool _canCheck = false;


        public void Start()
        {
            _canCheck = true;
        }

        public void TryToEndDay() // connected to bed button
        {
            if (!_canCheck) return;

            Stack<Vector3Int> tilesToCheck = new();
            List<Vector3Int> floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
            List<Vector3Int> wallTilePositions = new(); // list of wall tiles around the free space
            List<Vector3Int> doorPositions = new(); // list of all positions of doors if any door is found
            int maxHouseSpaceTiles = 50;

            var checkPos = Vector3Int.FloorToInt(transform.root.position);
            tilesToCheck.Push(checkPos);

            while (tilesToCheck.Count > 0)
            {
                var p = tilesToCheck.Pop();

                // if there is a tile without floor or wall then it is not an enclosed area
                if (!_tmr.FloorTilemap.HasTile(p) && !_tmr.WallTilemap.HasTile(p) && !HasDoor(p))
                {
                    //_feedbackHolder.DisplayFeedback("Not valid housing. Make sure the area around you is enclosed.", Color.yellow);
                    Debug.Log("Can't Sleep area is not enclosed");
                    return;
                }

                // if there is a door, add it do the door positions
                if (HasDoor(p))
                {
                    doorPositions.Add(p);
                    continue;
                }

                // if tile has a wall, continue
                if (_tmr.WallTilemap.HasTile(p))
                {
                    wallTilePositions.Add(p);
                    continue;
                }

                // add floor tile to floorTilePositions and push new tiles to check
                if (!floorTilePositions.Contains(p))
                {
                    floorTilePositions.Add(p);

                    tilesToCheck.Push(new Vector3Int(p.x - 1, p.y));
                    tilesToCheck.Push(new Vector3Int(p.x + 1, p.y));
                    tilesToCheck.Push(new Vector3Int(p.x, p.y - 1));
                    tilesToCheck.Push(new Vector3Int(p.x, p.y + 1));
                }
            }

            // if floor tile positions are greater than maxHouseSpaceTiles, then housing is too big.
            if (floorTilePositions.Count > maxHouseSpaceTiles)
            {
                //_feedbackHolder.DisplayFeedback("Not valid housing. Enclosed space too large.", Color.yellow);
                Debug.Log("Can't Sleep Space too large");
                return;
            }

            // if there is no doors found then housing not valid
            if (doorPositions.Count <= 0)
            {
                //_feedbackHolder.DisplayFeedback("Not valid housing. No doors found.", Color.yellow);
                Debug.Log("Can't Sleep no door found");
                return;
            }

            // loop through all doors found
            // a valid door is if the door is flanked n/s or e/w by wall tiles and check if one
            // side of the empty space door is part of the floor tile positions and one side is not.
            bool validDoorFound = false;

            foreach (Vector3Int p in doorPositions)
            {
                Vector3Int nn = new(p.x, p.y + 1);
                Vector3Int sn = new(p.x, p.y - 1);
                Vector3Int en = new(p.x + 1, p.y);
                Vector3Int wn = new(p.x - 1, p.y);

                if (_tmr.WallTilemap.HasTile(nn) && _tmr.WallTilemap.HasTile(sn))
                {
                    int counter = 0;

                    if (floorTilePositions.Contains(en))
                        counter++;

                    if (floorTilePositions.Contains(wn))
                        counter++;

                    if (counter == 1)
                    {
                        validDoorFound = true;
                        break;
                    }
                }

                if (_tmr.WallTilemap.HasTile(en) && _tmr.WallTilemap.HasTile(wn))
                {
                    int counter = 0;

                    if (floorTilePositions.Contains(nn))
                        counter++;

                    if (floorTilePositions.Contains(sn))
                        counter++;

                    if (counter == 1)
                    {
                        validDoorFound = true;
                        break;
                    }
                }
            }

            if (!validDoorFound)
            {
                //_feedbackHolder.DisplayFeedback("Not valid housing. There is no door that leads outside of this space.", Color.yellow);
                Debug.Log("Can't Sleep no valid door");
                return;
            }

            Debug.Log("Going to sleep now!");
            DayManager.Instance.EndDay();
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


    }
}
