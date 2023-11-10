using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace IslandBoy
{
    public class ActionControl : MonoBehaviour
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private SpriteRenderer _swingSr;
        [SerializeField] private AudioClip _wooshSound;

        private TileAction _ta;
        private PlayerInput _input;
        private Animator _animator;
        private Slot _focusSlotRef;
        private bool _canPerform = true;

        private void Awake()
        {
            _input = new();
            _input.Player.PrimaryAction.started += Hit;
            _input.Enable();

            _animator = GetComponent<Animator>();
            _ta = transform.GetChild(0).GetComponent<TileAction>();

            GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotUpdated);
            GameSignals.DAY_END.AddListener(DisableActions);
            GameSignals.DAY_START.AddListener(EnableActions);
            GameSignals.PLAYER_DIED.AddListener(DisableActions);
            GameSignals.GAME_PAUSED.AddListener(DisableActions);
            GameSignals.GAME_UNPAUSED.AddListener(EnableActions);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.AddListener(DontSwingThisFrame);
        }

        private void OnDestroy()
        {
            _input.Disable();

            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotUpdated);
            GameSignals.DAY_END.RemoveListener(DisableActions);
            GameSignals.DAY_START.RemoveListener(EnableActions);
            GameSignals.PLAYER_DIED.RemoveListener(DisableActions);
            GameSignals.GAME_PAUSED.RemoveListener(DisableActions);
            GameSignals.GAME_UNPAUSED.RemoveListener(EnableActions);
            GameSignals.MOUSE_SLOT_GIVES_ITEM.RemoveListener(DontSwingThisFrame);
        }

        private void Hit(InputAction.CallbackContext context)
        {
            if (!_canPerform) return;

            _ta.HitTile();
        }

        private void DontSwingThisFrame(ISignalParameters parameters)
        {
            StartCoroutine(FrameDelay());
        }

        private IEnumerator FrameDelay()
        {
            _canPerform = false;
            yield return new WaitForSeconds(0.2f);
            _canPerform = true;
        }

        private void FocusSlotUpdated(ISignalParameters parameters)
        {
            if (parameters.HasParameter("FocusSlot"))
            {
                _focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");
            }
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
    }
}
