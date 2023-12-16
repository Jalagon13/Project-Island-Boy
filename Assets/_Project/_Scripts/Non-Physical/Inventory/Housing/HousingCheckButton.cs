using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class HousingCheckButton : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReference _tmr;
        //[SerializeField] private AdventurerEntity _adventurerPrefab;
        [SerializeField] private FeedbackHolder _feedbackHolder;
        [SerializeField] private AudioClip _sonarSound;
        [SerializeField] private AudioClip _moveInSound;
        [SerializeField] private List<Resource> _furnitureCheckList;

        private Button _button;
        private Image _buttonBgImage;
        private Image _buttonIconImage;
        private HousingHoverImage _buttonHoverImage;
        //private AdventurerEntity _adventurerReference;
        private Color _originalButtonColor;

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
            _buttonBgImage = transform.GetComponent<Image>();
            _buttonIconImage = transform.GetChild(0).GetComponent<Image>();
            _buttonHoverImage = transform.GetChild(0).GetComponent<HousingHoverImage>();
            _originalButtonColor = _buttonBgImage.color;
        }

        private void Start()
        {
            //string header = $"Attract a {_adventurerPrefab.AdventurerName} to your island!";
            string content = "Requirements:<br> * Bed<br>";

            foreach (Resource requirements in _furnitureCheckList)
            {
                content += $" * {requirements.ResourceName}<br>";
            }

            //_buttonHoverImage.Initialize(header, content);
        }

        public void CheckHousing() // attached to button
        {
            //if (_adventurerReference != null)
            //{
            //    _feedbackHolder.DisplayFeedback($"{_adventurerPrefab.name} is already occupying a room", Color.green);
            //    return;
            //}

            // checks if this is an enclosed space with flooring everywhere
            Stack<Vector3Int> tilesToCheck = new();
            List<Vector3Int> floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
            List<Vector3Int> wallTilePositions = new(); // list of wall tiles around the free space
            List<Vector3Int> doorPositions = new(); // list of all positions of doors if any door is found
            List<Resource> foundFurniture = new();
            List<Bed> foundBeds = new();
            int maxHouseSpaceTiles = 50;

            tilesToCheck.Push(Vector3Int.FloorToInt(transform.root.position));

            while (tilesToCheck.Count > 0)
            {
                var p = tilesToCheck.Pop();

                // if there is a tile without floor or wall then it is not an enclosed area
                //if (!_tmr.FloorTilemap.HasTile(p) && !_tmr.WallTilemap.HasTile(p) && !HasDoor(p))
                //{
                //    _feedbackHolder.DisplayFeedback("Not valid housing. Make sure the area around you is enclosed.", Color.yellow);
                //    return;
                //}

                // if there is a door, add it do the door positions
                if (HasDoor(p))
                {
                    doorPositions.Add(p);
                    continue;
                }

                // if tile has a wall, continue
                //if (_tmr.WallTilemap.HasTile(p))
                //{
                //    wallTilePositions.Add(p);
                //    continue;
                //}

                // check for checklist RSC here, 
                var centerPos = new Vector2(p.x + 0.5f, p.y + 0.5f);
                var colliders1 = Physics2D.OverlapCircleAll(centerPos, 0.25f);
                bool reqFound = false;

                foreach (var col in colliders1)
                {
                    if(col.TryGetComponent(out Resource rsc))
                    {
                        if (foundFurniture.Contains(rsc))
                            continue;

                        foreach (Resource req in _furnitureCheckList)
                        {
                            if(rsc.ResourceName == req.ResourceName)
                            {
                                foundFurniture.Add(rsc);
                                reqFound = true;
                            }
                        }
                    }
                }

                if (reqFound)
                    continue;

                // If there is a bed add it to foundBeds List
                var colliders3 = Physics2D.OverlapCircleAll(centerPos, 0.25f);
                foreach (var col in colliders3)
                {
                    if (col.TryGetComponent(out Bed bed))
                    {
                        foundBeds.Add(bed);
                    }
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

            if(foundBeds.Count <= 0)
            {
                // if there is no bed, say there is no bed
                _feedbackHolder.DisplayFeedback("Not valid housing. There is no bed.", Color.yellow);
                return;
            }
            else
            {
                // check if there are any already registered beds
                //foreach (Bed bed in foundBeds)
                //{
                //    if (bed.Registered)
                //    {
                //        _feedbackHolder.DisplayFeedback("Not valid housing. An NPC is already occupying this space.", Color.yellow);
                //        return;
                //    }
                //}
            }

            // if floor tile positions are greater than maxHouseSpaceTiles, then housing is too big.
            if (floorTilePositions.Count > maxHouseSpaceTiles)
            {
                _feedbackHolder.DisplayFeedback("Not valid housing. Enclosed space too large.", Color.yellow);
                return;
            }

            // if there is no doors found then housing not valid
            if (doorPositions.Count <= 0)
            {
                _feedbackHolder.DisplayFeedback("Not valid housing. No doors found.", Color.yellow);
                return;
            }

            // if checklist is not compelete, then housing not valid
            foreach (Resource req in _furnitureCheckList)
            {
                bool furnitureFound = false;

                foreach (Resource rsc in foundFurniture)
                {
                    if(rsc.ResourceName == req.ResourceName)
                    {
                        furnitureFound = true;
                    }
                }

                if (!furnitureFound)
                {
                    _feedbackHolder.DisplayFeedback("Not valid housing. Furniture requirements not met.", Color.yellow);
                    return;
                }
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

                //if (_tmr.WallTilemap.HasTile(nn) && _tmr.WallTilemap.HasTile(sn))
                //{
                //    int counter = 0;

                //    if (floorTilePositions.Contains(en))
                //        counter++;

                //    if (floorTilePositions.Contains(wn))
                //        counter++;

                //    if(counter == 1)
                //    {
                //        validDoorFound = true;
                //        break;
                //    }
                //}

                //if (_tmr.WallTilemap.HasTile(en) && _tmr.WallTilemap.HasTile(wn))
                //{
                //    int counter = 0;

                //    if (floorTilePositions.Contains(nn))
                //        counter++;

                //    if (floorTilePositions.Contains(sn))
                //        counter++;

                //    if (counter == 1)
                //    {
                //        validDoorFound = true;
                //        break;
                //    }
                //}
            }

            if (!validDoorFound)
            {
                _feedbackHolder.DisplayFeedback("Not valid housing. There is no door that leads outside of this space.", Color.yellow);
                return;
            }

            // find a random position of the clear floor tiles and spawn the NPC there
            Vector3Int randFloorSpace = floorTilePositions[Random.Range(0, floorTilePositions.Count)];
            Vector2 spawnNpcPosition = new(randFloorSpace.x + 0.5f, randFloorSpace.y + 0.5f);

            _feedbackHolder.DisplayFeedback("Housing found! NPC has moved in!", Color.green);
            //_adventurerReference = Instantiate(_adventurerPrefab, spawnNpcPosition, Quaternion.identity);
            //_adventurerReference.RegisterBed(foundBeds[0]);
            //foundBeds[0].RegisterBed(_furnitureCheckList, _adventurerReference);

        }

        public void DisableButton()
        {
            var tempColor = _originalButtonColor;
            tempColor.a = 0.3f;
            _buttonBgImage.color = tempColor;
            _buttonIconImage.color = tempColor;
            _button.enabled = false;
            _buttonHoverImage.enabled = false;
        }

        public void EnableButton()
        {
            var tempColor = _originalButtonColor;
            tempColor.a = 1;
            _buttonBgImage.color = tempColor;
            _buttonIconImage.color = tempColor;
            _button.enabled = true;
            _buttonHoverImage.enabled = true;
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
