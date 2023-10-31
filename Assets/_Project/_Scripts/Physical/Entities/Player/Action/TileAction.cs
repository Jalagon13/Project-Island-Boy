using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class TileAction : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private ItemParameter _powerParameter;
        [SerializeField] private ItemParameter _durabilityParameter;

        private TileHpCanvas _stHpCanvas;
        private TileIndicator _ti;
        private PlayerInput _input;
        private InventorySlot _selectedSlot;
        private ToolType _baseToolType;
        private float _basePower;
        private bool _brokeRscThisFrame; // used to stop any logic after damage has been applied to breakable

        public float BasePower { set { _basePower = value; } }
        public ToolType BaseToolType { get { return _baseToolType; } set { _baseToolType = value; } }

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.started += Interact;
            _stHpCanvas = GetComponent<TileHpCanvas>();
            _ti = GetComponent<TileIndicator>();
        }

        private void OnEnable()
        {
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(InjectSelectedSlot);
            GameSignals.OBJECT_PLACED.AddListener(PlaceDeployable);
            _input.Enable();
        }

        private void OnDisable()
        {
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(InjectSelectedSlot);
            GameSignals.OBJECT_PLACED.RemoveListener(PlaceDeployable);
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

                //if(col.TryGetComponent(out Interactable interact))
                //{
                //    interact.Interact();
                //}
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
            _brokeRscThisFrame = false;

            HammerTileLogic();
            DamageBreakable();
        }

        private void HammerTileLogic()
        {
            if (_selectedSlot.ItemObject != null)
                if (_selectedSlot.ItemObject.ToolType != ToolType.Hammer) return;
            if (_selectedSlot.ItemObject == null) return;
            //if (!IsClear()) return;

            if (_ti.WallTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
            {
                DestroyTile(_ti.WallTilemap);
                ModifyDurability();
            }
            else if (_ti.FloorTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
            {
                DestroyTile(_ti.FloorTilemap);
                ModifyDurability();
            }
        }

        private void DamageBreakable()
        {
            if (ApplyDamageToBreakable(transform.position))
            {
                ModifyDurability();
            }
        }


        public bool ApplyDamageToBreakable(Vector3 pos)
        {
            if (_selectedSlot.ItemObject != null)
                if (_selectedSlot.ItemObject.ToolType == ToolType.Sword) return false;

            var colliders = Physics2D.OverlapCircleAll(pos, 0.2f);

            foreach (var collider in colliders)
            {
                IBreakable breakable = collider.GetComponent<IBreakable>();

                if (breakable != null)
                {
                    if (breakable.Hit(CalcPower(), CalcToolType()))
                    {
                        if (breakable.CurrentHitPoints <= 0)
                        {
                            _brokeRscThisFrame = true;
                            _stHpCanvas.HideHpCanvas();
                            _ti.ChangeToOff();

                            _ti.UpdateLogic();
                        }
                        else
                        {
                            _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                        }

                        return true;
                    }
                }
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

            tm.SetTile(pos, null);
            tile.UpdatePathfinding(new(pos.x + 0.5f, pos.y + 0.5f));
        }

        public void PlaceDeployable(ISignalParameters parameters)
        {
            var deployable = (GameObject)parameters.GetParameter("ObjectPlaced");

            var position = transform.position -= new Vector3(0.5f, 0.5f);
            ExtensionMethods.SpawnObject(deployable, position, Quaternion.identity);
            
        }

        public void ModifyDurability()
        {
            //if (_pr.SelectedSlot.ItemObject is not ToolObject) return;
            if (_selectedSlot.CurrentParameters.Count <= 0) return;

            var itemParams = _selectedSlot.CurrentParameters;

            if (itemParams.Contains(_durabilityParameter))
            {
                int index = itemParams.IndexOf(_durabilityParameter);
                float newValue = itemParams[index].Value - 1;
                itemParams[index] = new ItemParameter
                {
                    Parameter = _durabilityParameter.Parameter,
                    Value = newValue
                };

                _selectedSlot.InventoryItem.UpdateDurabilityCounter();
            }
        }

        private float CalcPower()
        {
            if (_selectedSlot.CurrentParameters.Count <= 0) return _basePower;

            var itemParams = _selectedSlot.CurrentParameters;

            if (itemParams.Contains(_powerParameter))
            {
                int index = itemParams.IndexOf(_powerParameter);
                return itemParams[index].Value;
            }
            
            return _basePower;
        }

        private ToolType CalcToolType()
        {
            var item = _selectedSlot.ItemObject;
            return item == null ? _baseToolType : item.ToolType;
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
