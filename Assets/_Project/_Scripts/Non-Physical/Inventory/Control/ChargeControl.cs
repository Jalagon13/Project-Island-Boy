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
        public CursorControl TileAction { get; private set; }
        public Slot FocusSlot { get; private set; }

        public ItemParameter ChargeTimeParameter;

        private Action<float> _onRelease;
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

            TileAction = FindObjectOfType<CursorControl>();

            GameSignals.FOCUS_SLOT_UPDATED.AddListener(UpdateFocusSlot);
            GameSignals.FOCUS_SLOT_UPDATED.AddListener(UpdateChargeTime);
            GameSignals.FOCUS_SLOT_UPDATED.AddListener(ResetLaunch);
            GameSignals.ITEM_CHARGING.AddListener(StartOfCharge);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(UpdateFocusSlot);
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(UpdateChargeTime);
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(ResetLaunch);
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
            if (_isCharging && _isHeldDown)
            {
                _currentLaunchForce += _chargeSpeed * Time.deltaTime;
                var maxMinDiff = _maxLaunchForce - _minLaunchForce;
                var maxCurrDiff = _maxLaunchForce - _currentLaunchForce;
                ThrowSlider.gameObject.SetActive(true);
                ThrowSlider.value = (maxMinDiff - maxCurrDiff) / maxMinDiff;

                if (_currentLaunchForce >= _maxLaunchForce)
                {
                    Release();
                }
            }
            else
            {
                ResetLaunch();
            }
        }

        private void StartOfCharge(ISignalParameters parameters)
        {
            if (parameters.HasParameter("ReleaseBehavior"))
            {
                _onRelease = (Action<float>)parameters.GetParameter("ReleaseBehavior");
                _isCharging = true;
            }
        }

        private void UpdateFocusSlot(ISignalParameters parameters)
        {
            if (parameters.HasParameter("FocusSlot"))
            {
                FocusSlot = (Slot)parameters.GetParameter("FocusSlot");
            }
        }

        private void UpdateChargeTime(ISignalParameters parameters)
        {
            if (FocusSlot == null) return;
            if (FocusSlot.CurrentParameters.Count <= 0) return;
            var itemParams = FocusSlot.CurrentParameters;

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
        }

        private void Release()
        {
            _onRelease?.Invoke(LaunchForcePercentage);

            _isCharging = false;
            ThrowSlider.gameObject.SetActive(false);
            _currentLaunchForce = _minLaunchForce;
        }

        private void ResetLaunch(ISignalParameters parameters = null)
        {
            _isCharging = false;
            ThrowSlider.gameObject.SetActive(false);
            _currentLaunchForce = _minLaunchForce;
        }
    }
}
