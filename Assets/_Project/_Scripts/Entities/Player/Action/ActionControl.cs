using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
    // Implement Swing Speed, Cooldown, and Power attributes for resource gathering
    public class ActionControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private AudioClip _wooshSound;
        [Header("Base Stats")]
        [SerializeField] private ToolType _baseToolType;
        [SerializeField] private ItemParameter _basePower;
        [SerializeField] private ItemParameter _baseCooldown;
        [SerializeField] private ItemParameter _baseSwingSpeedMultiplier;

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
        private bool _performingSwing;

        public bool IsHeldDown { get { return _isHeldDown; } }

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += TryPerformSwing;
            _input.Player.PrimaryAction.performed += SetIsHeldDown;
            _input.Player.PrimaryAction.canceled += SetIsHeldDown;

            _animator = GetComponent<Animator>();
            _animator.speed = 1 * _baseSwingSpeedMultiplier.Value;
            _moveInput = transform.root.GetComponent<PlayerMoveInput>();
            _camera = Camera.main;

            _sta = transform.GetChild(0).GetComponent<SingleTileAction>();
            _sta.BasePower = _basePower.Value;
            _sta.BaseToolType = _baseToolType;
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
            SwingEnd();
        }

        private void FixedUpdate()
        {
            _counter += Time.deltaTime;

            if (_counter > CalcParameter(_baseCooldown))
                _counter = CalcParameter(_baseCooldown);

            if (_isHeldDown)
                PerformSwing();
        }

        private void TryPerformSwing(InputAction.CallbackContext context)
        {
            PerformSwing();
        }

        private void PerformSwing()
        {
            if (_counter < CalcParameter(_baseCooldown) || PointerHandler.IsOverLayer(5) || _performingSwing) return;

            AnimStateManager.ChangeAnimationState(_animator, GetAnimationHash());
        }

        public void SwingStart()
        {
            _performingSwing = true;
            _moveInput.Speed = 1.5f;
            _animator.speed = 1 * CalcParameter(_baseSwingSpeedMultiplier);

            AudioManager.Instance.PlayClip(_wooshSound, false, true);
        }

        private float CalcParameter(ItemParameter baseParameter)
        {
            var item = _pr.SelectedSlot.ItemObject;

            if (item == null)
                return baseParameter.Value;

            if (item.DefaultParameterList.Contains(baseParameter))
            {
                int index = item.DefaultParameterList.IndexOf(baseParameter);
                return item.DefaultParameterList[index].Value;
            }

            return baseParameter.Value;
        }

        public void SwingEnd()
        {
            _performingSwing = false;
            _moveInput.Speed = 3f;
            _counter = 0f;

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
