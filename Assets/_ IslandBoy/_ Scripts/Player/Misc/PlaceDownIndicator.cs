using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IslandBoy
{
    public class PlaceDownIndicator : MonoBehaviour
    {
        [SerializeField] private PlayerObject _pr;

        private GameObject _spriteHolder;
        private Slot _focusSlotRef;

        private GameObject _itemSpriteHolder;
        private SpriteRenderer _image;
        private Sprite _itemSprite;

        public Sprite ItemSprite { get { return _itemSprite; } set { _itemSprite = value; _image.sprite = _itemSprite; } }

        private void Awake()
        {
            _spriteHolder = transform.GetChild(0).gameObject;

            _itemSpriteHolder = _spriteHolder.transform.GetChild(1).gameObject;
            _image = _itemSpriteHolder.GetComponent<SpriteRenderer>();
            _pr.PlaceDownIndicator = gameObject.GetComponent<PlaceDownIndicator>();

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
                if (_focusSlotRef != null && _focusSlotRef.ItemObject != null) ItemSprite = _focusSlotRef.ItemObject.UiDisplay;
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
                        _itemSpriteHolder.SetActive(false);
                        return;
                    }
                }
            }
        }

        private void SpriteHandle()
        {
            _spriteHolder.SetActive(false);
            _itemSpriteHolder.SetActive(false);

            if (_focusSlotRef == null)
            {
                _spriteHolder.SetActive(false);
                _itemSpriteHolder.SetActive(false);
                return;
            }

            if (_focusSlotRef.ItemObject == null)
            {
                _spriteHolder.SetActive(false);
                _itemSpriteHolder.SetActive(false);
            }

            if (_focusSlotRef.ItemObject is DeployObject || _focusSlotRef.ItemObject is WallObject || _focusSlotRef.ItemObject is FloorObject)
            {
                _spriteHolder.SetActive(true);

                if(_focusSlotRef.ItemObject is FurnitureObject)
                    _itemSpriteHolder.SetActive(true);
            }
        }
    }
}
