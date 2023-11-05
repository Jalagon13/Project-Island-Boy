using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IslandBoy
{
    public class TileIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private TilemapReferences _tmr;
        [SerializeField] private Color _indicatorEmptyColor;
        [SerializeField] private Color _indicatorTransparentColor;
        [SerializeField] private Color _canHitColor;

        private TileHpCanvas _stHpCanvas;
        private TileAction _ta;
        private SpriteRenderer _sr;
        private AdventurerEntity _ae = null;
        private InventorySlot _selectedSlot;

        private void Awake()
        {
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _stHpCanvas = GetComponent<TileHpCanvas>();
            _ta = GetComponent<TileAction>();
        }

        private void OnEnable()
        {
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(OnSelectedSlotUpdated);
        }

        private void OnDisable()
        {
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(OnSelectedSlotUpdated);
        }

        private void Update()
        {
            if (!transform.hasChanged) return;

            UpdateLogic();
        }

        private void OnSelectedSlotUpdated(ISignalParameters parameters)
        {
            _selectedSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");

            UpdateLogic();
        }

        public void UpdateLogic()
        {
            // if mouse item has an item, override _selectedSlot with Mouse slot.

            // if mouse has no item, update selected slot again.
            if (_selectedSlot == null) return;

            ChangeToOff();

            if (_ta.OverInteractable())
            {
                ChangeToOn();
            }
            else
            {
                RscHarvest();
                HammerTile();
            }

            transform.hasChanged = false;
        }

        private void HammerTile()
        {
            if (_selectedSlot.ItemObject == null) return;
            if (_selectedSlot.ItemObject != null)
                if (_selectedSlot.ItemObject.ToolType != ToolType.Hammer) return;

            if(_tmr.FloorTilemap.HasTile(Vector3Int.FloorToInt(transform.position)) || _tmr.WallTilemap.HasTile(Vector3Int.FloorToInt(transform.position)))
                ChangeToOn();
            else
                ChangeToOff();
        }

        private void RscHarvest()
        {
            foreach (var breakable in GetBreakableObjects())
            {
                if (breakable.BreakWithAnyTool)
                {
                    DisplayBreakableStatus(breakable);
                }
                else if(_selectedSlot.ItemObject != null)
                {
                    if (breakable.BreakType == _selectedSlot.ItemObject.ToolType)
                    {
                        DisplayBreakableStatus(breakable);

                        break;
                    }
                }
            }
        }

        private void DisplayBreakableStatus(IBreakable breakable)
        {
            ChangeToOn();

            if (breakable.CurrentHitPoints < breakable.MaxHitPoints)
            {
                _stHpCanvas.ShowHpCanvas(breakable.MaxHitPoints, breakable.CurrentHitPoints);
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

        public void ChangeToOn()
        {
            _sr.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            _sr.color = _canHitColor;
        }

        public void ChangeToOff()
        {
            _sr.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

            if (_selectedSlot.ItemObject is DeployObject ||
                _selectedSlot.ItemObject is FloorObject ||
                _selectedSlot.ItemObject is WallObject)
                _sr.color = _indicatorTransparentColor;
            else
                _sr.color = _indicatorEmptyColor;


            _stHpCanvas.HideHpCanvas();
        }
    }
}
