using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    public class ActionControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private float _cooldown;
        [SerializeField] private AudioClip _wooshSound;

        private SingleTileAction _sta;
        private PlayerInput _input;
        private Animator _animator;
        private PlayerMoveInput _moveInput;
        private Camera _camera;
        private readonly int _idleHash = Animator.StringToHash("[Anim] AC Idle");
        private readonly int _rightSwingHash = Animator.StringToHash("[Anim] AC Swing Right");
        private readonly int _upSwingHash = Animator.StringToHash("[Anim] AC Swing Up");
        private readonly int _leftSwingHash = Animator.StringToHash("[Anim] AC Swing Left");
        private readonly int _downSwingHash = Animator.StringToHash("[Anim] AC Swing Down");
        private float _counter;
        private bool _isHeldDown;

        public bool IsHeldDown { get { return _isHeldDown; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += TryPerformSwing;
            _input.Player.PrimaryAction.performed += SetIsHeldDown;
            _input.Player.PrimaryAction.canceled += SetIsHeldDown;

            _camera = Camera.main;
            _animator = GetComponent<Animator>();
            _moveInput = transform.root.GetComponent<PlayerMoveInput>();
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
            if (_counter < _cooldown || PointerHandler.IsOverLayer(5)) return;

            _counter = 0f;
            _moveInput.Speed = 1.5f;

            AudioManager.Instance.PlayClip(_wooshSound, false, true);
            AnimStateManager.ChangeAnimationState(_animator, GetAnimationHash());
        }

        public void PlayIdle()
        {
            _moveInput.Speed = 3f;
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

        private int GetAnimationHash()
        {
            var cursorAngle = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var playerVec = (Vector2)cursorAngle - _pr.PositionReference;
            float angle = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle = Mathf.Abs(angle);
                float leftover = 180 - angle;
                angle = 180 + leftover;
            }

            if ((angle < 45 && angle > 0) || (angle < 359.999 && angle > 315))
            {
                return _rightSwingHash;
            }
            else if (angle < 135 && angle > 45)
            {
                return _upSwingHash;
            }
            else if (angle < 225 && angle > 135)
            {
                return _leftSwingHash;
            }
            else if (angle < 315 && angle > 225)
            {
                return _downSwingHash;
            }

            return 0;
            //Ctx.PR.IsFacingRight.SetBool((angle < 90 && angle > 0) || (angle < 360 && angle > 270));
        }
    }
}
