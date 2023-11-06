using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    // Implement Swing Speed, Cooldown, and Power attributes for resource gathering
    public class ActionControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private SpriteRenderer _swingSr;
        [SerializeField] private AudioClip _wooshSound;
        [Header("Base Stats")]
        [SerializeField] private ItemParameter _baseDamage;
        [SerializeField] private ItemParameter _basePower;
        [SerializeField] private ItemParameter _baseCooldown;
        [SerializeField] private ItemParameter _baseSwingSpeedMultiplier;

        private TileAction _ta;
        private PlayerInput _input;
        private Animator _animator;
        private PlayerMoveInput _moveInput;
        private SwingCollider _swingCollider;
        private Camera _camera;
        private InventorySlot _selectedSlot;
        private float _counter;
        private bool _isHeldDown;
        private bool _performingSwing;
        private bool _canPerform = true;
        private readonly int _idleHash = Animator.StringToHash("[Anim] AC Idle");
        private readonly int _rightSwingHash = Animator.StringToHash("[Anim] AC Swing Right");
        private readonly int _upSwingHash = Animator.StringToHash("[Anim] AC Swing Up");
        private readonly int _leftSwingHash = Animator.StringToHash("[Anim] AC Swing Left");
        private readonly int _downSwingHash = Animator.StringToHash("[Anim] AC Swing Down");

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.performed += SetIsHeldDown;
            _input.Player.PrimaryAction.canceled += SetIsHeldDown;
            _input.Enable();

            _animator = GetComponent<Animator>();
            _animator.speed = 1 * _baseSwingSpeedMultiplier.Value;
            _moveInput = transform.root.GetComponent<PlayerMoveInput>();
            _ta = transform.GetChild(0).GetComponent<TileAction>();
            _ta.transform.parent = null;

            _swingCollider = transform.GetChild(0).GetChild(0).GetComponent<SwingCollider>();
            _swingCollider.BaseDamage = _baseDamage.Value;

            GameSignals.SELECTED_SLOT_UPDATED.AddListener(ProcessSelectedSlotUpdate);
            GameSignals.ITEM_ADDED.AddListener(UpdateHeldItem);
            GameSignals.DAY_END.AddListener(DisableActions);
            GameSignals.DAY_START.AddListener(EnableActions);
            GameSignals.PLAYER_DIED.AddListener(DisableActions);
            GameSignals.GAME_PAUSED.AddListener(DisableActions);
            GameSignals.GAME_UNPAUSED.AddListener(EnableActions);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.SELECTED_SLOT_UPDATED.RemoveListener(ProcessSelectedSlotUpdate);
            GameSignals.ITEM_ADDED.RemoveListener(UpdateHeldItem);
            GameSignals.DAY_END.RemoveListener(DisableActions);
            GameSignals.DAY_START.RemoveListener(EnableActions);
            GameSignals.PLAYER_DIED.RemoveListener(DisableActions);
            GameSignals.GAME_PAUSED.RemoveListener(DisableActions);
            GameSignals.GAME_UNPAUSED.RemoveListener(EnableActions);
        }

        private IEnumerator Start()
        {
            SwingEnd();

            yield return new WaitForEndOfFrame();

            _camera = Camera.main;
        }

        private void Update()
        {
            if (_selectedSlot == null) return;

            _counter += Time.deltaTime;

            if (_counter > CalcParameter(_baseCooldown))
                _counter = CalcParameter(_baseCooldown);

            if (_isHeldDown)
                Swing();
        }

        private void ProcessSelectedSlotUpdate(ISignalParameters parameters)
        {
            _selectedSlot = (InventorySlot)parameters.GetParameter("SelectedSlot");
            
            UpdateSwingSprite();
        }

        private void UpdateHeldItem(ISignalParameters parameters)
        {
            UpdateSwingSprite();
        }

        private void UpdateSwingSprite()
        {
            _swingSr.sprite = _selectedSlot.ItemObject != null ? _selectedSlot.ItemObject.UiDisplay : null;
            _swingCollider.SelectedSlot = _selectedSlot;
        }

        private void DisableActions(ISignalParameters parameters)
        {
            _ta.enabled = false;
            _canPerform = false;
        }

        private void EnableActions(ISignalParameters parameters)
        {
            _ta.enabled = true; 
            _canPerform = true;
        }

        private void Swing()
        {
            if (_counter < CalcParameter(_baseCooldown) || PointerHandler.IsOverLayer(5) || _performingSwing || !_canPerform) return;
            if (_selectedSlot.ItemObject == null) return;
            if(_selectedSlot.ItemObject.ToolType == ToolType.None) return;
            AnimStateManager.ChangeAnimationState(_animator, GetAnimationHash());
        }

        public void SwingStart()
        {
            _performingSwing = true;
            _moveInput.Speed = 1f;
            _animator.speed = 1 * CalcParameter(_baseSwingSpeedMultiplier);

            GameSignals.SWING_PERFORMED.Dispatch();
            AudioManager.Instance.PlayClip(_wooshSound, false, true, 0.75f);
        }

        private float CalcParameter(ItemParameter baseParameter)
        {
            var item = _selectedSlot.ItemObject;

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
            _moveInput.Speed = _moveInput.BaseSpeed;
            _counter = 0f;

            AnimStateManager.ChangeAnimationState(_animator, _idleHash);
        }

        public void HitAtStaIndicator()
        {
            _ta.HitTile();
        }

        private void SetIsHeldDown(InputAction.CallbackContext context)
        {
            _isHeldDown = context.performed;
        }

        private int GetAnimationHash()
        {
            var cursorAngle = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var playerVec = (Vector2)cursorAngle - _pr.Position;
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
