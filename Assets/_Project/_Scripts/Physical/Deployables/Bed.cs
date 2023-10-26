using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class Bed : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;

        private List<Vector3Int> _floorTilePositions = new();
        private bool _canCheck = false;

        public void Start()
        {
            _canCheck = true;
        }

        public void TryToEndDay() // connected to bed button
        {
            if (!_canCheck) return;

            if (!DayManager.Instance.CanSleep())
            {
                PopupMessage.Create(transform.position, "Too early to sleep!", Color.yellow, new(0.5f, 0.5f), 1f);
                return;
            }

            if (InValidSpace())
            {
                DayManager.Instance.EndDay();
            }
        }

        public bool InValidSpace() // check for floors and walls for house is valid. check furniture too. make floortilePos a global var.
        {
            Stack<Vector3Int> tilesToCheck = new();
            _floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
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
                    PopupMessage.Create(transform.position, "Area not enclosed!", Color.yellow, new(0.5f, 0.5f), 1f);
                    return false;
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
                if (!_floorTilePositions.Contains(p))
                {
                    _floorTilePositions.Add(p);

                    tilesToCheck.Push(new Vector3Int(p.x - 1, p.y));
                    tilesToCheck.Push(new Vector3Int(p.x + 1, p.y));
                    tilesToCheck.Push(new Vector3Int(p.x, p.y - 1));
                    tilesToCheck.Push(new Vector3Int(p.x, p.y + 1));
                }
            }

            // if floor tile positions are greater than maxHouseSpaceTiles, then housing is too big.
            if (_floorTilePositions.Count > maxHouseSpaceTiles)
            {
                PopupMessage.Create(transform.position, "Space too large!", Color.yellow, new(0.5f, 0.5f), 1f);
                return false;
            }

            // if there is no doors found then housing not valid
            if (doorPositions.Count <= 0)
            {
                PopupMessage.Create(transform.position, "No door found!", Color.yellow, new(0.5f, 0.5f), 1f);
                return false;
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

                    if (_floorTilePositions.Contains(en))
                        counter++;

                    if (_floorTilePositions.Contains(wn))
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

                    if (_floorTilePositions.Contains(nn))
                        counter++;

                    if (_floorTilePositions.Contains(sn))
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
                PopupMessage.Create(transform.position, "No valid door!", Color.yellow, new(0.5f, 0.5f), 1f);
                return false;
            }

            return true;
        }

        public bool FindFurniture(NpcObject npc)
        {
            // if in valid space
            if (InValidSpace())
            {
                List<Resource> foundFurniture = new();

                // check floor tile positions and return any furniture found
                _floorTilePositions.ForEach(p => 
                {
                    var centerPos = new Vector2(p.x + 0.5f, p.y + 0.5f);
                    var colliders = Physics2D.OverlapCircleAll(centerPos, 0.25f);

                    foreach (var col in colliders)
                    {
                        if (col.TryGetComponent(out Resource rsc))
                        {
                            if (foundFurniture.Contains(rsc))
                                continue;

                            foreach (DeployObject furniture in npc.FurnitureCheckList)
                            {
                                var go = furniture.PrefabToDeploy;
                                var furnitureRsc = go.GetComponent<Resource>();

                                if (furnitureRsc.ResourceName == rsc.ResourceName)
                                {
                                    foundFurniture.Add(rsc);
                                    break;
                                }
                            }
                        }
                    }
                });

                foreach (DeployObject furniture in npc.FurnitureCheckList)
                {
                    bool furnitureFound = false;
                    var go = furniture.PrefabToDeploy;
                    var furnitureRsc = go.GetComponent<Resource>();

                    foreach (Resource found in foundFurniture)
                    {
                        if(furnitureRsc.ResourceName == found.ResourceName)
                        {
                            furnitureFound = true;
                        }
                    }

                    if (!furnitureFound)
                    {
                        return false;
                    }
                }
            }

            return true;
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
