using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class TileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private TileHpCanvas _stHpCanvas;
        private TileIndicator _ti;
        private PlayerInput _input;
        private InventorySlot _selectedSlot;
        private bool _placingThisFrame;

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.started += Interact;
            _stHpCanvas = GetComponent<TileHpCanvas>();
            _ti = GetComponent<TileIndicator>();

            GameSignals.SELECTED_SLOT_UPDATED.AddListener(InjectSelectedSlot);

            _input.Enable();
        }

        private void OnDestroy()
        {
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(InjectSelectedSlot);

            _input.Disable();
        }

        private void Update()
        {
            transform.SetPositionAndRotation(CenterOfTilePos(), Quaternion.identity);
        }

        private void InjectSelectedSlot(ISignalParameters parameters)
        {
            _selectedSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");
        }

        public bool OverInteractable()
        {
            var colliders = Physics2D.OverlapBoxAll(CenterOfTilePos(), new Vector2(0.5f, 0.5f), 0);

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
            var colliders = Physics2D.OverlapBoxAll(CenterOfTilePos(), new Vector2(0.5f, 0.5f), 0);

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
            var colliders = Physics2D.OverlapBoxAll(CenterOfTilePos(), new Vector2(0.5f, 0.5f), 0);

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
            if (_selectedSlot.ItemObject == null) return;
            if (_selectedSlot.ItemObject.ToolType == ToolType.None) return;
            if (_selectedSlot.ItemObject != null)
                if (_selectedSlot.ItemObject.ToolType != ToolType.Hammer) return;

            if (_tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                DestroyTile(_tmr.WallTilemap);
            else if (_tmr.FloorTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                DestroyTile(_tmr.FloorTilemap);
        }

        public void ApplyDamageToBreakable()
        {
            foreach (var breakable in GetBreakableObjects())
            {
                if (breakable.BreakWithAnyTool)
                {
                    TryHitBreakable(breakable);
                    break;
                }
                else if (_selectedSlot.ItemObject != null)
                {
                    if (breakable.BreakType == _selectedSlot.ItemObject.ToolType)
                    {
                        TryHitBreakable(breakable);
                    }
                }
            }
        }

        private void TryHitBreakable(IBreakable breakable)
        {
            if (breakable.Hit(CalcPower(), CalcToolType()))
            {
                if (breakable.CurrentHitPoints <= 0)
                {
                    _stHpCanvas.HideHpCanvas();
                    _ti.ChangeToOff();

                    _ti.UpdateLogic();
                }
                else
                {
                    _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                }
            }
        }

        private List<IBreakable> GetBreakableObjects()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

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

        //public void ModifyDurability()
        //{
        //    //if (_pr.SelectedSlot.ItemObject is not ToolObject) return;
        //    if (_selectedSlot.CurrentParameters.Count <= 0) return;

        //    var itemParams = _selectedSlot.CurrentParameters;

        //    if (itemParams.Contains(_durabilityParameter))
        //    {
        //        int index = itemParams.IndexOf(_durabilityParameter);
        //        float newValue = itemParams[index].Value - 1;
        //        itemParams[index] = new ItemParameter
        //        {
        //            Parameter = _durabilityParameter.Parameter,
        //            Value = newValue
        //        };

        //        _selectedSlot.InventoryItem.UpdateDurabilityCounter();
        //    }
        //}

        private float CalcPower()
        {
            if (_selectedSlot.CurrentParameters.Count <= 0) return 8.4f;

            var itemParams = _selectedSlot.CurrentParameters;

            if (itemParams.Contains(_powerParameter))
            {
                int index = itemParams.IndexOf(_powerParameter);
                return itemParams[index].Value;
            }
            
            return 8.4f;
        }

        private ToolType CalcToolType()
        {
            var item = _selectedSlot.ItemObject;
            return item == null ? ToolType.None : item.ToolType;
        }

        private Vector2 CenterOfTilePos()
        {
            var playerPosTileCenter = GetCenterOfTilePos(_pr.Position/* + new Vector2(0f, 0.4f)*/);
            var dir = (_pr.MousePosition - playerPosTileCenter).normalized;

            return GetCenterOfTilePos(playerPosTileCenter + dir);
        }

        private Vector2 GetCenterOfTilePos(Vector3 pos)
        {
            var xPos = Mathf.FloorToInt(pos.x) + 0.5f;
            var yPos = Mathf.FloorToInt(pos.y) + 0.5f;

            return new Vector2(xPos, yPos);
        }
    }
}
