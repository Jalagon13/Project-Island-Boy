using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class TileIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private Tilemap _floorTilemap;
        [SerializeField] private Tilemap _islandTilemap;
        [SerializeField] private Color _indicatorEmptyColor;
        [SerializeField] private Color _indicatorTransparentColor;
        [SerializeField] private Color _canHitColor;

        private TileHpCanvas _stHpCanvas;
        private TileAction _ta;
        private SpriteRenderer _sr;
        private AdventurerEntity _ae = null;

        public Tilemap WallTilemap { get { return _wallTilemap; } }
        public Tilemap FloorTilemap { get { return _floorTilemap; } }
        public Tilemap IslandTilemap { get { return _islandTilemap; } }

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _stHpCanvas = GetComponent<TileHpCanvas>();
            _ta = GetComponent<TileAction>();
        }

        private void OnEnable()
        {
            HotbarControl.OnSelectedSlotUpdated += UpdateLogic;
        }

        private void OnDisable()
        {
            HotbarControl.OnSelectedSlotUpdated -= UpdateLogic;
        }

        private void Update()
        {
            if (!transform.hasChanged) return;

            UpdateLogic();
        }

        public void UpdateLogic()
        {
            if (_ta.OverInteractable())
            {
                bool foundAdventurer = false;
                var colliders = Physics2D.OverlapCircleAll(transform.position, 0.4f);

                foreach (var col in colliders)
                {
                    if(col.TryGetComponent(out AdventurerEntity ae))
                    {
                        ChangeToOff();
                        _ae = ae;
                        _ae.ShowSelectIndicator();
                        foundAdventurer = true;
                        return;
                    }
                }

                if(!foundAdventurer)
                {
                    ChangeToOn();
                }
            }
            else
            {
                if(_ae != null)
                {
                    _ae.HideSelectIndicator();
                    _ae = null;
                }

                ChangeToOff();
                RscHarvest();
                ShovelTile();
                HammerTile();
                Indifferent();
            }

            transform.hasChanged = false;
        }

        private void HammerTile()
        {
            if (_pr.SelectedSlot.ItemObject != null)
                if (_pr.SelectedSlot.ItemObject.ToolType != ToolType.Hammer) return;
            if (_pr.SelectedSlot.ItemObject == null) return;

            if(_floorTilemap.HasTile(Vector3Int.FloorToInt(transform.position)) || _wallTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                ChangeToOn();
            else
                ChangeToOff();
        }

        private void Indifferent()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

            List<IBreakable> indifferentObjects = new();

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IBreakable breakable))
                {
                    if (breakable.BreakType == ToolType.Indifferent)
                        indifferentObjects.Add(breakable);
                }
            }

            if (indifferentObjects.Count > 0)
            {
                foreach (var breakable in indifferentObjects)
                {
                    ChangeToOn();

                    if (breakable.CurrentHitPoints < breakable.MaxHitPoints)
                    {
                        _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                    }

                    break;
                }
            }
        }

        private void ShovelTile()
        {
            if (_pr.SelectedSlot.ItemObject != null)
                if (_pr.SelectedSlot.ItemObject.ToolType != ToolType.Shovel) return;
            if (_pr.SelectedSlot.ItemObject == null) return;
            if (!_islandTilemap.HasTile(Vector3Int.FloorToInt(transform.position))) return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

            List<IBreakable> shovelObjects = new();

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IBreakable breakable))
                {
                    if(breakable.BreakType == ToolType.Shovel)
                        shovelObjects.Add(breakable);
                }
            }

            if(shovelObjects.Count > 0)
            {
                foreach (var breakable in shovelObjects)
                {
                    ChangeToOn();

                    if (breakable.CurrentHitPoints < breakable.MaxHitPoints)
                    {
                        _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                    }

                    break;
                }
            }
            else
            {
                if (_ta.IsClear())
                    ChangeToOn();
                else
                    ChangeToOff();
            }
        }

        private void RscHarvest()
        {
            if(_pr.SelectedSlot.ItemObject != null)
                if (_pr.SelectedSlot.ItemObject.ToolType == ToolType.Shovel) return;

            var colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f);

            List<IBreakable> breakableObjects = new();

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IBreakable breakable))
                {
                    breakableObjects.Add(breakable);
                }
            }

            if (breakableObjects.Count <= 0) 
                return;

            var item = _pr.SelectedSlot.ItemObject;

            ToolType selSlotTType = item == null ? _ta.BaseToolType : item.ToolType;

            foreach (var breakable in breakableObjects)
            {
                if(breakable.BreakType == selSlotTType)
                {
                    ChangeToOn();

                    if (breakable.CurrentHitPoints < breakable.MaxHitPoints)
                    {
                        _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
                    }

                    break;
                }
            }
        }

        public void ChangeToOn()
        {
            _sr.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            _sr.color = _canHitColor;
        }

        public void ChangeToOff()
        {
            _sr.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            if (_pr.SelectedSlot.ItemObject is DeployObject ||
                _pr.SelectedSlot.ItemObject is FloorObject ||
                _pr.SelectedSlot.ItemObject is WallObject)
                _sr.color = _indicatorTransparentColor;
            else
                _sr.color = _indicatorEmptyColor;


            _stHpCanvas.HideHpCanvas();
        }
    }
}
