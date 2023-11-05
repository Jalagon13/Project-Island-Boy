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
        [field:SerializeField] public TilemapReferences TMR { get; private set; }
        [field:SerializeField] public Slider ThrowSlider { get; private set; }
        public TileAction TileAction { get; private set; }
        public InventorySlot SelectedSlot { get; private set; }
        [field:SerializeField] public ItemParameter ChargeTimeParameter { get; private set; }

        private Action<SelectedSlotControl, float> _onLaunch;
        private PlayerInput _input;
        private bool _isHeldDown, _isCharging;
        private float _baseCoolDown = 0.17f, _minThrowForce = 5f, _maxThrowForce = 25f, _maxChargeTime = 1.5f, _baseChargeTime = 1.5f;
        private float _counter, _chargeSpeed, _currentThrowForce;

        public bool IsCharging { get { return _isCharging; } set { _isCharging = true; } }
        public Action<SelectedSlotControl, float> OnLaunch { set { _onLaunch = value; } }
        public float ThrowForcePercentage 
        { 
            get 
            {
                float a = _maxThrowForce - _minThrowForce;
                float b = _currentThrowForce - _minThrowForce;

                return b / a; 
            }
        }

        private void Awake()
        {
            _input = new();

            _input.Player.PrimaryAction.started += SelectedSlotPrimaryAction;
            _input.Player.PrimaryAction.performed += IsHeldDown;
            _input.Player.PrimaryAction.canceled += IsHeldDown;

            _input.Player.SecondaryAction.started += SelectedSlotSecondaryAction;

            TileAction = FindObjectOfType<TileAction>();
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

            GameSignals.SELECTED_SLOT_UPDATED.AddListener(InjectSelectedSlot);
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(UpdateChargeTime);
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(ResetLaunch);
        }

        private void OnDisable()
        {
            _input.Disable();

            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(InjectSelectedSlot);
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(UpdateChargeTime);
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(ResetLaunch);
        }

        private void Update()
        {
            _counter += Time.deltaTime;

            if (_counter > _baseCoolDown)
                _counter = _baseCoolDown;

            if (_isHeldDown && !_isCharging)
                TryExecuteSlotPrimaryAction();

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

        private void InjectSelectedSlot(ISignalParameters parameters)
        {
            SelectedSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");
        }

        private void UpdateChargeTime(ISignalParameters parameters)
        {
            if (SelectedSlot.CurrentParameters.Count <= 0) return;
            var itemParams = SelectedSlot.CurrentParameters;

            if (itemParams.Contains(ChargeTimeParameter))
            {
                int index = itemParams.IndexOf(ChargeTimeParameter);
                _maxChargeTime = itemParams[index].Value;
                _chargeSpeed = (_maxThrowForce - _minThrowForce) / _maxChargeTime;
            }
            else
            {
                _maxChargeTime = _baseChargeTime;
            }
        }

        private void IsHeldDown(InputAction.CallbackContext context)
        {
            _isHeldDown = context.performed;

            if (!_isHeldDown && _isCharging)
            {
                _onLaunch?.Invoke(this, _currentThrowForce);

                _isCharging = false;
                ThrowSlider.gameObject.SetActive(false);
                _currentThrowForce = _minThrowForce;
            }
        }

        private void ResetLaunch(ISignalParameters parameters)
        {
            _isCharging = false;
            ThrowSlider.gameObject.SetActive(false);
            _currentThrowForce = _minThrowForce;
        }

        private void SelectedSlotPrimaryAction(InputAction.CallbackContext context)
        {
            TryExecuteSlotPrimaryAction();
        }

        private void SelectedSlotSecondaryAction(InputAction.CallbackContext context)
        {
            TryExecuteSecondaryAction();
        }

        private void TryExecuteSlotPrimaryAction()
        {
            if (SelectedSlot.ItemObject != null && _counter >= _baseCoolDown)
            {
                SelectedSlot.ItemObject.ExecutePrimaryAction(this);
                _counter = 0;
            }
        }

        private void TryExecuteSecondaryAction()
        {
            if (SelectedSlot.ItemObject != null && _counter >= _baseCoolDown)
            {
                SelectedSlot.ItemObject.ExecuteSecondaryAction(this);
                _counter = 0;
            }
        }
    }
}
