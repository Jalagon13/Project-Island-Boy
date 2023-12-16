using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class Door : Interactable
    {
        [Header("Base Door Parameters")]
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private Sprite _closeSprite;
        [SerializeField] private AudioClip _doorOpenSound;
        [SerializeField] private AudioClip _doorCloseSound;

        private Collider2D _doorCollider;
        private bool _opened;

        protected override void Awake()
        {
            base.Awake();
            _doorCollider = transform.GetChild(1).GetComponent<Collider2D>();
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

        }

        public override void Interact()
        {
            _instructions.gameObject.SetActive(false);

            if (!_canInteract) return;
            if (_opened)
                Close();
            else
                Open();
        }

        public void Open()
        {
            _doorCollider.gameObject.SetActive(false);
            _opened = true;
            _sr.sprite = _openSprite;

            Bounds updateBounds = new(transform.position, new(2, 2, 1));
            MMSoundManagerSoundPlayEvent.Trigger(_doorOpenSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
            AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
        }

        public void Close()
        {
            _doorCollider.gameObject?.SetActive(true);
            _opened = false;
            _sr.sprite = _closeSprite;

            Bounds updateBounds = new(transform.position, new(2, 2, 1));
            MMSoundManagerSoundPlayEvent.Trigger(_doorCloseSound, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
            AstarPath.active.UpdateGraphs(updateBounds, 0.1f);
        }

        public override void ShowDisplay()
        {
            _yellowCorners.gameObject.SetActive(true);
            _instructions.gameObject.SetActive(true);
        }
    }
}
