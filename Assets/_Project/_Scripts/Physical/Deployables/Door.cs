using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class Door : Interactable
    {
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private Sprite _closeSprite;
        [SerializeField] private AudioClip _doorOpenSound;
        [SerializeField] private AudioClip _doorCloseSound;

        private Collider2D _doorCollider;
        private SpriteRenderer _sr;
        private bool _opened;

        public override void Awake()
        {
            base.Awake();
            _doorCollider = transform.GetChild(1).GetComponent<Collider2D>();
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

        }

        public override IEnumerator Start()
        {
            Close();

            return base.Start();
        }

        public override void Interact()
        {
            if (!_canInteract) return;

            if (_opened)
                Close();
            else
                Open();
        }

        public void Open()
        {
            _doorCollider.isTrigger = true;
            _opened = true;
            _sr.sprite = _openSprite;
            AudioManager.Instance.PlayClip(_doorOpenSound, false, true);
        }

        public void Close()
        {
            _doorCollider.isTrigger = false;
            _opened = false;
            _sr.sprite = _closeSprite;
            AudioManager.Instance.PlayClip(_doorCloseSound, false, true);
        }
    }
}
