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
        [SerializeField] private GameObject _minerNpcPrefab;
        [SerializeField] private AudioClip _sonarSound;
        [SerializeField] private AudioClip _moveInSound;

        private bool _minerSpawned;
        private Button _button;
        private Image _buttonBgImage;
        private Image _buttonIconImage;
        private HousingHoverImage _buttonHoverImage;
        private TextMeshProUGUI _feedbackText;
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

        public void CheckHousing()
        {
            if (_minerSpawned)
                return;

            AudioManager.Instance.PlayClip(_sonarSound, false, false);

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
                    DisplayFeedback("No valid housing found around you. Make sure there are no empty spaces!", Color.yellow);
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
                DisplayFeedback("No valid housing found around you. Space is too small!", Color.yellow);
                return;
            }

            // find a random position of the clear floor tiles and spawn the NPC there
            Vector3Int randFloorSpace = freeFloorSpaces[Random.Range(0, freeFloorSpaces.Count)];
            Vector2 spawnNpcPosition = new(randFloorSpace.x + 0.5f, randFloorSpace.y + 0.5f);

            StartCoroutine(SpawnAdventurer(spawnNpcPosition));
        }

        private IEnumerator SpawnAdventurer(Vector2 spawnPos)
        {
            yield return new WaitForSeconds(1f);

            DisplayFeedback("Housing found! Miner has moved in!", Color.green);
            Instantiate(_minerNpcPrefab, spawnPos, Quaternion.identity);
            AudioManager.Instance.PlayClip(_moveInSound, false, false);
            _minerSpawned = true;

            _pr.LevelSystem.AddExperience(3000);
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
