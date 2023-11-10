using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class TileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private float _maxDist;

        private PlayerInput _input;
        private SpriteRenderer _sr;
        private Slot _focusSlotRef;
        private bool _placingThisFrame;
        private Vector2 _previousCenterPos;
        private Vector2 _currentCenterPos;

        public Vector2 CurrentCenterPos { get { return _currentCenterPos; } }

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.started += Interact;
            _input.Enable();

            _sr = GetComponent<SpriteRenderer>();

            GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
        }

        private void OnDestroy()
        {
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);

            _input.Disable();
        }

        private void Update()
        {
            transform.SetPositionAndRotation(CalcPosition(), Quaternion.identity);

            CheckWhenEnterNewTile();
        }

        private void CheckWhenEnterNewTile()
        {
            _currentCenterPos = CalcCenterTile();

            if(_currentCenterPos != _previousCenterPos)
            {
                OnEnterNewTile();
                
                _previousCenterPos = _currentCenterPos;
            }
        }

        private void OnEnterNewTile()
        {
            _sr.enabled = GetBreakableObjects().Count <= 0;

            Signal signal = GameSignals.TILE_ACTION_ENTERED_NEW_TILE;
            signal.ClearParameters();
            signal.AddParameter("Breakables", GetBreakableObjects());
            signal.AddParameter("CurrentCenterPos", _currentCenterPos);
            signal.AddParameter("OverInteractable", OverInteractable());
            signal.Dispatch();
        }

        public List<IBreakable> GetBreakableObjects()
        {
            var colliders = Physics2D.OverlapCircleAll(_currentCenterPos, 0.2f);

            List<IBreakable> breakableObjects = new();

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IBreakable breakable))
                {
                    breakableObjects.Add(breakable);
                }
            }

            return breakableObjects;
        }

        private Vector2 CalcCenterTile()
        {
            int tileX = Mathf.FloorToInt(transform.position.x);
            int tileY = Mathf.FloorToInt(transform.position.y);

            return new Vector2(tileX + 0.5f, tileY + 0.5f);
        }

        private void FocusSlotUpdated(ISignalParameters parameters)
        {
            if (parameters.HasParameter("FocusSlot"))
            {
                _focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");
            }
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

        private void Interact(InputAction.CallbackContext context)
        {
            var colliders = Physics2D.OverlapCircleAll(_currentCenterPos, 0.2f);

            foreach (Collider2D col in colliders)
            {
                var results = col.GetComponents<Interactable>();

                foreach (Interactable interactable in results)
                {
                    interactable.Interact();
                }
            }
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

        public void HitTile()
        {
            if (_placingThisFrame) return;

            HammerTileLogic();
            ApplyDamageToBreakable();
        }

        private void HammerTileLogic()
        {
            if (_focusSlotRef.ItemObject == null) return;
            if (_focusSlotRef.ItemObject.ToolType == ToolType.None) return;
            if (_focusSlotRef.ItemObject != null)
                if (_focusSlotRef.ItemObject.ToolType != ToolType.Hammer) return;

            if (_tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                DestroyTile(_tmr.WallTilemap);
            else if (_tmr.FloorTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                DestroyTile(_tmr.FloorTilemap);
        }

        public void ApplyDamageToBreakable()
        {
            foreach (var breakable in GetBreakableObjects())
            {
                breakable.Hit(CalcToolType());
            }
        }

        private void DestroyTile(Tilemap tm)
        {
            var pos = Vector3Int.FloorToInt(transform.position);

            if (!tm.HasTile(pos)) return;

            RuleTileExtended tile = tm.GetTile<RuleTileExtended>(pos);
            GameAssets.Instance.SpawnItem(transform.position, tile.Item, 1);
            AudioManager.Instance.PlayClip(tile.BreakSound, false, true);

            tm.SetTile(pos, null);
            tile.UpdatePathfinding(new(pos.x + 0.5f, pos.y + 0.5f));
        }

        public void PlaceDeployable(GameObject deployable)
        {
            var position = transform.position -= new Vector3(0.5f, 0.5f);
            ExtensionMethods.SpawnObject(deployable, position, Quaternion.identity);
            _placingThisFrame = true;
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.2f);

            _placingThisFrame = false;
        }

        private ToolType CalcToolType()
        {
            var item = _focusSlotRef.ItemObject;
            return item == null ? ToolType.None : item.ToolType;
        }

        private Vector2 CalcPosition()
        {
            Vector2 taPosition;
            Vector2 playerPos = transform.root.transform.localPosition;
            Vector2 direction = (_pr.MousePosition - playerPos).normalized;

            taPosition = Vector2.Distance(playerPos, _pr.MousePosition) > _maxDist ? playerPos + (direction * _maxDist) : _pr.MousePosition;

            return taPosition;
        }

        private Vector2 GetCenterOfTilePos(Vector3 pos)
        {
            var xPos = Mathf.FloorToInt(pos.x) + 0.5f;
            var yPos = Mathf.FloorToInt(pos.y) + 0.5f;

            return new Vector2(xPos, yPos);
        }
    }
}
