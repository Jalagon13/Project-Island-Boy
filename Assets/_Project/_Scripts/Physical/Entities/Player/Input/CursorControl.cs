using System;
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
        [SerializeField] private float _startingClickDistance;
        [SerializeField] private float _clickCd = 0.1f;
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _clickDistanceParameter;

        private PlayerInput _input;
        private SpriteRenderer _sr;
        private Slot _focusSlotRef;
        private Clickable _currentClickable;
        private Timer _clickTimer;
        private Vector2 _previousCenterPos;
        private Vector2 _currentCenterPos;
        private bool _canHit = true;
        private bool _heldDown;
        private float _currentClickDistance;

        // temp buff stuff will rework/refactor later
        [SerializeField] private TextMeshProUGUI _buffText;
        [SerializeField] private float _buffDuration;
        private Timer _buffTimer;
        private int _buffAmount;

        public Clickable CurrentClickable { get { return _currentClickable; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += Hit;
            _input.Player.PrimaryAction.performed += Hold;
            _input.Player.PrimaryAction.canceled += Hold;
            _input.Player.SecondaryAction.started += Interact;
            _input.Enable();

            _clickTimer = new(_clickCd);
            _buffTimer = new(0);
            _buffText.enabled = false;

            _sr = GetComponent<SpriteRenderer>();
            _currentClickDistance = _startingClickDistance;

            GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
            GameSignals.DAY_END.AddListener(DisableAbilityToHit);
            GameSignals.DAY_START.AddListener(EnableAbilityToHit);
            GameSignals.PLAYER_DIED.AddListener(DisableAbilityToHit);
            GameSignals.GAME_PAUSED.AddListener(DisableAbilityToHit);
            GameSignals.GAME_UNPAUSED.AddListener(EnableAbilityToHit);
            GameSignals.ENTITY_SLAIN.AddListener(AddPlusTwoHitBuff);
        }

        private void OnDestroy()
        {
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);
            GameSignals.DAY_END.RemoveListener(DisableAbilityToHit);
            GameSignals.DAY_START.RemoveListener(EnableAbilityToHit);
            GameSignals.PLAYER_DIED.RemoveListener(DisableAbilityToHit);
            GameSignals.GAME_PAUSED.RemoveListener(DisableAbilityToHit);
            GameSignals.GAME_UNPAUSED.RemoveListener(EnableAbilityToHit);
            GameSignals.ENTITY_SLAIN.RemoveListener(AddPlusTwoHitBuff);

            _input.Disable();
        }

        private void FixedUpdate()
        {
            transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);

            _clickTimer.Tick(Time.deltaTime);


            //_buffTimer.Tick(Time.deltaTime);
            //_buffText.enabled = _buffTimer.RemainingSeconds > 0;
            //if (_buffTimer.RemainingSeconds > 0)
            //    _buffText.text = $"+2 Hit Buff: {Math.Round(_buffTimer.RemainingSeconds, 1)} sec";

            if (_heldDown)
                Hit(new());

            CheckWhenEnterNewTile();
            UpdateCurrentClickable();
            DisplayCursor();
        }

        private void AddPlusTwoHitBuff(ISignalParameters parameters)
        {
            _buffAmount = 0;
            _buffTimer.RemainingSeconds += _buffDuration;
            _buffTimer.OnTimerEnd += RemovePlusTwoHitBuff;
        }

        private void RemovePlusTwoHitBuff()
        {
            _buffAmount = 0;
            _buffTimer.OnTimerEnd -= RemovePlusTwoHitBuff;
        }

        private void Hold(InputAction.CallbackContext context)
        {
            _heldDown = context.performed;
        }

        private void Hit(InputAction.CallbackContext context)
        {
            if (HammerHitSomething() || PointerHandler.IsOverLayer(5) ||
                _focusSlotRef.ItemObject is not ToolObject || _clickTimer.RemainingSeconds > 0) return;


            if (_currentClickable != null && _canHit && _currentClickable is not Entity)
            {
                ToolType toolType = _focusSlotRef == null ? ToolType.None : _focusSlotRef.ToolType;
                int totalHit = CalcToolHitAmount() + CalcBuffModifiers();

                _currentClickable.OnHit(toolType, totalHit);
                _clickTimer.RemainingSeconds = _clickCd;
            }
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
            _currentClickable = null;

            Signal signal = GameSignals.CURSOR_ENTERED_NEW_TILE;
            signal.ClearParameters();
            signal.AddParameter("CenterPosition", _currentCenterPos);
            signal.Dispatch();
        }

        private void DisplayCursor()
        {
            _sr.enabled = _currentClickable == null;
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
                _currentClickDistance = CalcClickDistance();
            }
        }

        private int CalcToolHitAmount()
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

        private int CalcBuffModifiers()
        {
            return _buffAmount;
        }

        private float CalcClickDistance()
        {
            if (_focusSlotRef.ItemObject == null) return _startingClickDistance;

            if (_focusSlotRef.ItemObject.DefaultParameterList.Contains(_clickDistanceParameter))
            {
                var index = _focusSlotRef.ItemObject.DefaultParameterList.IndexOf(_clickDistanceParameter);
                var clickDistanceParameter = _focusSlotRef.ItemObject.DefaultParameterList[index];

                return clickDistanceParameter.Value;
            }

            return _startingClickDistance;
        }

        private void UpdateCurrentClickable()
        {
            Clickable lastestClickable = ClickableFound();

            if (lastestClickable != null)
            {
                if (_currentClickable == lastestClickable) return;

                _currentClickable = lastestClickable;
            }
            else
            {
                if (_currentClickable == null) return;
                _currentClickable = null;
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

            return clickablesFound.Count > 0 ? clickablesFound.Last() is Entity ? null : clickablesFound.Last() : null;
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
            if (PointerHandler.IsOverLayer(5)) return false;

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
        }

        private Vector2 CalcPosition()
        {
            Vector2 taPosition;
            Vector2 playerPos = transform.root.transform.localPosition;
            Vector2 direction = (_pr.MousePosition - playerPos).normalized;

            taPosition = Vector2.Distance(playerPos, _pr.MousePosition) > _currentClickDistance ? (playerPos += new Vector2(0, 0.25f)) + (direction * _currentClickDistance) : _pr.MousePosition;

            return taPosition;
        }
    }
}
