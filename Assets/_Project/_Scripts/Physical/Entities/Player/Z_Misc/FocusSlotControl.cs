using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class FocusSlotControl : MonoBehaviour
    {
        [SerializeField] private TilemapReference _wallTm;
        [SerializeField] private TilemapReference _floorTm;
        [SerializeField] private TilemapReference _groundTm;

        private Timer _primaryDelayCooldownTimer;
        private Timer _secondaryDelayCooldownTimer;
        private PlayerInput _input;
        private Slot _focusSlot;
        private Slot _hotbarSlot;
        private Slot _mouseSlot;
        private Player _player;
        private CursorControl _cursorControl;
        private float _primaryActionDelayCoolDown = 0.34f;
        private float _secondayActionDelayCoolDown = 0.15f;
        private bool _primaryHeldDown;
        private bool _secondaryHeldDown;
        private bool _mouseSlotHasitem;

        public Slot FocusSlot { get { return _focusSlot; } }
        public Player Player { get { return _player; } }
        public TilemapReference WallTm { get { return _wallTm; } }
        public TilemapReference FloorTm { get { return _floorTm; } }
        public TilemapReference GroundTm { get { return _groundTm; } }
        public CursorControl CursorControl { get { return _cursorControl; } }

        private void Awake()
        {
            _player = GetComponent<Player>();
            _cursorControl = transform.GetChild(2).GetComponent<CursorControl>();

            _input = new();
            _input.Player.PrimaryAction.started += ExecutePrimaryAction;
            _input.Player.PrimaryAction.performed += PrimaryHeldDown;
            _input.Player.PrimaryAction.canceled += PrimaryHeldDown;
            _input.Player.SecondaryAction.started += ExecuteSecondaryAction;
            _input.Player.SecondaryAction.performed += SecondaryHeldDown;
            _input.Player.SecondaryAction.canceled += SecondaryHeldDown;
            _input.Enable();

            _primaryDelayCooldownTimer = new(_primaryActionDelayCoolDown);
            _secondaryDelayCooldownTimer = new(_secondayActionDelayCoolDown);

            GameSignals.HOTBAR_SLOT_UPDATED.AddListener(UpdateFocusSlotToHotbarSlot);
            GameSignals.MOUSE_SLOT_HAS_ITEM.AddListener(UpdateFocusSlotToMouseSlot);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.AddListener(UpdateLocalMouseSlot);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.HOTBAR_SLOT_UPDATED.RemoveListener(UpdateFocusSlotToHotbarSlot);
            GameSignals.MOUSE_SLOT_HAS_ITEM.RemoveListener(UpdateFocusSlotToMouseSlot);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.RemoveListener(UpdateLocalMouseSlot);
        }

        private void Update()
        {
            _primaryDelayCooldownTimer.Tick(Time.deltaTime);
            _secondaryDelayCooldownTimer.Tick(Time.deltaTime);

            if (_primaryHeldDown)
                ExecutePrimaryAction(new());

            if (_secondaryHeldDown)
                ExecuteSecondaryAction(new());
        }

        private void PrimaryHeldDown(InputAction.CallbackContext context)
        {
            _primaryHeldDown = context.performed;
        }

        private void SecondaryHeldDown(InputAction.CallbackContext context)
        {
            _secondaryHeldDown = context.performed;
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
            if(_primaryDelayCooldownTimer.RemainingSeconds <= 0 && _focusSlot.ItemObject != null && !_secondaryHeldDown && !Pointer.IsOverUI())
            {
                _focusSlot.ItemObject.ExecutePrimaryAction(this);
                _primaryDelayCooldownTimer.RemainingSeconds = _primaryActionDelayCoolDown;
            }
        }

        private void ExecuteSecondaryAction(InputAction.CallbackContext context)
        {
            if (_secondaryDelayCooldownTimer.RemainingSeconds <= 0 && _focusSlot.ItemObject != null && !_primaryHeldDown && !Pointer.IsOverUI())
            {
                _focusSlot.ItemObject.ExecuteSecondaryAction(this);
                _secondaryDelayCooldownTimer.RemainingSeconds = _secondayActionDelayCoolDown;
            }
        }
    }
}