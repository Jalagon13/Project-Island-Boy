using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class StaInput : MonoBehaviour
    {
        private PlayerInput _input;
        private bool _isHeldDown;

        public bool IsHeldDown { get { return _isHeldDown; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.performed += SetIsHeldDown;
            _input.Player.PrimaryAction.canceled += SetIsHeldDown;
        }

        private void SetIsHeldDown(InputAction.CallbackContext context)
        {
            _isHeldDown = context.performed;
        }
        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }
    }
}
