using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IslandBoy
{
	public class SwingControl : MonoBehaviour
	{
		[SerializeField] private float _swingCd;
		[SerializeField] private AudioClip _wooshClip;

		private SpriteRenderer _swingSr;
		private Timer _swingTimer;
		private Animator _animator;
		private Camera _camera;
		private Slot _focusSlotRef;

		private readonly int _hashRightHit = Animator.StringToHash("[ANM] RightSwing");
		private readonly int _hashUpHit = Animator.StringToHash("[ANM] UpSwing");
		private readonly int _hashLeftHit = Animator.StringToHash("[ANM] LeftSwing");
		private readonly int _hashDownHit = Animator.StringToHash("[ANM] DownSwing");
		private readonly int _hashIdle = Animator.StringToHash("[ANM] Idle");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_swingSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
			_swingTimer = new(_swingCd);

			GameSignals.FOCUS_SLOT_UPDATED.AddListener(OnUpdateFocusSlot);
			GameSignals.ITEM_DEPLOYED.AddListener(RefreshCd);
		}

		private void OnDestroy()
		{
			GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(OnUpdateFocusSlot);
			GameSignals.ITEM_DEPLOYED.RemoveListener(RefreshCd);
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			_camera = Camera.main;
		}

		private void FixedUpdate()
		{
			_swingTimer.Tick(Time.deltaTime);
		}

		private void RefreshCd(ISignalParameters parameters)
		{
			_swingTimer.RemainingSeconds = _swingCd;
		}

		private void OnUpdateFocusSlot(ISignalParameters parameters)
		{
			if (parameters.HasParameter("FocusSlot"))
			{
				_focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");
				UpdateSwingSprite();
			}
		}

		private void UpdateSwingSprite()
		{
			if (_focusSlotRef == null)
			{
				_swingSr.sprite = null;
				return;
			}
			_swingSr.sprite = _focusSlotRef.ItemObject == null ? null : _focusSlotRef.ItemObject is ToolObject ? _focusSlotRef.ItemObject.UiDisplay : null;
		}

		public void PerformAnimation()
		{
			if (_swingTimer.RemainingSeconds > 0 || _focusSlotRef == null || _focusSlotRef.ItemObject is not ToolObject || Pointer.IsOverUI()) return;
				
			PerformCorrectAnimation();
		}

		public void OnSwingEnd()
		{
			_swingTimer.RemainingSeconds = _swingCd;
			AnimStateManager.ChangeAnimationState(_animator, _hashIdle);
		}

		private void PerformCorrectAnimation()
		{
			var cursorAngle = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			var playerVec = cursorAngle - transform.root.position;
			float angle = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;

			if (angle < 0)
			{
				angle = Mathf.Abs(angle);
				float leftover = 180 - angle;
				angle = 180 + leftover;
			}

			if ((angle < 45 && angle > 0) || (angle < 359.999 && angle > 315))
			{
				AnimStateManager.ChangeAnimationState(_animator, _hashRightHit);
			}
			else if (angle < 135 && angle > 45)
			{
				AnimStateManager.ChangeAnimationState(_animator, _hashUpHit);
			}
			else if (angle < 225 && angle > 135)
			{
				AnimStateManager.ChangeAnimationState(_animator, _hashLeftHit);
			}
			else if (angle < 315 && angle > 225)
			{
				AnimStateManager.ChangeAnimationState(_animator, _hashDownHit);
			}
		}
	}
}
