using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IslandBoy
{
    public class HotbarControl : MonoBehaviour
    {
        [SerializeField] private Color _highlightedColor;
        [SerializeField] private Color _notHighlightedColor;
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
        }

        private void OnDestroy()
        {
            GameSignals.GAME_PAUSED.RemoveListener(PauseHandle);
            GameSignals.GAME_UNPAUSED.RemoveListener(UnpauseHandle);
            GameSignals.PLAYER_DIED.RemoveListener(PauseHandle); 
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
            _slotIndex = 0;
            HighlightSelected();
        }

        private void PauseHandle(ISignalParameters parameters)
        {
            _input.Disable();
        }

        private void UnpauseHandle(ISignalParameters parameters)
        {
            _input.Enable();
        }

        private void DecreaseSelectedSlot(ISignalParameters parameters)
        {
            _selectedSlot.InventoryItem.Count--;
        }

        private void SelectSlotScroll(InputAction.CallbackContext context)
        {
            float scrollNum = context.ReadValue<float>();

            UnHighlightPrevious();

            if (scrollNum < 0)
            {
                _slotIndex++;
                if (_slotIndex > _hotbarSlots.Length - 1)
                    _slotIndex = 0;
            }
            else if(scrollNum > 0)
            {
                _slotIndex--;
                if(_slotIndex < 0)
                    _slotIndex = _hotbarSlots.Length - 1;
            }

            HighlightSelected();
        }

        private void SelectSlot(InputAction.CallbackContext context)
        {
            _slotIndex = Int32.Parse(context.action.name) - 1;

            if (_selectedSlot != _hotbarSlots[_slotIndex])
            {
                UnHighlightPrevious();
            }
            
            HighlightSelected();
        }

        private void HighlightSelected()
        {
            _selectedSlot = _hotbarSlots[_slotIndex];
            var image = _selectedSlot.GetComponent<Image>();
            image.color = _highlightedColor;

            DispatchSelectedSlotUpdated();
        }

        private void DispatchSelectedSlotUpdated()
        {
            Signal signal = GameSignals.SELECTED_SLOT_UPDATED;
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
