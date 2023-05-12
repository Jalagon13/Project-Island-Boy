using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class ActionControl : MonoBehaviour
    {
        private readonly int _idleHash = Animator.StringToHash("[Anim] AC Idle");
        private readonly int _rightSwingHash = Animator.StringToHash("[Anim] AC Swing Right");
        private PlayerInput _input;
        private Animator _animator;
        private bool _isHeldDown;

        public bool IsHeldDown { get { return _isHeldDown; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += Test;
            _input.Player.PrimaryAction.performed += SetIsHeldDown;
            _input.Player.PrimaryAction.canceled += SetIsHeldDown;

            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            PlayIdle();
        }

        private void Test(InputAction.CallbackContext context)
        {
            AnimStateManager.ChangeAnimationState(_animator, _rightSwingHash);
        }

        public void PlayIdle()
        {
            AnimStateManager.ChangeAnimationState(_animator, _idleHash);
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
