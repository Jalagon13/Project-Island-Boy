using System;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IslandBoy
{
	public class HotbarControl : MonoBehaviour
	{
		[SerializeField] private Color _highlightedColor;
		[SerializeField] private Color _notHighlightedColor;
		[SerializeField] private MMF_Player _hotbarUpdateFeedback;
		[SerializeField] private InventorySlot[] _hotbarSlots;

		private PlayerInput _input;
		private InventorySlot _selectedSlot;
		private InventorySlot _previousSlot;
		private int _slotIndex;

		private void Awake()
		{

			_input = new();
			_input.Hotbar.Scroll.performed += SelectSlotScroll;
			_input.Hotbar._1.started += SelectSlot;
			_input.Hotbar._2.started += SelectSlot;
			_input.Hotbar._3.started += SelectSlot;
			_input.Hotbar._4.started += SelectSlot;
			_input.Hotbar._5.started += SelectSlot;
			_input.Hotbar._6.started += SelectSlot;
			_input.Hotbar._7.started += SelectSlot;
			_input.Hotbar._8.started += SelectSlot;
			_input.Hotbar._9.started += SelectSlot;

			GameSignals.GAME_PAUSED.AddListener(PauseHandle);
			GameSignals.GAME_UNPAUSED.AddListener(UnpauseHandle);
			GameSignals.PLAYER_DIED.AddListener(PauseHandle);
			GameSignals.DAY_START.AddListener(UnpauseHandle);
			GameSignals.PLAYER_RESPAWN.AddListener(UnpauseHandle);
			GameSignals.ITEM_ADDED.AddListener(HighlightSelected);
		}

		private void OnDestroy()
		{
			GameSignals.GAME_PAUSED.RemoveListener(PauseHandle);
			GameSignals.GAME_UNPAUSED.RemoveListener(UnpauseHandle);
			GameSignals.PLAYER_DIED.RemoveListener(PauseHandle); 
			GameSignals.DAY_START.RemoveListener(UnpauseHandle);
			GameSignals.PLAYER_RESPAWN.RemoveListener(UnpauseHandle);
			GameSignals.ITEM_ADDED.RemoveListener(HighlightSelected);
		}

		private void OnEnable()
		{
			_input.Enable();
		}

		private void OnDisable()
		{
			_input.Disable();   
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();

			_slotIndex = 0;
			HighlightSelected(null);
		}

		private void PauseHandle(ISignalParameters parameters)
		{
			_input.Disable();
		}

		private void UnpauseHandle(ISignalParameters parameters)
		{
			_input.Enable();
		}

		private void SelectSlotScroll(InputAction.CallbackContext context)
		{
			if(Pointer.IsOverUI()) return;
			
			
			float scrollNum = context.ReadValue<float>();

			UnHighlightPrevious();

			if (scrollNum < 0)
			{
				_hotbarUpdateFeedback?.PlayFeedbacks();
				_slotIndex++;
				if (_slotIndex > _hotbarSlots.Length - 1)
					_slotIndex = 0;
			}
			else if(scrollNum > 0)
			{
				_hotbarUpdateFeedback?.PlayFeedbacks();
				_slotIndex--;
				if(_slotIndex < 0)
					_slotIndex = _hotbarSlots.Length - 1;
			}

			HighlightSelected(null);
		}

		private void SelectSlot(InputAction.CallbackContext context)
		{
			_hotbarUpdateFeedback?.PlayFeedbacks();
			_slotIndex = Int32.Parse(context.action.name) - 1;

			if (_selectedSlot != _hotbarSlots[_slotIndex])
			{
				UnHighlightPrevious();
			}
			
			HighlightSelected(null);
		}

		private void HighlightSelected(ISignalParameters parameters)
		{
			_selectedSlot = _hotbarSlots[_slotIndex];
			var image = _selectedSlot.GetComponent<Image>();
			image.color = _highlightedColor;
			
			DispatchSelectedSlotUpdated();
		}

		private void DispatchSelectedSlotUpdated()
		{
			Signal signal = GameSignals.HOTBAR_SLOT_UPDATED;
			signal.ClearParameters();
			signal.AddParameter("SelectedSlot", _selectedSlot);
			signal.Dispatch();
		}

		private void UnHighlightPrevious()
		{
			_previousSlot = _selectedSlot;

			if (_previousSlot == null) return;
			
			var image = _previousSlot.GetComponent<Image>();
			image.color = _notHighlightedColor;
		}
	}
}
