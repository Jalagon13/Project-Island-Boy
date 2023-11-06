using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class SelectedSlotControl : MonoBehaviour
    {
        [SerializeField] private TilemapReferences _tmr;

        private Timer _delayTimer; // used for tiny delay between primary/seconday item execution to prevent them from being executed every frame
        private PlayerInput _input;
        private Slot _focusSlot;
        private Slot _hotbarSlot;
        private Slot _mouseSlot;
        private Player _player;
        private TileAction _ta;
        private float _delayCooldown = 0.2f;
        private bool _isHeldDown;
        private bool _mouseSlotHasitem;

        public Slot FocusSlot { get { return _focusSlot; } }
        public Player Player { get { return _player; } }
        public TilemapReferences TMR { get { return _tmr; } }
        public TileAction TileAction { get { return _ta; } }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _ta = transform.GetChild(2).GetChild(0).GetComponent<TileAction>();

            _input = new();
            _input.Player.PrimaryAction.started += ExecutePrimaryAction;
            _input.Player.PrimaryAction.performed += IsHeldDown;
            _input.Player.PrimaryAction.canceled += IsHeldDown;
            _input.Player.SecondaryAction.started += ExecuteSecondaryAction;
            _input.Enable();

            _delayTimer = new(_delayCooldown);

            GameSignals.SELECTED_SLOT_UPDATED.AddListener(UpdateFocusSlotToHotbarSlot);
            GameSignals.MOUSE_SLOT_HAS_ITEM.AddListener(UpdateFocusSlotToMouseSlot);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.AddListener(UpdateLocalMouseSlot);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(UpdateFocusSlotToHotbarSlot);
            GameSignals.MOUSE_SLOT_HAS_ITEM.RemoveListener(UpdateFocusSlotToMouseSlot);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.RemoveListener(UpdateLocalMouseSlot);
        }

        private void Update()
        {
            _delayTimer.Tick(Time.deltaTime);

            if (_isHeldDown)
            {
                ExecutePrimaryAction(new());
            }
        }

        private void IsHeldDown(InputAction.CallbackContext context)
        {
            _isHeldDown = context.performed;
        }

        private void UpdateFocusSlotToHotbarSlot(ISignalParameters parameters)
        {
            if (parameters.HasParameter("SelectedSlot"))
            {
                _hotbarSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");

                SetFocusSlot();
            }
        }

        private void UpdateFocusSlotToMouseSlot(ISignalParameters parameters)
        {
            if (parameters.HasParameter("MouseSlot"))
            {
                _mouseSlot = (Slot)parameters.GetParameter("MouseSlot");
                _mouseSlotHasitem = true;

                SetFocusSlot();
            }
        }

        private void UpdateLocalMouseSlot(ISignalParameters parameters)
        {
            _mouseSlotHasitem = false;

            SetFocusSlot();
        }

        private void SetFocusSlot()
        {
            _focusSlot = _mouseSlotHasitem ? _mouseSlot : _hotbarSlot;

            Signal signal = GameSignals.FOCUS_SLOT_UPDATED;
            signal.ClearParameters();
            signal.AddParameter("FocusSlot", _focusSlot);
            signal.Dispatch();
        }

        private void ExecutePrimaryAction(InputAction.CallbackContext context)
        {
            if(_delayTimer.RemainingSeconds <= 0 && _focusSlot.ItemObject != null)
            {
                _focusSlot.ItemObject.ExecutePrimaryAction(this);
                _delayTimer.RemainingSeconds = _delayCooldown;
            }
        }

        private void ExecuteSecondaryAction(InputAction.CallbackContext context)
        {
            if (_delayTimer.RemainingSeconds <= 0 && _focusSlot.ItemObject != null)
            {
                _focusSlot.ItemObject.ExecuteSecondaryAction(this);
                _delayTimer.RemainingSeconds = _delayCooldown;
            }
        }
    }
}
