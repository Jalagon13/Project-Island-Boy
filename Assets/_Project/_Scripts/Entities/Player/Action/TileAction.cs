using UnityEngine;
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
        private ToolType _baseToolType;
        private float _basePower;
        private bool _brokeRscThisFrame; // used to stop any logic after damage has been applied to breakable

        public float BasePower { set { _basePower = value; } }
        public ToolType BaseToolType { get { return _baseToolType; } set { _baseToolType = value; } }

        private void Awake()
        {
            _stHpCanvas = GetComponent<TileHpCanvas>();
            _ti = GetComponent<TileIndicator>();
        }

        private void Update()
        {
            transform.position = CalcStaPos();
        }

        public bool IsClear()
        {
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0);

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
            ApplyDamageToBreakable();
            ShovelTileLogic();
        }

        private void HammerTileLogic()
        {
            if (_pr.SelectedSlot.ItemObject != null)
                if (_pr.SelectedSlot.ItemObject.ToolType != ToolType.Hammer) return;
            if (_pr.SelectedSlot.ItemObject == null) return;
            if (!IsClear()) return;

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

        private void ShovelTileLogic()
        {
            if (_pr.SelectedSlot.ItemObject != null)
                if (_pr.SelectedSlot.ItemObject.ToolType != ToolType.Shovel) return;
            if (_pr.SelectedSlot.ItemObject == null) return;
            if (!_ti.IslandTilemap.HasTile(Vector3Int.FloorToInt(transform.position))) return;
            if (!IsClear()) return;
            if (_brokeRscThisFrame) return;

            DestroyTile(_ti.IslandTilemap);
            ModifyDurability();

            _brokeRscThisFrame = false;

            _ti.UpdateLogic();
        }

        private void ApplyDamageToBreakable()
        {
            if (_pr.SelectedSlot.ItemObject != null)
                if (_pr.SelectedSlot.ItemObject.ToolType == ToolType.Sword) return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

            foreach (var collider in colliders)
            {
                IBreakable breakable = collider.GetComponent<IBreakable>();

                if (breakable != null)
                {
                    if (breakable.Hit(CalcPower(), CalcToolType()))
                    {
                        ModifyDurability();

                        if (breakable.CurrentHitPoints <= 0)
                        {
                            _brokeRscThisFrame = true;
                            _stHpCanvas.HideHpCanvas();
                            _ti.ChangeToOffIndicator();

                            _ti.UpdateLogic();
                        }
                        else
                        {
                            _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                        }
                    }
                }
            }
        }

        private void DestroyTile(Tilemap tm)
        {
            var pos = Vector3Int.FloorToInt(transform.position);

            if (!tm.HasTile(pos)) return;

            RuleTileExtended tile = tm.GetTile<RuleTileExtended>(pos);
            WorldItemManager.Instance.SpawnItem(transform.position, tile.Item, 1);
            AudioManager.Instance.PlayClip(tile.BreakSound, false, true);
            tm.SetTile(pos, null);
        }

        public void PlaceDeployable(GameObject deployable)
        {
            var position = transform.position -= new Vector3(0.5f, 0.5f);
            //Instantiate(deployable, position, Quaternion.identity);
            ExtensionMethods.SpawnObject(deployable, position, Quaternion.identity);
        }

        private void ModifyDurability()
        {
            if (_pr.SelectedSlot.CurrentParameters.Count <= 0) return;

            var itemParams = _pr.SelectedSlot.CurrentParameters;

            if (itemParams.Contains(_durabilityParameter))
            {
                int index = itemParams.IndexOf(_durabilityParameter);
                float newValue = itemParams[index].Value - 1;
                itemParams[index] = new ItemParameter
                {
                    Parameter = _durabilityParameter.Parameter,
                    Value = newValue
                };

                _pr.SelectedSlot.InventoryItem.UpdateDurabilityCounter();
            }
        }

        private float CalcPower()
        {
            if (_pr.SelectedSlot.CurrentParameters.Count <= 0) return _basePower;

            var itemParams = _pr.SelectedSlot.CurrentParameters;

            if (itemParams.Contains(_powerParameter))
            {
                int index = itemParams.IndexOf(_powerParameter);
                return itemParams[index].Value;
            }
            
            return _basePower;
        }

        private ToolType CalcToolType()
        {
            var item = _pr.SelectedSlot.ItemObject;
            return item == null ? _baseToolType : item.ToolType;
        }

        private Vector2 CalcStaPos()
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
