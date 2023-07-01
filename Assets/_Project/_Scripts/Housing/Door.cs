using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandBoy
{
    public class Door : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PlayerReference _pr;
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private Sprite _closeSprite;
        [SerializeField] private AudioClip _doorOpenSound;
        [SerializeField] private AudioClip _doorCloseSound;

        private Collider2D _doorCollider;
        private SpriteRenderer _sr;
        private bool _opened;

        private void Awake()
        {
            _doorCollider = transform.GetChild(0).GetComponent<Collider2D>();
            _sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Close();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right && _pr.PlayerInRange(transform.position))
            {
                if (_opened)
                    Close();
                else
                    Open();
            }
        }

        private void Open()
        {
            _doorCollider.isTrigger = true;
            _opened = true;
            _sr.sprite = _openSprite;
            AudioManager.Instance.PlayClip(_doorOpenSound, false, true);
        }

        private void Close()
        {
            _doorCollider.isTrigger = false;
            _opened = false;
            _sr.sprite = _closeSprite;
            AudioManager.Instance.PlayClip(_doorCloseSound, false, true);
        }
    }
}
