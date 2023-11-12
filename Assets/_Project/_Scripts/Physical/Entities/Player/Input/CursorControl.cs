using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class CursorControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private float _baseClickDistance;
        [SerializeField] private ItemParameter _powerParameter;

        private PlayerInput _input;
        private SpriteRenderer _sr;
        private Sprite _defaultPointerSprite;
        private Slot _focusSlotRef;
        private Clickable _currentClickableThisFrame;
        private Vector2 _previousCenterPos;
        private Vector2 _currentCenterPos;
        private bool _placingThisFrame;
        private bool _canHit = true;
        private bool _shownDisplayThisFrame;

        public Vector2 CurrentCenterPos { get { return _currentCenterPos; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += Hit;
            _input.Player.SecondaryAction.started += Interact;
            _input.Enable();

            _sr = GetComponent<SpriteRenderer>();
            _defaultPointerSprite = _sr.sprite;

            GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
            GameSignals.DAY_END.AddListener(DisableAbilityToHit);
            GameSignals.DAY_START.AddListener(EnableAbilityToHit);
            GameSignals.PLAYER_DIED.AddListener(DisableAbilityToHit);
            GameSignals.GAME_PAUSED.AddListener(DisableAbilityToHit);
            GameSignals.GAME_UNPAUSED.AddListener(EnableAbilityToHit);
        }

        private void OnDestroy()
        {
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);
            GameSignals.DAY_END.AddListener(DisableAbilityToHit);
            GameSignals.DAY_START.AddListener(EnableAbilityToHit);
            GameSignals.PLAYER_DIED.AddListener(DisableAbilityToHit);
            GameSignals.GAME_PAUSED.AddListener(DisableAbilityToHit);
            GameSignals.GAME_UNPAUSED.AddListener(EnableAbilityToHit);

            _input.Disable();
        }

        private void Update()
        {
            transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);

            CheckWhenEnterNewTile();
            UpdateCurrentClickable();
            DisplayCursor();
        }

        private void CheckWhenEnterNewTile()
        {
            _currentCenterPos = CalcCenterTile();

            if (_currentCenterPos != _previousCenterPos)
            {
                OnEnterNewTile();

                _previousCenterPos = _currentCenterPos;
            }
        }

        private void OnEnterNewTile()
        {
            _currentClickableThisFrame = null;

            Signal signal = GameSignals.CURSOR_ENTERED_NEW_TILE;
            signal.ClearParameters();
            signal.AddParameter("CenterPosition", _currentCenterPos);
            signal.Dispatch();
        }

        private void DisplayCursor()
        {
            _sr.enabled = _currentClickableThisFrame == null;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

            foreach (Collider2D col in colliders)
            {
                var results = col.GetComponents<Interactable>();

                foreach (Interactable interactable in results)
                {
                    interactable.Interact();
                }
            }
        }

        private void FocusSlotUpdated(ISignalParameters parameters)
        {
            if (parameters.HasParameter("FocusSlot"))
            {
                _focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");

                if (_focusSlotRef.ItemObject != null)
                {
                    _sr.sprite = _focusSlotRef.ItemObject.ToolType != ToolType.None ? _focusSlotRef.ItemObject.UiDisplay : _defaultPointerSprite;
                }
                else
                {
                    _sr.sprite = _defaultPointerSprite;
                }
            }
        }

        private void Hit(InputAction.CallbackContext context)
        {
            if (HammerHitSomething() || PointerHandler.IsOverLayer(5)) return;

            if (_currentClickableThisFrame != null && _canHit)
            {
                _currentClickableThisFrame.OnClick(_focusSlotRef == null ? ToolType.None : _focusSlotRef.ToolType, CalcPower());
            }
        }

        private int CalcPower()
        {
            if (_focusSlotRef.ItemObject == null) return 0;

            if (_focusSlotRef.ItemObject.DefaultParameterList.Contains(_powerParameter))
            {
                var index = _focusSlotRef.ItemObject.DefaultParameterList.IndexOf(_powerParameter);
                var powerParameter = _focusSlotRef.ItemObject.DefaultParameterList[index];

                return (int)powerParameter.Value;
            }

            return 0;
        }

        private void UpdateCurrentClickable()
        {
            Clickable lastestClickable = ClickableFound();

            if (lastestClickable != null)
            {
                if (_currentClickableThisFrame == lastestClickable) return;

                _currentClickableThisFrame = lastestClickable;
                _shownDisplayThisFrame = false;
            }
            else
            {
                if (_currentClickableThisFrame == null) return;
                _currentClickableThisFrame = null;
            }
        }

        private Clickable ClickableFound()
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
            List<Clickable> clickablesFound = new();

            if (colliders.Count() > 0)
            {
                foreach (Collider2D c in colliders)
                {
                    if (c.TryGetComponent(out Clickable clickable))
                    {
                        clickablesFound.Add(clickable);
                    }
                }
            }

            return clickablesFound.Count > 0 ? clickablesFound.Last() : null;
        }

        private void DisableAbilityToHit(ISignalParameters parameters)
        {
            _canHit = false;
        }

        private void EnableAbilityToHit(ISignalParameters parameters)
        {
            _canHit = true;
        }

        private Vector2 CalcCenterTile()
        {
            int tileX = Mathf.FloorToInt(transform.position.x);
            int tileY = Mathf.FloorToInt(transform.position.y);

            return new Vector2(tileX + 0.5f, tileY + 0.5f);
        }

        public bool OverInteractable()
        {
            var colliders = Physics2D.OverlapCircleAll(_currentCenterPos, 0.2f);

            foreach (Collider2D col in colliders)
            {
                if (col.TryGetComponent(out Interactable interact))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsClear()
        {
            var colliders = Physics2D.OverlapBoxAll(CalcPosition(), new Vector2(0.5f, 0.5f), 0);

            foreach(Collider2D col in colliders)
            {
                if(col.gameObject.layer == 3) 
                    return false;
            }

            return true;
        }

        private bool HammerHitSomething()
        {
            if (_focusSlotRef.ItemObject == null) return false;
            if (_focusSlotRef.ItemObject.ToolType == ToolType.None) return false;
            if (_focusSlotRef.ItemObject != null)
                if (_focusSlotRef.ItemObject.ToolType != ToolType.Hammer) return false;

            if (_tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(_currentCenterPos)))
            {
                DestroyTile(_tmr.WallTilemap);
                return true;
            }
            else if (_tmr.FloorTilemap.HasTile(Vector3Int.FloorToInt(_currentCenterPos)))
            {
                DestroyTile(_tmr.FloorTilemap);
                return true;
            }

            return false;
        }

        private void DestroyTile(Tilemap tm)
        {
            var pos = Vector3Int.FloorToInt(transform.position);

            if (!tm.HasTile(pos)) return;

            RuleTileExtended tile = tm.GetTile<RuleTileExtended>(pos);
            GameAssets.Instance.SpawnItem(transform.position, tile.Item, 1);
            AudioManager.Instance.PlayClip(tile.BreakSound, false, true);
            GameSignals.CLICKABLE_CLICKED.Dispatch();

            tm.SetTile(pos, null);
            tile.UpdatePathfinding(new(pos.x + 0.5f, pos.y + 0.5f));
        }

        public void PlaceDeployable(GameObject deployable)
        {
            var position = _currentCenterPos -= new Vector2(0.5f, 0.5f);
            Instantiate(deployable, position, Quaternion.identity);
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            _placingThisFrame = true;
            yield return new WaitForSeconds(0.2f);
            _placingThisFrame = false;
        }

        private Vector2 CalcPosition()
        {
            Vector2 taPosition;
            Vector2 playerPos = transform.root.transform.localPosition;
            Vector2 direction = (_pr.MousePosition - playerPos).normalized;

            taPosition = Vector2.Distance(playerPos, _pr.MousePosition) > _baseClickDistance ? (playerPos += new Vector2(0, 0.25f)) + (direction * _baseClickDistance) : _pr.MousePosition;

            return taPosition;
        }
    }
}
