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
        private InventorySlot _selectedSlot;
        private Player _player;
        private TileAction _ta;
        private float _delayCooldown = 0.2f;
        private bool _isHeldDown;

        public InventorySlot SelectedSlot { get { return _selectedSlot; } }
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

            GameSignals.SELECTED_SLOT_UPDATED.AddListener(UpdateSelectedSlot);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(UpdateSelectedSlot);
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

        private void UpdateSelectedSlot(ISignalParameters parameters)
        {
            _selectedSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");
        }

        private void ExecutePrimaryAction(InputAction.CallbackContext context)
        {
            if(_delayTimer.RemainingSeconds <= 0 && _selectedSlot.ItemObject != null)
            {
                _selectedSlot.ItemObject.ExecutePrimaryAction(this);
                _delayTimer.RemainingSeconds = _delayCooldown;
            }
        }

        private void ExecuteSecondaryAction(InputAction.CallbackContext context)
        {
            if (_delayTimer.RemainingSeconds <= 0 && _selectedSlot.ItemObject != null)
            {
                _selectedSlot.ItemObject.ExecuteSecondaryAction(this);
                _delayTimer.RemainingSeconds = _delayCooldown;
            }
        }
    }
}
