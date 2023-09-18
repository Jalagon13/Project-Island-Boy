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
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private AdventurerEntity _adventurerPrefab;
        [SerializeField] private AudioClip _sonarSound;
        [SerializeField] private AudioClip _moveInSound;
        [SerializeField] private List<Resource> _checkList;

        private Button _button;
        private Image _buttonBgImage;
        private Image _buttonIconImage;
        private HousingHoverImage _buttonHoverImage;
        private TextMeshProUGUI _feedbackText;
        private AdventurerEntity _adventurerReference;
        private Color _originalButtonColor;

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
            _buttonBgImage = transform.GetComponent<Image>();
            _buttonIconImage = transform.GetChild(0).GetComponent<Image>();
            _buttonHoverImage = transform.GetChild(0).GetComponent<HousingHoverImage>();

            _originalButtonColor = _buttonBgImage.color;
            _feedbackText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _feedbackText.enabled = false;
        }

        private void OnDisable()
        {
            EnableButton();
        }

        public void CheckHousing() // attached to button
        {
            if (_adventurerReference != null)
                return;

            AudioManager.Instance.PlayClip(_sonarSound, false, false);

            // checks if this is an enclosed space with flooring everywhere
            Stack<Vector3Int> tilesToCheck = new();
            List<Vector3Int> floorTilePositions = new(); // list of positions of tile that have a floor and no wall or door on it.
            List<Vector3Int> wallTilePositions = new(); // list of wall tiles around the free space
            int maxHouseSpaceTiles = 2220;

            tilesToCheck.Push(Vector3Int.FloorToInt(transform.root.position));

            while (tilesToCheck.Count > 0)
            {
                var p = tilesToCheck.Pop();

                // if there is a tile without floor or wall then it is not an enclosed area
                if (!_tmr.FloorTilemap.HasTile(p) && !_tmr.WallTilemap.HasTile(p) && !HasDoor(p))
                {
                    DisplayFeedback("No valid housing found. Make sure the area around you is enclosed.", Color.yellow);
                    return;
                }

                // if tile has a wall, continue
                if (_tmr.WallTilemap.HasTile(p))
                {
                    wallTilePositions.Add(p);
                    continue;
                }

                // check for checklist RSC here

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
            if(floorTilePositions.Count > maxHouseSpaceTiles)
            {
                DisplayFeedback("No valid housing found. Enclosed space too large", Color.yellow);
                return;
            }

            // loop through all wall tiles and find the "flagged" tiles meaning a wall tile is sticking out and put that in it's own
            // flagged wall tile list
            // loop through all flagged wall tiles untill all appropriate wall tiles are flagged by checking it's neighbors
            // now I have all the border wall tiles to the enclosed space
            // with these tiles, check for a door by checking if the door is flanked by 2 NON-flagged wall tiles north/south or east/west
            // if there is at least one door, than this requirement is good.
            foreach (Vector3Int pos in wallTilePositions)
            {
                _tmr.WallTilemap.SetTile(pos, null);
            }








            //// first check how many free floor spaces are there in the floorTilePositions list
            //List<Vector3Int> freeFloorSpaces = new();
            //foreach (Vector3Int pos in floorTilePositions)
            //{
            //    if (TileAreaClear(pos))
            //    {
            //        freeFloorSpaces.Add(pos);
            //    }
            //}

            //// if minimum number of clear floor tiles is below the set minimum, this is not a valid housing
            //int minClearFloorTileAmount = 5;
            //if (freeFloorSpaces.Count < minClearFloorTileAmount)
            //{
            //    DisplayFeedback("No valid housing found around you. Space is too small!", Color.yellow);
            //    return;
            //}

            //// find a random position of the clear floor tiles and spawn the NPC there
            //Vector3Int randFloorSpace = freeFloorSpaces[Random.Range(0, freeFloorSpaces.Count)];
            //Vector2 spawnNpcPosition = new(randFloorSpace.x + 0.5f, randFloorSpace.y + 0.5f);

            //StartCoroutine(SpawnAdventurer(spawnNpcPosition));
        }

        private IEnumerator SpawnAdventurer(Vector2 spawnPos)
        {
            yield return new WaitForSeconds(1f);

            DisplayFeedback("Housing found! Miner has moved in!", Color.green);

            _adventurerReference = Instantiate(_adventurerPrefab, spawnPos, Quaternion.identity);
            AudioManager.Instance.PlayClip(_moveInSound, false, false);
            //_pr.LevelSystem.AddExperience(3000);
        }

        private void DisplayFeedback(string text, Color textColor)
        {
            DisableButton();

            _feedbackText.enabled = true;
            _feedbackText.text = text;
            _feedbackText.color = textColor;

            StartCoroutine(ButtonDelay());
        }

        private IEnumerator ButtonDelay()
        {
            yield return new WaitForSeconds(5f);

            EnableButton();
        }

        private void DisableButton()
        {
            var tempColor = _originalButtonColor;
            tempColor.a = 0.5f;
            _buttonBgImage.color = tempColor;
            _buttonIconImage.color = tempColor;
            _button.enabled = false;
            _buttonHoverImage.enabled = false;
        }

        private void EnableButton()
        {
            var tempColor = _originalButtonColor;
            tempColor.a = 1;
            _buttonBgImage.color = tempColor;
            _buttonIconImage.color = tempColor;
            _button.enabled = true;
            _buttonHoverImage.enabled = true;

            _feedbackText.enabled = false;
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
