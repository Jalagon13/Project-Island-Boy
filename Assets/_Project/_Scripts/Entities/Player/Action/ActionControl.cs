using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class ActionControl : MonoBehaviour
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private AudioClip _wooshSound;

        private SingleTileAction _sta;
        private PlayerInput _input;
        private Animator _animator;
        private readonly int _idleHash = Animator.StringToHash("[Anim] AC Idle");
        private readonly int _rightSwingHash = Animator.StringToHash("[Anim] AC Swing Right");
        private float _counter;
        private bool _isHeldDown;

        public bool IsHeldDown { get { return _isHeldDown; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += TryPerformSwing;
            _input.Player.PrimaryAction.performed += SetIsHeldDown;
            _input.Player.PrimaryAction.canceled += SetIsHeldDown;

            _animator = GetComponent<Animator>();
            _sta = transform.GetChild(0).GetComponent<SingleTileAction>();
            _sta.transform.parent = null;
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Start()
        {
            PlayIdle();
        }

        private void FixedUpdate()
        {
            _counter += Time.deltaTime;

            if (_counter > _cooldown)
                _counter = _cooldown;

            if (_isHeldDown)
                PerformSwing();
        }

        private void TryPerformSwing(InputAction.CallbackContext context)
        {
            PerformSwing();
        }

        private void PerformSwing()
        {
            if (_counter < _cooldown) return;

            _counter = 0f;
            AudioManager.Instance.PlayClip(_wooshSound, false, true);
            AnimStateManager.ChangeAnimationState(_animator, _rightSwingHash);
        }

        public void PlayIdle()
        {
            AnimStateManager.ChangeAnimationState(_animator, _idleHash);
        }

        public void HitAtStaIndicator()
        {
            _sta.HitTile();
        }

        private void SetIsHeldDown(InputAction.CallbackContext context)
        {
            _isHeldDown = context.performed;
        }
    }
}
