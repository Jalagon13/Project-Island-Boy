using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class PlayerDebug : MonoBehaviour
    {
        [SerializeField] private float _speedDebugValue; // L to toggle

        private PlayerMoveInput _moveInput;
        private PlayerInput _input;
        private bool _speedDebugOn;

        private void Awake()
        {
            _moveInput = GetComponent<PlayerMoveInput>();
            _input = new();
            _input.Player.SpeedDebug.started += ToggleSpeedDebug;
            _input.Enable();
        }

        private void OnDestroy()
        {
            _input.Disable();
        }

        private void ToggleSpeedDebug(InputAction.CallbackContext context)
        {
            _speedDebugOn = !_speedDebugOn;

            _moveInput.Speed = _speedDebugOn ? _speedDebugValue : _moveInput.BaseSpeed;
        }
    }
}
