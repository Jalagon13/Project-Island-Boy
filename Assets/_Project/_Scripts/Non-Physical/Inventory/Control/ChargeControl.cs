using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace IslandBoy
{
    public class ChargeControl : MonoBehaviour
    {
        [field:SerializeField] public PlayerReference PR { get; private set; }
        [field:SerializeField] public Slider ThrowSlider { get; private set; }
        public TileAction TileAction { get; private set; }
        public InventorySlot SelectedSlot { get; private set; }

        public ItemParameter ChargeTimeParameter;

        private Action<float, float> _onRelease;
        private PlayerInput _input;
        private bool _isHeldDown, _isCharging;
        private float _minLaunchForce = 5f, _maxLaunchForce = 25f, _maxChargeTime = 1.5f, _baseChargeTime = 1.5f;
        private float _chargeSpeed, _currentLaunchForce;

        public float LaunchForcePercentage 
        { 
            get 
            {
                float a = _maxLaunchForce - _minLaunchForce;
                float b = _currentLaunchForce - _minLaunchForce;

                return b / a; 
            }
        }

        private void Awake()
        {
            _input = new();
            _input.Player.SecondaryAction.performed += IsHeldDown;
            _input.Player.SecondaryAction.canceled += IsHeldDown;
            _input.Enable();

            TileAction = FindObjectOfType<TileAction>();

            GameSignals.SELECTED_SLOT_UPDATED.AddListener(InjectSelectedSlot);
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(UpdateChargeTime);
            GameSignals.SELECTED_SLOT_UPDATED.AddListener(ResetLaunch);
            GameSignals.ITEM_CHARGING.AddListener(StartOfCharge);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(InjectSelectedSlot);
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(UpdateChargeTime);
            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(ResetLaunch);
            GameSignals.ITEM_CHARGING.RemoveListener(StartOfCharge);
        }

        private void Start()
        {
            _chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
            _currentLaunchForce = _minLaunchForce;
            ThrowSlider.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isCharging)
            {
                _currentLaunchForce += _chargeSpeed * Time.deltaTime;
                var maxMinDiff = _maxLaunchForce - _minLaunchForce;
                var maxCurrDiff = _maxLaunchForce - _currentLaunchForce;
                ThrowSlider.gameObject.SetActive(true);
                ThrowSlider.value = (maxMinDiff - maxCurrDiff) / maxMinDiff;

                if (_currentLaunchForce >= _maxLaunchForce)
                    _currentLaunchForce = _maxLaunchForce;
            }
        }

        private void StartOfCharge(ISignalParameters parameters)
        {
            if (parameters.HasParameter("ReleaseBehavior"))
            {
                _onRelease = (Action<float, float>)parameters.GetParameter("ReleaseBehavior");
                _isCharging = true;
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
                _chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
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
                _onRelease?.Invoke(LaunchForcePercentage, _currentLaunchForce);

                _isCharging = false;
                ThrowSlider.gameObject.SetActive(false);
                _currentLaunchForce = _minLaunchForce;
            }
        }

        private void ResetLaunch(ISignalParameters parameters)
        {
            _isCharging = false;
            ThrowSlider.gameObject.SetActive(false);
            _currentLaunchForce = _minLaunchForce;
        }
    }
}
