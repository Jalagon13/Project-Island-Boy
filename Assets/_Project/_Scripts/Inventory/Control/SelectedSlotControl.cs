using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace IslandBoy
{
    public class SelectedSlotControl : MonoBehaviour
    {
        [field:SerializeField] public PlayerReference PR { get; private set; }
        [field:SerializeField] public HealthBar HealthBar { get; private set; }
        [field:SerializeField] public EnergyBar EnergyBar { get; private set; }
        [field:SerializeField] public TileAction SingleTileAction { get; private set; }
        [field:SerializeField] public Collider2D PlayerEntityCollider { get; private set; }
        [field:SerializeField] public Tilemap WallTilemap { get; private set; }
        [field:SerializeField] public Tilemap FloorTilemap { get; private set; }
        [field:SerializeField] public Tilemap IslandTilemap { get; private set; }
        [field:SerializeField] public Slider ThrowSlider { get; private set; }

        private Action<SelectedSlotControl, float> _onThrow;
        private PlayerInput _input;
        private bool _isHeldDown, _isCharging;
        private float _baseCoolDown = 0.17f, _minThrowForce = 5f, _maxThrowForce = 25f, _maxChargeTime = 1f;
        private float _counter, _chargeSpeed, _currentThrowForce;

        public bool HeldDown { get { return _isHeldDown; } }
        public bool IsCharging { get { return _isCharging; } set { _isCharging = true; } }
        public Action<SelectedSlotControl, float> OnThrow { set { _onThrow = value; } }

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.started += SelectedSlotAction;
            _input.Player.SecondaryAction.performed += IsHeldDown;
            _input.Player.SecondaryAction.canceled += IsHeldDown;
        }

        private void Start()
        {
            _chargeSpeed = (_maxThrowForce - _minThrowForce) / _maxChargeTime;
            _currentThrowForce = _minThrowForce;
            ThrowSlider.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            _counter += Time.deltaTime;

            if (_counter > _baseCoolDown)
                _counter = _baseCoolDown;

            if (_isHeldDown && !_isCharging)
                TryExecuteSlotAction();

            if (_isCharging)
            {
                _currentThrowForce += _chargeSpeed * Time.deltaTime;
                var maxMinDiff = _maxThrowForce - _minThrowForce;
                var maxCurrDiff = _maxThrowForce - _currentThrowForce;
                ThrowSlider.gameObject.SetActive(true);
                ThrowSlider.value = (maxMinDiff - maxCurrDiff) / maxMinDiff;

                if (_currentThrowForce >= _maxThrowForce)
                    _currentThrowForce = _maxThrowForce;
            }
        }

        private void IsHeldDown(InputAction.CallbackContext context)
        {
            _isHeldDown = context.performed;

            if (!_isHeldDown && _isCharging)
                Throw();
        }

        private void Throw()
        {
            _isCharging = false;
            ThrowSlider.gameObject.SetActive(false);
            _onThrow?.Invoke(this, _currentThrowForce);
            _currentThrowForce = _minThrowForce;
        }

        private void SelectedSlotAction(InputAction.CallbackContext context)
        {
            TryExecuteSlotAction();
        }

        private void TryExecuteSlotAction()
        {
            if (PR.SelectedSlot.ItemObject != null && _counter >= _baseCoolDown)
            {
                PR.SelectedSlot.ItemObject.ExecuteAction(this);
                _counter = 0;
            }
        }

        public void RestoreStat(ConsumeType cType, int value)
        {
            switch (cType)
            {
                case ConsumeType.Energy:
                    EnergyBar.AddTo(value);
                    break;
                case ConsumeType.Health:
                    HealthBar.AddTo(value);
                    break;
            }
        }
    }
}
