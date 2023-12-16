using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class PlaceDownIndicator : MonoBehaviour
    {
        //private SpriteRenderer _sr;
        private GameObject _spriteHolder;
        private Slot _focusSlotRef;

        private void Awake()
        {
            //_sr = GetComponent<SpriteRenderer>();
            _spriteHolder = transform.GetChild(0).gameObject;

            GameSignals.FOCUS_SLOT_UPDATED.AddListener(FocusSlotHandle);
            GameSignals.CURSOR_ENTERED_NEW_TILE.AddListener(UpdatePositionToNewCenterTile);
        }

        private void OnDestroy()
        {
            GameSignals.FOCUS_SLOT_UPDATED.RemoveListener(FocusSlotHandle);
            GameSignals.CURSOR_ENTERED_NEW_TILE.RemoveListener(UpdatePositionToNewCenterTile);
        }

        private void FocusSlotHandle(ISignalParameters parameters)
        {
            if (parameters.HasParameter("FocusSlot"))
            {
                _focusSlotRef = (Slot)parameters.GetParameter("FocusSlot");

                SpriteHandle();
            }
        }

        private void UpdatePositionToNewCenterTile(ISignalParameters parameters)
        {
            if (parameters.HasParameter("CenterPosition"))
            {
                transform.position = (Vector2)parameters.GetParameter("CenterPosition");

                SpriteHandle();

                Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

                foreach (Collider2D col in colliders)
                {
                    if (col.gameObject.layer == 3)
                    {
                        _spriteHolder.SetActive(false);
                        return;
                    }
                }
            }
        }

        private void SpriteHandle()
        {
            _spriteHolder.SetActive(false);

            if(_focusSlotRef == null)
            {
                _spriteHolder.SetActive(false);
                return;
            }

            if (_focusSlotRef.ItemObject == null)
                _spriteHolder.SetActive(false);

            if (_focusSlotRef.ItemObject is DeployObject || _focusSlotRef.ItemObject is WallObject || _focusSlotRef.ItemObject is FloorObject)
                _spriteHolder.SetActive(true);
        }
    }
}
