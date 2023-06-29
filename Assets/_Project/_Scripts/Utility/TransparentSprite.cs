using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandBoy
{
    public class TransparentSprite : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Color _transparentColor;
        private Color _originalColor;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transparentColor = _spriteRenderer.color;
            _transparentColor.a = 0.25f;
            _originalColor = _spriteRenderer.color;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.TryGetComponent(out Entity entity))
            {
                _spriteRenderer.color = _transparentColor;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.TryGetComponent(out Entity entity))
            {
                _spriteRenderer.color = _originalColor;
            }
        }
    }
}
